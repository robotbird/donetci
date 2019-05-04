using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utry.Core.Domain;
using Utry.Core.Services;
using Utry.Framework.Utils;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Framework.Log;
using Utry.CI.Models;


namespace Utry.CI.Controllers
{
    public class ReviewController : BaseController
    {
        private ReviewService _reviewService = new ReviewService();

        private UserService _userService = new UserService();

        private ReviewProblemService _reviewproService = new ReviewProblemService();

        private DemandService _demandService = new DemandService();


        public ActionResult Index()
        {
            return null;
        }

        public ActionResult ReviewList(ReviewModel model)
        {
            //model.ProjectList = _projectService.GetProInfoList();
            //model.ProjectSelectItem = model.ProjectList.ConvertAll(c => new SelectListItem { Text=c.ProjectName,Value = c.ProjectCode  });
            model.StatusSelectItem.Add(new SelectListItem { Text = "--选择状态--", Value = "", Selected = true });
            model.StatusSelectItem.Add(new SelectListItem { Text = "已评审", Value = "已评审" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "未通过", Value = "未通过" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "延期", Value = "延期" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "不评审", Value = "不评审" });

            const int pageSize = 10;
            int count = 0;
            int pageIndex = PressRequest.GetInt("page", 1);

            var where = "";
            if (!string.IsNullOrEmpty(PressRequest.GetFormString("Status")))
            {
                where += " and r.Status ='" + PressRequest.GetFormString("Status") + "' ";
            }
            if (!string.IsNullOrEmpty(model.ProjectName) && Utils.IsSafeSqlString(model.ProjectName))
            {
                where += " and p.ProjectName like '%" + model.ProjectName + "%' ";
            }
            if (!string.IsNullOrEmpty(model.DemandCode) && Utils.IsSafeSqlString(model.DemandCode))
            {
                where += " and rp.DemandCode like '%" + model.DemandCode + "%' ";
            }
            if (!string.IsNullOrEmpty(model.BeginDate))
            {
                where += " and r.BeginDate >= '" + model.BeginDate + "' ";
            }
            if (!string.IsNullOrEmpty(model.EndDate))
            {
                where += " and r.BeginDate <= '" + model.EndDate + "' ";
            }

            var list = _reviewService.GetReviewList(pageSize, pageIndex, out count, where);
            model.PageList.LoadPagedList(list);
            model.ReviewList = (List<CIReview>)list;
            return View(model);
        }

        public void UserSplit(List<CIReviewProblem> rplist)
        {
            #region 处理保存人员字符串
            if (rplist.Count > 0)
            {
                foreach (var item in rplist)
                {
                    var Proname = "";
                    var Solname = "";
                    if (item.Provider != "" && item.Provider != null)
                    {
                        var providername = item.Provider.Substring(0, item.Provider.Length - 1);
                        var providernames = providername.Split(',');
                        for (int i = 0; i < providernames.Length; i++)
                        {
                            if (providernames.Length == 1)
                            {
                                if (providernames[i] != "" && providernames[i] != ",")
                                {
                                    Proname += _userService.GetUser(providernames[i]).FullName + ","; 
                                }
                            }
                            else
                            {
                                if (providernames[i] != "" && providernames[i] != ",")
                                {
                                    Proname += _userService.GetUser(providernames[i]).FullName + ",";
                                }
                            }
                        }
                        Proname = Proname.Substring(0, Proname.Length - 1);
                        item.Provider = Proname;
                    }

                    if (item.Solver != "" && item.Solver != null)
                    {
                        var solver = item.Solver.Substring(0, item.Solver.Length - 1);
                        var solvers = solver.Split(',');
                        for (int i = 0; i < solvers.Length; i++)
                        {
                            if (solvers.Length == 1)
                            {
                                if (solvers[i] != "" && solvers[i] != ",")
                                {
                                    Solname += _userService.GetUser(solvers[i]).FullName + ",";
                                }
                            }
                            else
                            {
                                if (solvers[i] != "" && solvers[i] != ",")
                                {
                                    Solname += _userService.GetUser(solvers[i]).FullName + ",";
                                }
                            }
                        }
                        Solname = Solname.Substring(0, Solname.Length - 1);
                        item.Solver = Solname;
                    }
                }
            }
            #endregion
        }


