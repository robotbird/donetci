using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using Microsoft.Build.BuildEngine;

using SourceSafeTypeLib;
//using Microsoft.VisualStudio.SourceSafe.Interop;
using Utry.Framework;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Framework.Utils;
using Utry.Core.Domain;
using Utry.Core.Services;
using Utry.CI.Models;

namespace Utry.CI.Controllers
{
    public class CheckItemController : BaseController
    {
        private CheckItemService _checkItemService = new CheckItemService();

        #region 代码提交
        /// <summary>
        /// 需求及缺陷列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckList(CheckItemListModel model)
        {
            var planlist = new VersionPlanService().GetPlanList();
            model.PlanList = planlist;
            model.PlanSelectItem = planlist.ConvertAll(c => new SelectListItem { Text = c.PlanCode, Value = c.ID });
            model.PlanSelectItem.Add(new SelectListItem { Text = "计划编号(全部)", Value = "", Selected = true });

            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            //如果角色为分组后的开发人员
            if (CurrentUser.Role != "管理员" && !String.IsNullOrEmpty(CurrentUser.OrgCode))
            {
                where += " and u.orgcode= '" + CurrentUser.OrgCode + "'";
            }
            //如果角色为没有分组后的开发人员
            if (CurrentUser.Role != "管理员" && String.IsNullOrEmpty(CurrentUser.OrgCode))
            {
                where += " and c.username= '" + CurrentUser.UserName + "'";
            }
            //需求bug编号
            if (!string.IsNullOrEmpty(model.DemandCode) && Utils.IsSafeSqlString(model.DemandCode))
            {
                where += " and c.DemandCode like '%" + model.DemandCode + "%' ";
            }
            //时间选择
            if (!string.IsNullOrEmpty(model.datefrom))
            {
                where += " and c.updatetime>='" + model.datefrom + "' ";
            }
            if (!string.IsNullOrEmpty(model.dateto))
            {
                where += " and c.updatetime<='" + model.dateto + "' ";
            }
            //计划编号
            if (!string.IsNullOrEmpty(model.PlanId))
            {
                where += " and c.PlanId='" + model.PlanId + "'";
            }
            var list = _checkItemService.GetCheckItemPageList(pageSize, pageIndex, out count, where);
            model.PageList.LoadPagedList(list);
            model.ItemList = (List<CICheckItem>)list;
            return View(model);
        }

        /// <summary>
        /// 提交物编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ItemEdit(string Id)
        {
            var model = new CheckItemModel();
            if (Id != null)
            {
                model.CheckItem = _checkItemService.GetCheckItem(Id);
                if (!string.IsNullOrEmpty(model.CheckItem.PlanId)) 
                {
                    model.plan = new VersionPlanService().GetPlan(model.CheckItem.PlanId);                
                }
            }
            return View(model);
        }
   
        /// <summary>
        /// 保存checkitem
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ItemEdit(CICheckItem Item)
        {
            var model = new CheckItemModel();
            model.CheckItem = Item;

            bool success = true;
            //验证代码清单
            #region 格式验证
            Item.CodeList = Item.CodeList.Trim().Replace("（", "(").Replace("）", ")");
            //if (Item.CodeList == "") 
            //{
            //    ErrorNotification("代码不能为空");
            //    success = false;
            //}
            if (Item.DemandCode == "")
            {
                ErrorNotification("需求编号不能为空");
                success = false;
            }
            var attachment = Request.Files["AttachmentFile"];
            if ((attachment == null||attachment.FileName =="")&&string.IsNullOrEmpty(Item.Attachment))
            {
                success = false;
                ErrorNotification("提交物不能为空");
            }
            else 
            {
                if (attachment != null && attachment.FileName != "")
                {
                    var folder = "/"+ConfigHelper.GetValue("uploadpath")+ "/" +CurrentUser.UserName ;

                    if (!System.IO.Directory.Exists(Server.MapPath(folder))) 
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(folder));                    
                    }
                    var savepath = Server.MapPath(folder+ "/" + attachment.FileName);
                    if (System.IO.File.Exists(savepath)) 
                    {
                        System.IO.File.Delete(savepath);
                    }
                    attachment.SaveAs(savepath);
                    Item.Attachment = Request.Url.Authority  + folder + "/" + attachment.FileName;
                    if (Item.Attachment.IndexOf("http://") < 0) 
                    {
                        Item.Attachment = "http://" + Item.Attachment;
                    }
                }
            }
            if (Item.DeployNote == "") 
            {
                success = false;
                ErrorNotification("部署注意事项不能为空");
            }

