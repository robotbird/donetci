using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utry.CI.Models;
using Utry.Core.Domain;
using Utry.Core.Services;
using Utry.Framework.Utils;
using Utry.Framework.Web;

namespace Utry.CI.Controllers
{
    public class UsersController : BaseController
    {
        //
        // GET: /Users/

        private UserService _userService = new UserService();

        /// <summary>
        /// 开发人员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UsersList()
        {
            UserModel model = new UserModel();
            model.Users = _userService.GetAllUserInfo();
            return View(model);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserAdd(CIUser user)
        {
            user.PassWord = EncryptHelper.MD5(user.PassWord);
            user.RegTime = DateTime.Now;
            _userService.InsertUser(user);
            return Redirect(Url.Action("Userslist", "Users"));
        }




        /// <summary>
        /// 编辑人员所属小组
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserEdit(string username)
        {
            UserModel model = new UserModel();
            var _userorgService = new UserOrgService();
            if (!String.IsNullOrEmpty(username))
            {
                model.Action = "UserEdit";
                model.User = _userService.GetUser(username);
            }
            else
            {
                model.Action = "UserAdd";
            }
            model.UserOrg = _userorgService.GetUserOrgList();
            model.UserSelect = model.UserOrg.ConvertAll(c => new SelectListItem { Text = c.OrgName, Value = c.OrgCode });
            model.UserSelect.Add(new SelectListItem { Text = "选择所属小组", Value = "", Selected = true });
            model.UserRole = new List<SelectListItem>();
            model.UserRole.Add(new SelectListItem { Text = "选择所属角色", Value = "", Selected = true });
            model.UserRole.Add(new SelectListItem { Text = "开发人员", Value = "开发人员" });
            model.UserRole.Add(new SelectListItem { Text = "开发组长", Value = "开发组长" });
            model.UserRole.Add(new SelectListItem { Text = "版本发布人员", Value = "版本发布人员" });
            model.UserRole.Add(new SelectListItem { Text = "管理员", Value = "管理员" });
            
            return View(model);
        }

        /// <summary>
        /// 编辑人员信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserEdit(CIUser user)
        {
            if (string.IsNullOrEmpty(user.PassWord))
            {
                user.PassWord = PressRequest.GetFormString("hidPwd");
            }
            else
            {
                user.PassWord = EncryptHelper.MD5(user.PassWord);
            }
            _userService.UpdateUserOrg(user);
            return Redirect(Url.Action("Userslist", "Users"));
        }


        /// <summary>
        /// 删除开发人员
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ActionResult UsersDelete(string username)
        {
            var result = _userService.DeleteUser(username);
            if (result > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("Userslist", "Users"));
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("Userslist", "Users"));
            }
        }

        /// <summary>
        /// 人员选择树形列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UserSelect(string username,string fullname)
        {
            UserModel model= new UserModel();
            model.User = new CIUser();
            model.User.UserName = username+",";
            model.User.FullName = fullname+",";
            return View(model);
        }


        public ActionResult ChooseUser(string username)
        {
            UserModel model = new UserModel();
            model.Users = _userService.GetUserList();
            var strJson = "[{ name:'开发部',iconOpen:'/Assets/zTree/css/zTreeStyle/img/diy/1_close.png',iconClose:'/Assets/zTree/css/zTreeStyle/img/diy/1_open.png',id:0,children:[";
            var usernames = username.Split(',');
            foreach (var item in model.Users)
            {
                if (usernames.Length > 0)
                {
                    for (int i = 0; i < usernames.Length; i++)
                    {
                        if (item.UserName == usernames[i])
                        {
                            strJson += "{ name:'" + item.FullName + "',checked:true,icon:'/Assets/zTree/css/zTreeStyle/img/diy/10.gif',id:'" + item.UserName + "'},";
                            i = usernames.Length;
                        }
                        else if (item.UserName != usernames[i] && i == usernames.Length-1)
                        {
                            strJson += "{ name:'" + item.FullName + "',icon:'/Assets/zTree/css/zTreeStyle/img/diy/10.gif',id:'" + item.UserName + "'},";
                        }
                        else
                        { 
                            
                        }
                    }
                }
                
            }
            strJson += "]}];";
            #region "简单json格式"

            //List<object> list =new List<object>();
            //var strJson = "[{id='1',pid='0',name:'开发部'}";
            //list.Add(strJson);
            //for (int i = 0; i < model.Users.Count; i++)
            //{
            //    strJson = "{id:'" + model.Users[i].UserName + "',pid='1',name='" + model.Users[i].FullName + "'}";
            //    list.Add(strJson);
            //}
            //strJson = "]";
            //list.Add(strJson);
            #endregion
            return Content(strJson);
        }

        /// <summary>
        /// 人员树根据输入姓名搜索
        /// </summary>
        /// <param name="userfullname"></param>
        /// <returns></returns>
        public ActionResult SearchUser(string fullname)
        {
            UserModel model = new UserModel();
            model.Users = _userService.GetUserByFullName(fullname);
            var strJson = "[{ name:'开发部',iconOpen:'/Assets/zTree/css/zTreeStyle/img/diy/1_close.png',iconClose:'/Assets/zTree/css/zTreeStyle/img/diy/1_open.png',open:true,id:0,children:[";
            if (model.Users.Count > 0)
            {
                foreach (var item in model.Users)
                {
                    strJson += "{ name:'" + item.FullName + "',icon:'/Assets/zTree/css/zTreeStyle/img/diy/10.gif',id:'" + item.UserName + "'},";
                }
                strJson += "]}];";
            }
            else
            {
                strJson += "{ name:'',icon:'/Assets/zTree/css/zTreeStyle/img/diy/10.gif',id:''},";
            }
            
            return Content(strJson);
        }

        /// <summary>
        /// 人员下拉框多选可输入形式选择
        /// </summary>
        /// <returns></returns>
        public ActionResult UsersSelect()
        {
            var list = _userService.GetUserList();
            var data = "";
            foreach (var item in list)
            {
                data += item.FullName + "," + item.UserName+"|";
            }
            return Content(data);
        }


    }
}