        /// <summary>
        /// 需求列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ReviewProList(ReviewModel model)
        {
            model.ReviewProList = _reviewproService.GetReproList();
            UserSplit(model.ReviewProList);
            model.UserList = _userService.GetUserList();
            return View(model);
        }

        public void DropDownList(ReviewModel model)
        {
            model.ProjectList = new ProjectService().GetProInfoList();
            model.ProjectSelectItem = model.ProjectList.ConvertAll(c => new SelectListItem { Text = c.ProjectName, Value = c.ProjectCode });
            model.ProjectSelectItem.Add(new SelectListItem { Text = "--选择项目--", Value = "", Selected = true });

            model.StatusSelectItem.Add(new SelectListItem { Text = "--选择结果--", Value = "", Selected = true });
            model.StatusSelectItem.Add(new SelectListItem { Text = "已评审", Value = "已评审" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "未通过", Value = "未通过" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "延期", Value = "延期" });
            model.StatusSelectItem.Add(new SelectListItem { Text = "不评审", Value = "不评审" });

            model.MethodSelectItem.Add(new SelectListItem { Text = "--选择评审方式--", Value = "", Selected = true });
            model.MethodSelectItem.Add(new SelectListItem { Text = "会议", Value = "会议" });
            model.MethodSelectItem.Add(new SelectListItem { Text = "邮件", Value = "邮件" });
            model.MethodSelectItem.Add(new SelectListItem { Text = "走查", Value = "走查" });

            model.IfReviewSelectItem.Add(new SelectListItem { Text = "--选择是否复审--", Value = "", Selected = true });
            model.IfReviewSelectItem.Add(new SelectListItem { Text = "不需要", Value = "不需要" });
            model.IfReviewSelectItem.Add(new SelectListItem { Text = "需要", Value = "需要" });

            model.DemandList = _demandService.GetDemandList();
            model.DemandSelectItem = model.DemandList.ConvertAll(c => new SelectListItem { Text = c.DemandNumber, Value = c.DemandNumber });
            model.DemandSelectItem.Add(new SelectListItem { Text = "--请选择需求编号--", Value = "", Selected = true });

            model.IfSolveSelectItem.Add(new SelectListItem { Text = "--评审状态--", Value = "", Selected = true });
            model.IfSolveSelectItem.Add(new SelectListItem { Text = "已评审", Value = "已评审" });
            model.IfSolveSelectItem.Add(new SelectListItem { Text = "未通过", Value = "未通过" });
            model.IfSolveSelectItem.Add(new SelectListItem { Text = "延期", Value = "延期" });
            model.IfSolveSelectItem.Add(new SelectListItem { Text = "不评审", Value = "不评审" });
        }