            var codelist = Item.CodeList;

            if (!string.IsNullOrEmpty(codelist)) 
            {
                codelist = codelist.Replace("/", @"\");//统一替换成反斜杠
                string[] stringSeparators = new string[] { "\r\n" };
                string[] files = codelist.Split(stringSeparators, StringSplitOptions.None);
                foreach (var file in files)
                {
                    //正则验证:^ccms_7|ccms_6(\/[\w-]*)(新增)|(修改)$
                    //必须是ccms_7或者ccms_v6开头
                    var v6 = file.ToLower().IndexOf(@"ccms_v6\");
                    var v7 = file.ToLower().IndexOf(@"ccms_v7\");
                    if (v6 != 0 && v7 != 0)
                    {
                        ErrorNotification(@"代码必须是ccms_v7\或者ccms_v6\开头：" + file);
                        success = false;
                    }
                    //(新增)或者(修改)结尾
                    var posfixadd = file.LastIndexOf("(新增)");
                    var postfixmod = file.LastIndexOf("(修改)");
                    if (posfixadd == -1 && postfixmod == -1)
                    {
                        success = false;
                        ErrorNotification("代码必须是(新增)或者(修改)结尾：" + file);
                    }
                    // 中间必须包含这个斜杠 /
                    if (file.IndexOf(@"\") <= 0)
                    {
                        success = false;
                        ErrorNotification(@"代码中间必须包含这个斜杠 \：" + file);
                    }
                    // 不能包含这个斜杠 \
                    //if (file.IndexOf("\\") >= 0)
                    //{
                    //    success = false;
                    //    ErrorNotification("代码不能包含这个斜杠 \\：" + file);
                    //}
                }
            }
            
            #endregion

            if (string.IsNullOrEmpty(Item.ID))
            {
                Item.AddTime = DateTime.Now;
                Item.UpdateTime = DateTime.Now;
                Item.Developer = CurrentUser.FullName;
                Item.UserName = CurrentUser.UserName;
                //验证需求编号
                if (!_checkItemService.ExistsDemandCode(Item.DemandCode))
                {
                    success = false;
                    ErrorNotification("需求或者bug编号不存在");
                    return View(model);
                }
                int num = 0;
                if (success) 
                {
                    Item.ID = Guid.NewGuid().ToString();
                    num = _checkItemService.InsertCheckItem(Item);                
                }
                if (num > 0)
                {
                    SuccessNotification("添加成功");
                }
                else
                {
                    ErrorNotification("添加失败");
                    return View(model);
                }
            }
            else
            {
                var oldmodel = _checkItemService.GetCheckItem(Item.ID);
                Item.AddTime = oldmodel.AddTime;
                Item.GetVssCnt = oldmodel.GetVssCnt;
                Item.Developer = oldmodel.Developer;
                Item.UpdateTime = DateTime.Now;
                Item.PlanId = oldmodel.PlanId;
                Item.UserName = oldmodel.UserName;
                model.plan = new VersionPlanService().GetPlan(Item.PlanId);
                int num = 0;
                if (success) 
                {
                    num = _checkItemService.UpdateCheckItem(Item);                
                }
                if (num > 0)
                {
                    SuccessNotification("修改成功");
                }
                else
                {
                    ErrorNotification("修改失败");
                    return View(model);
                }
            }
            if (success)
            {
                var url = PressRequest.GetUrlReferrer();
                return Redirect(url);
            }
            else 
            {
                return View(model);            
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult DeleteItem(string Id) 
        {
            var num = 0;
            num = _checkItemService.DeleteCheckItem(Id);
            if (num > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("checklist", "checkitem"));
            }
            else 
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("checklist", "checkitem"));                                    
            }
        }

        public ActionResult UploadFile()
        {
            var file = Request.Files["uploadify"];
            var folder = Request["folder"];
            return Json("1");
        }

        /// <summary>
        /// 添加需求到计划
        /// </summary>
        /// <returns></returns>
        public ActionResult AddItemToPlan() 
        {
            var model = new CheckItemModel();
            var planlist = new VersionPlanService().GetPlanList();
            var openplan = from plan in planlist where plan.OpenDate==DateTime.Now.ToString("yyyy-MM-dd") select plan;
            if (openplan != null&&openplan.Count()>0) 
            {
                planlist = openplan.ToList<CIVersionPlan>();
                model.PlanSelectItem = planlist.ConvertAll(c => new SelectListItem { Text = c.PlanCode, Value = c.ID });            
            } 
            else 
            {
                model.PlanSelectItem.Add(new SelectListItem { Text="暂无开放计划",Value=""});            
            }
            return View(model);
        }
        /// <summary>
        /// 保存编号到计划
        /// </summary>
        [HttpPost]
        public ActionResult AddItemToPlan(CICheckItem Item)
        {
            var model = new CheckItemModel();
            model.CheckItem = Item;

            bool success = true;
            //验证代码清单
            #region 格式验证
            if (Item.DemandCode == "")
            {
                ErrorNotification("需求编号不能为空");
                success = false;
            }
            #endregion

            if (string.IsNullOrEmpty(Item.ID))
            {
                Item.AddTime = DateTime.Now;
                Item.UpdateTime = DateTime.Now;
                Item.Developer = CurrentUser.FullName;
                Item.UserName = CurrentUser.UserName;
                Item.Status = "计划中";
                //验证需求编号
                if (!_checkItemService.ExistsDemandCode(Item.DemandCode))
                {
                    success = false;
                    ErrorNotification("需求或者bug编号不存在");
                    return AddItemToPlan();
                }
                int num = 0;
                if (success)
                {
                    Item.ID = Guid.NewGuid().ToString();
                    num = _checkItemService.InsertCheckItem(Item);
                }
                if (num > 0)
                {
                    SuccessNotification("添加成功");
                }
                else
                {
                    ErrorNotification("添加失败");
                    return AddItemToPlan();
                }
            }
           
            if (success)
            {
                return Redirect(Url.Action("checklist", "checkitem"));
            }
            else
            {
                return AddItemToPlan();
            }
        }

        #endregion

        #region 代码发布
        /// <summary>
        /// 发布列表
        /// </summary>
        /// <returns></returns>
        public ActionResult DeployList(CheckItemListModel model)
        {
            model.VersionState = Request["VersionState"];
            model.TestState = Request["TestState"];
            model.CodeState = Request["CodeState"];
            model.IsNewCode = Request["IsNewCode"];
            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = GetWhere(model);

            var planlist = new VersionPlanService().GetPlanList();
            model.PlanSelectItem = planlist.ConvertAll(c => new SelectListItem { Text = c.PlanCode, Value = c.ID });
            model.PlanSelectItem.Add(new SelectListItem {Text="计划编号(全部)",Value="",Selected=true });

            var itemlist = _checkItemService.GetCheckItemPageList(pageSize, pageIndex, out count, where);

            //检查代码重复
            List<string> allcode = new List<string>();//所有代码
            List<string> repcode = new List<string>();//重复的代码
            foreach (var item in itemlist)
            {
                var codelist =TypeConverter.ObjectToString(item.CodeList);
                var codefile = codelist.Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace("新增", "").Replace("修改", "");
                string[] files = codefile.Split('\n');
                foreach (var file in files)
                {
                    var vfile = file.Replace("\r", "").Replace(" ", "").Trim();
                    if (allcode.Contains(vfile))
                    {
                        repcode.Add(vfile);
                    }
                    allcode.Add(vfile);
                }
            }

            #region 判断当前页是否有重复代码
            var fitemlist = new List<CICheckItem>();
            /*foreach (var item in itemlist)
            {
                foreach (var code in repcode)
                {
                    if (item.CodeList != null) 
                    {
                        if (item.CodeList.Contains(code))
                        {
                            if (code.IndexOf("重复") <= 0)
                            {
                                item.CodeList = item.CodeList.Replace(code + "(", code + "(重复)(");
                            }
                        }
                    }
                }
                fitemlist.Add(item);
            }
            //代码重复
            if (model.CodeState == "1") 
            {
                fitemlist = fitemlist.Where(i => i.CodeList.Contains("(重复)")).ToList(); ;
            }
            if (model.CodeState == "0") 
            {
                fitemlist = fitemlist.Where(i => !i.CodeList.Contains("(重复)")).ToList(); ;
            }*/
            #endregion

            //是否新增代码
            if (model.IsNewCode == "1")
            {
                fitemlist = fitemlist.Where(i => i.CodeList.Contains("(新增)")).ToList();
            }
            if (model.IsNewCode == "0")
            {
                fitemlist = fitemlist.Where(i => !i.CodeList.Contains("(新增)")).ToList();
            }

            model.PageList.LoadPagedList(itemlist);
            model.ItemList = itemlist.ToList();
            return View(model);
        }
        /// <summary>
        /// 获取筛选条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetWhere(CheckItemListModel model) 
        {
            var where = "";
            //版本状态
            if (model.VersionState == "1")
            {
                where += " and versionnum is not null";
            }
            else  if (model.VersionState == "0")
            {
                where += " and versionnum is  null";
            }
            //测试状态
            if (model.TestState == "1")
            {
                where += " and demandstate='测试通过'";
            }
            if (model.TestState == "0")
            {
                where += " and demandstate='测试未通过'";
            }
            //需求bug编号
            if (!string.IsNullOrEmpty(model.DemandCode) && Utils.IsSafeSqlString(model.DemandCode))
            {
                where += " and DemandCode like '%"+model.DemandCode+"%' ";
            }
            //时间选择
            if (!string.IsNullOrEmpty(model.datefrom)) 
            {
                where += " and c.updatetime>='"+model.datefrom+"' ";
            }
            if (!string.IsNullOrEmpty(model.dateto)) 
            {
                where += " and c.updatetime<='" + model.dateto + "' ";            
            }
            //计划编号
            if(!string.IsNullOrEmpty(model.PlanId))
            {
                where += " and PlanId='"+model.PlanId+"'";
            }
            return where;
        }
        /// <summary>
        /// 获取指定代码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCheckListCode()
        {
            var version = PressRequest.GetQueryInt("v");
            VSSDatabase vssDatabase = new VSSDatabaseClass();
            var VSSini = GetValue("VSSini");
            var VSSuser = GetValue("VSSuser");
            var VSSpwd = GetValue("VSSpwd");
            var CCMS_PRJ = GetValue("CCMS_PRJ");
            var CCMS_Local = GetValue("CCMS_Local");
            var ccmsversion = "";
            var ids = PressRequest.GetFormString("ItemId");
            bool success = true;

            try
            {
                vssDatabase.Open(@VSSini, VSSuser, VSSpwd);
                VSSItem vssitem = vssDatabase.get_VSSItem(CCMS_PRJ, false);
            }
            catch (COMException ex) 
            {
                ErrorNotification("VSS连接失败");
                success = false;
            }

            if (ids != "")
            {
                var idarr = ids.TrimEnd(',').Split(',');

                foreach (var id in idarr)
                {
                    var checkitem = _checkItemService.GetCheckItem(id);
                    var codelist = TypeConverter.ObjectToString(checkitem.CodeList);

                    if (version == 7) 
                    {
                        codelist = codelist.Replace("CCMS_V6", "CCMS_V7");
                        ccmsversion = "CCMS_V7";
                    }
                    if (version == 6)
                    {
                        codelist = codelist.Replace("CCMS_V7", "CCMS_V6");
                        ccmsversion = "CCMS_V6";
                    }

                    var codefile = codelist.Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace("新增", "").Replace("修改", "");
                    string[] files = codefile.Split('\n');
                    foreach (string file in files)
                    {
                        var vfile = file.Replace("\r", "").Replace(" ", "").Trim();
                        var localfile = vfile.Replace("/", "\\");
                        string vssfile = CCMS_PRJ + vfile;
                        string localFile = CCMS_Local + localfile;

                        try 
                        {
                            VSSItem vssFolder = vssDatabase.get_VSSItem(vssfile, false);
                            //更新之前先删除文件
                            if (System.IO.File.Exists(localFile))
                            {
                                System.IO.FileInfo f = new System.IO.FileInfo(localFile);
                                f.Attributes = FileAttributes.Normal;//解除只读权限
                                f.Delete();
                            }
                            vssFolder.Get(localFile, 0);//获取到本地文件夹
                            
                            #region 自动添加到工程文件
                            var vbproj = "";
                            var projectName = "";

                            if (localfile.EndsWith(".aspx"))
                            {
                                vbproj = "SCH.vbproj";
                                projectName = CCMS_Local+ccmsversion+@"\web\"+vbproj;
                            }
                            if (localfile.EndsWith(".vb") && localfile.IndexOf(".aspx") < 0) 
                            {
                                var t1 = localfile.Substring(ccmsversion.Length, localfile.Length - ccmsversion.Length);
                               var foldname = t1.Substring(1,t1.LastIndexOf(@"\")-1);
                                vbproj = foldname+".vbproj";
                                projectName = CCMS_Local + ccmsversion +@"\"+foldname + @"\"+ vbproj;
                            }

                            if (!string.IsNullOrEmpty(projectName)) 
                            {
                                Project project = new Project();
                                project.Load(projectName);

                                var foldname = localfile.Substring(0, localfile.LastIndexOf(@"\"));
                                var include = localfile.Substring(foldname.LastIndexOf(@"\") + 1, localfile.Length - foldname.LastIndexOf(@"\") - 1);

                                var isnew = true;
                                foreach (BuildItemGroup itemGroup in project.ItemGroups)
                                {
                                    foreach (BuildItem item in itemGroup)
                                    {
                                        if ((item.Include.ToLower() == include.ToLower()) || (include.ToLower().LastIndexOf(item.Include.ToLower())>0))
                                        {
                                            isnew = false;
                                            break;
                                        }
                                    }
                                }
                                if (isnew && localfile.EndsWith(".aspx"))
                                {
                                    var includedesigner = include + ".designer.vb";
                                    var includevb = include + ".vb";

                                    var itemGroup = project.AddNewItemGroup();
                                    var buildItem = itemGroup.AddNewItem("Content", include);

                                    var buildItemdesigner = itemGroup.AddNewItem("Compile", includedesigner);
                                    var filename = include.Substring(include.LastIndexOf(@"\") + 1, include.Length - include.IndexOf(@"\") - 1);
                                    buildItemdesigner.SetMetadata("DependentUpon", filename);

                                    var buildItemvb = itemGroup.AddNewItem("Compile", includevb);
                                    buildItemvb.SetMetadata("DependentUpon", filename);
                                    buildItemvb.SetMetadata("SubType", "ASPXCodebehind");
                                    project.Save(projectName);
                                }
                                else if (isnew && localfile.EndsWith(".vb") && vbproj != "SCH.vbproj") //非web工程
                                {
                                    var itemGroup = project.AddNewItemGroup();
                                    var buildItem = itemGroup.AddNewItem("Compile", include.Substring(include.IndexOf(@"\") + 1));
                                    project.Save(projectName);
                                }
                            }
                            
                            #endregion
                        }
                        catch (COMException ex)
                        {
                            ErrorNotification("代码不存在：" + file);
                            success = false;
                        }
 
                    }

                    checkitem.GetVssCnt += 1;//增加获取次数
                    _checkItemService.UpdateCheckItem(checkitem,null);
                }
                if (success) 
                {
                    SuccessNotification("获取代码成功");                
                }
            }
            else
            {
                ErrorNotification("获取代码失败");
            }
           // return Redirect(Url.Action("deploylist", "checkitem"));
            var url = PressRequest.GetUrlReferrer();
            return Redirect(url);
        }

        public ActionResult showvss()
        {
            var version = PressRequest.GetQueryInt("v");
            VSSDatabase vssDatabase = new VSSDatabaseClass();
            var VSSini = GetValue("VSSini");
            var VSSuser = GetValue("VSSuser");
            var VSSpwd = GetValue("VSSpwd");
            var CCMS_PRJ = GetValue("CCMS_PRJ");
            var CCMS_Local = GetValue("CCMS_Local");
            var ccmsversion = "";

            vssDatabase.Open(@VSSini, VSSuser, VSSpwd);
            VSSItem vssitem = vssDatabase.get_VSSItem(CCMS_PRJ, false);

            var s = "";
            s = string.Format("<br/>\n{0} contains:", vssitem.Spec);


            foreach (VSSItem vssItem in vssitem.get_Items(false))
            {
                s += string.Format("<br/>--{0}", vssItem.Name);
                var versions= vssitem.get_Versions();

                foreach (VSSVersion v in versions)
                {
                    s += "<br/>----" + v.Username + "," + v.Date + "," + v.VersionNumber;
                }
            }

            return Content(s);
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportToExcel(CheckItemListModel model) 
        {

            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);
            var where = GetWhere(model);

            var itemlist = _checkItemService.GetCheckItemPageList(pageSize, pageIndex, out count, where);

            var fitemlist = (List<CICheckItem>)itemlist;

            //代码重复
            if (model.CodeState == "1")
            {
                fitemlist = fitemlist.Where(i => i.CodeList.Contains("(重复)")).ToList(); ;
            }
            if (model.CodeState == "0")
            {
                fitemlist = fitemlist.Where(i => !i.CodeList.Contains("(重复)")).ToList(); ;
            }
            //是否新增代码
            if (model.IsNewCode == "1")
            {
                fitemlist = fitemlist.Where(i => i.CodeList.Contains("(新增)")).ToList();
            }
            if (model.IsNewCode == "0")
            {
                fitemlist = fitemlist.Where(i => !i.CodeList.Contains("(新增)")).ToList();
            }

            DataTable dt = new DataTable();
            ExcelHelper.ExportByWeb(ConvertToDatatable(fitemlist), "提交物清单:", "提交物清单" + string.Format("{0:yyyyMMddHHmmssffff}",DateTime.Now) + ".xls");



            SuccessNotification("导出excel成功");
            return Redirect(Url.Action("deploylist", "checkitem"));

        }
        /// <summary>
        /// 转换datatable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static DataTable ConvertToDatatable(List<CICheckItem> list)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("UTMP版本号");
            dt.Columns.Add("计划编号");
            dt.Columns.Add("需求编号");
            dt.Columns.Add("资源列表");
            dt.Columns.Add("提交物");
            dt.Columns.Add("验证方法");
            dt.Columns.Add("部署注意事项");
            dt.Columns.Add("utmp状态");
            dt.Columns.Add("获取次数");
            dt.Columns.Add("提交时间");
            dt.Columns.Add("开发人员");
            dt.Columns.Add("测试人员");
            foreach (var item in list)
            {
                var row = dt.NewRow();

                row["UTMP版本号"] = item.versionnum;//版本号
                row["计划编号"] = item.PlanCode;
                row["需求编号"] = item.DemandCode;
                row["资源列表"] = item.CodeList;
                row["提交物"] = item.Attachment;
                row["验证方法"] = item.ValidateNote;
                row["部署注意事项"] = item.DeployNote;
                row["utmp状态"] = item.demandstate;
                row["获取次数"] = item.GetVssCnt;
                row["提交时间"] = item.UpdateTime;
                row["开发人员"] = item.Developer;
                row["测试人员"] = "";
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];

        }
        #endregion
    }
}