        [HttpGet]
        public ActionResult ReviewAdd()
        {
            ReviewModel model = new ReviewModel();
            model.ID = Guid.NewGuid().ToString();
            model.Action = "ReviewAdd";
            model.UserList = _userService.GetUserList();
            #region "下拉框"
            DropDownList(model);
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult ReviewAdd(CIReview review)
        {
            try
            {
                review.ID = PressRequest.GetFormString("hidID");
                review.Member = PressRequest.GetFormString("hidUsername");

                var beginTime = PressRequest.GetFormString("BeginTime").Replace("/","-");
                var endTime = PressRequest.GetFormString("EndTime").Replace("/", "-");

                review.BeginDate = string.IsNullOrEmpty(beginTime) ? DateTime.Now : TypeConverter.ObjectToTime(beginTime);

                review.EndDate = string.IsNullOrEmpty(endTime) ? DateTime.Now : TypeConverter.ObjectToTime(endTime);

                if (PressRequest.GetFormString("hidIfAdd") == "1")
                {
                    review.UpdateTime = DateTime.Now;
                    _reviewService.UpdatePro(review);
                }
                else
                {
                    review.AddTime = DateTime.Now;
                    review.UpdateTime = DateTime.Now;
                    _reviewService.InsertPro(review);
                }
            }
            catch (Exception ex)
            {
               Logger.Error(ex);  
                ErrorNotification("保存出错："+ex.Message);
                return View();
            }
           

            if (PressRequest.GetFormString("hidIfPro") == "1")
            {
                return Redirect(Url.Action("projectinfo", "project", new { id = PressRequest.GetFormString("hidProID") }));
            }
            else
            {
                return Redirect(Url.Action("ReviewList", "Review"));
            }
        }

        [HttpGet]
        public ActionResult ReviewEdit(string id,string ifPro)
        {
            ReviewModel model = new ReviewModel();
            model.UserList = _userService.GetUserList();
            model.ID = id;
            model.IfAdd = "1";
            if (ifPro == "1")
            {
                model.IfPro = "1";
            }
            DropDownList(model);
            model.Action = "ReviewAdd";
            model.Review = _reviewService.GetById(id);

            if (model.Review == null)
            {
                return null;
            }
            var username = "";

            if (model.Review != null)
            {
                if (model.Review.Member.Length > 0)
                {
                    username = model.Review.Member.Substring(0, model.Review.Member.Length - 1);
                }
            }

            var usernames = username.Split(',');
            if (usernames.Length > 1)
            {
                for (int i = 0; i < usernames.Length; i++)
                {
                    model.User.FullName += _userService.GetUser(usernames[i]).FullName + ",";
                }
                model.User.FullName = model.User.FullName.Substring(0, model.User.FullName.Length - 1);
            }
            else
            {
                var user = _userService.GetUser(username);
                if (user != null)
                {
                   model.User.FullName += user.FullName ;
                }
            }
            if (model.Review != null)
            {
                model.ProjectCode = model.Review.ProjectCode;
                model.Method = model.Review.Method;
                model.Status = model.Review.Status;
                model.IfReview = model.Review.IfReview;
                model.BeginDate = model.Review.BeginDate.ToString();
                model.EndDate = model.Review.EndDate.ToString();
            }

            return View("ReviewAdd", model);
        }

        public ActionResult ReviewDelete(string id, string DelID)
        {
            int num = _reviewService.DeletePro(id);
            int numb = _reviewproService.DeleteByReviewId(id);
            if (num > 0 )
            {
                SuccessNotification("删除成功");
                if (string.IsNullOrEmpty(DelID))
                {
                    return Redirect(Url.Action("ReviewList", "Review"));
                }
                else
                {
                    return Redirect(Url.Action("Projectinfo", "Project", new { id =DelID }));
                }
                
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("ReviewList", "Review"));
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ActionResult DeleteFile(string filename)
        {
            var folder = @"~/" + ConfigHelper.GetValue("uploadpath") + "/" + CurrentUser.UserName;
            string path = Server.MapPath(folder+"/"+filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return Content("删除成功");
            }
            else
            {
                return Content("删除失败");                
            }
        }

        #region "评审问题"
        /// <summary>
        /// 评审添加页面中新增风险问题
        /// </summary>
        /// <param name="ReviewID"></param>
        /// <param name="DemandCode"></param>
        /// <param name="Provider"></param>
        /// <param name="Description"></param>
        /// <param name="Solver"></param>
        /// <param name="date"></param>
        /// <param name="ifsolve"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReviewProAdd(string ReviewID, string DemandCode, string Provider, string Description, string Solver, string date, string ifsolve, string DevelopTime, string DesignTime, string TestTime)
        {
            //ReviewProblemModel model = new ReviewProblemModel();
            //model.ReviewPro = reviewpro;
            CIReviewProblem reviewpro = new CIReviewProblem();
            int num = 0;
            reviewpro.ID = Guid.NewGuid().ToString();
            reviewpro.ReviewID = ReviewID;
            reviewpro.DemandCode = DemandCode;
            reviewpro.Provider = Provider;
            reviewpro.Solver = Solver;
            reviewpro.Description = Description;
            if (date == "")
            {
                date = DateTime.Now.ToString();
            }
            reviewpro.Deadline = Convert.ToDateTime(date);
            reviewpro.IfSolve = ifsolve;
            reviewpro.DevelopTime = DevelopTime;
            reviewpro.DesignTime = DesignTime;
            reviewpro.TestTime = TestTime;
            reviewpro.AddTime = DateTime.Now;
            reviewpro.UpdateTime = DateTime.Now;
            num = _reviewproService.InsertPro(reviewpro);
            if (num > 0)
            {
                return Content("提交成功");
            }
            else
            {
                return Content("提交失败");
            }
        }

        /// <summary>
        /// 需求列表中新增需求没有ReviewID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertReproWithOutDeadLine(string DemandCode, string Description, string Provider, string date, string Solver, string ifsolve,string DevelopTime,string DesignTime,string TestTime)
        {
            CIReviewProblem reviewpro = new CIReviewProblem();
            int num = 0;
            reviewpro.ID = Guid.NewGuid().ToString();
            reviewpro.ReviewID = "";
            reviewpro.DemandCode = DemandCode;
            reviewpro.Provider = Provider;
            reviewpro.Solver = Solver;
            reviewpro.Description = Description;
            reviewpro.IfSolve = ifsolve;
            reviewpro.DevelopTime = DevelopTime;
            reviewpro.DesignTime = DesignTime;
            reviewpro.TestTime = TestTime;
            reviewpro.AddTime = DateTime.Now;
            reviewpro.UpdateTime = DateTime.Now;
            if (date == "")
            {
                date = DateTime.Now.ToString();
            }
            reviewpro.Deadline = Convert.ToDateTime(date);
            
            num = _reviewproService.InsertPro(reviewpro);
            
            if (num > 0)
            {
                return Content("提交成功");
            }
            else
            {
                return Content("提交失败");
            }
        }

        public ActionResult UpdateRepro(string id, string DemandCode, string Description, string Provider, string date, string Solver, string ifsolve, string DevelopTime, string DesignTime, string TestTime)
        {
            CIReviewProblem reviewpro = new CIReviewProblem();
            reviewpro = _reviewproService.GetById(id);
            reviewpro.DemandCode = DemandCode;
            reviewpro.Description = Description;
            reviewpro.Provider = Provider;
            reviewpro.Solver = Solver;
            reviewpro.IfSolve = ifsolve;
            reviewpro.DevelopTime = DevelopTime;
            reviewpro.DesignTime = DesignTime;
            reviewpro.TestTime = TestTime;
            if (date == "")
            {
                date = DateTime.Now.ToString();
            }
            reviewpro.Deadline = Convert.ToDateTime(date);
            reviewpro.UpdateTime = DateTime.Now;

            int num = _reviewproService.UpdatePro(reviewpro);
            if (num > 0)
            {
                return Content("编辑成功");
            }
            else
            {
                return Content("编辑失败");
            }
            
        }

        public ActionResult GetReiewProAjax(string id)
        {
            ReviewModel model = new ReviewModel();
            model.ReviewProList = _reviewproService.GetAll(id);
            string html = "";
            if (model.ReviewProList.Count > 0)
            {
                foreach (var item in model.ReviewProList)
                {
                    var Proname = "";
                    var Solname = "";
                    if (item.Provider != "" && item.Provider != null)
                    {
                        var providername = item.Provider.Substring(0, item.Provider.Length - 1);
                        var providernames = providername.Split(',');
                        for (int i = 0; i < providernames.Length; i++)
                        {
                            if (providernames.Length == 1)
                            {
                                if (providernames[i] != "" && providernames[i] != ",")
                                {
                                    Proname += _userService.GetUser(providernames[i]).FullName + ",";
                                }
                            }
                            else
                            {
                                if (providernames[i] != "" && providernames[i] != ",")
                                {
                                    Proname += _userService.GetUser(providernames[i]).FullName + ",";
                                }
                            }

                        }
                        Proname = Proname.Substring(0, Proname.Length - 1);
                    }

                    if (item.Solver != "" && item.Solver != null)
                    {
                        var solver = item.Solver.Substring(0, item.Solver.Length - 1);
                        var solvers = solver.Split(',');
                        for (int i = 0; i < solvers.Length; i++)
                        {
                            if (solvers.Length == 1)
                            {
                                if (solvers[i] != "" && solvers[i] != ",")
                                {
                                    Solname += _userService.GetUser(solvers[i]).FullName + ",";
                                }
                                
                            }
                            else
                            {
                                if (solvers[i] != "" && solvers[i] != ",")
                                {
                                    Solname += _userService.GetUser(solvers[i]).FullName + ",";
                                }
                            }

                        }
                        Solname = Solname.Substring(0, Solname.Length - 1);
                    }



                    html += @"<tr>" +
                             "<td style='border-left: 0px;'>" + item.DemandCode + "</td>" +
                             "<td>" + item.Description + "</td>" +
                             "<td>" + Proname + "</td>" +
                             "<td>" + item.Deadline + "</td>" +
                             "<td>" + Solname + "</td>" +
                             "<td>" + item.demandstate + "</td>" +
                             "<td>" + item.IfSolve + "</td>" +
                             "<td>" + item.DevelopTime + "</td>" +
                             "<td>" + item.DesignTime + "</td>" +
                             "<td>" + item.TestTime + "</td>" +
                             "<td><input type='button' value='删除' name='del' class='btn btn-primary btn-sm' onclick=DeleteReviewPro('" + item.ID + "'); /></td>" +
                             "</tr>";
                }
            }
            return Content(html);
        }

        public ActionResult DeleteReviewPro(string id)
        {
            int num = 0;
            num = _reviewproService.DeletePro(id);
            if (num > 0)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// 需求列表编辑时获取人员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetReviewProByID(string id)
        {
            ReviewModel model = new ReviewModel();
            model.ReviewPro = _reviewproService.GetById(id);
            var username = model.ReviewPro.Provider + "|" + model.ReviewPro.Solver;
            return Content(username);
        }

        public ActionResult IfExistDemand(string demand)
        {
            var list = _reviewproService.GetReproByDemand(demand);
            var result = "";
            if (list.Count > 0)
            {
                result = "此需求编号已使用,请重新选择";
            }
            return Content(result);
        }

        public ActionResult Upload(HttpPostedFileBase Filedata)
        {
            // 如果没有上传文件
            if (Filedata == null ||
                string.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }

            var folder = "/" + ConfigHelper.GetValue("uploadpath") + "/" + CurrentUser.UserName;
            if (!System.IO.Directory.Exists(Server.MapPath(folder)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath(folder));
            }
            var savepath = Server.MapPath(folder + "/" + Filedata.FileName);
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }
            Filedata.SaveAs(savepath);

            var attachment = "";
            attachment = Request.Url.Authority + folder + "/" + Filedata.FileName;

            attachment = attachment.Replace("&", "and");
            attachment = attachment.Replace("?", "");
            attachment = attachment.Replace("=", "");
            if (attachment.IndexOf("http://") < 0)
            {
                attachment = "http://" + attachment;
            }
            return Content(attachment);
        }

        #endregion
    }
}