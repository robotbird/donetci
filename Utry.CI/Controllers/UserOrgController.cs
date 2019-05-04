using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Utry.Core.Domain;
using Utry.CI.Models;
using Utry.Framework.Web;
using Utry.Core.Services;

namespace Utry.CI.Controllers
{
    public class UserOrgController : BaseController
    {

        private UserOrgService _userorgService = new UserOrgService();

        public ActionResult UserOrgList()
        {
            UserOrgModel model = new UserOrgModel();
            model.UserOrgList = _userorgService.GetUserOrgList();
            return View(model);
        }


        [HttpGet]
        public ActionResult UserOrgAdd()
        {
            UserOrgModel model = new UserOrgModel();
            model.Action = "UserOrgAdd";
            model.UserOrg = new CIUserOrg();
            return View(model);
        }


        /// <summary>
        /// 添加小组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserOrgAdd(CIUserOrg userorg)
        {
            //CIUserOrg model = new CIUserOrg();
            //model.OrgName = PressRequest.GetFormString("orgname");
            //model.OrgCode = PressRequest.GetFormString("orgcode");
            _userorgService.InsertUserOrg(userorg);
            return Redirect(Url.Action("UserOrgList","UserOrg"));
        }

        [HttpGet]
        public ActionResult UserOrgEdit(string OrgCode)
        {
            UserOrgModel model = new UserOrgModel();
            model.Action = "UserOrgEdit";
            model.UserOrg = _userorgService.GetOrgByCode(OrgCode);
            return View("UserOrgAdd", model);
        }

        /// <summary>
        /// 提交修改
        /// </summary>
        /// <param name="userorg"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserOrgEdit(CIUserOrg userorg)
        {
            //CIUserOrg model = new CIUserOrg();
            //model.OrgName = userorg.OrgName;
            //model.OrgCode = userorg.OrgCode;
            var orgcode = PressRequest.GetFormString("hdOrgCode");
            _userorgService.UpdateUserOrgBystringCode(userorg, orgcode);
            return Redirect(Url.Action("UserOrgList", "UserOrg"));
        }


        public ActionResult UserOrgDelete(string OrgCode)
        {
            var result = _userorgService.DeleteUserOrg(OrgCode);
            if (result > 0)
            {
                SuccessNotification("删除成功");
                return Redirect(Url.Action("UserOrgList", "UserOrg"));
            }
            else
            {
                ErrorNotification("删除失败");
                return Redirect(Url.Action("UserOrgList", "UserOrg"));
            }
        }










        /// <summary>
        /// 检验输入的小组编号是否存在
        /// </summary>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public ActionResult CheckOrgCode(string OrgCode)
        {
            var orgcode = PressRequest.GetFormString("username");
            var msg = _userorgService.ExistUserOrgCode(orgcode) ? "1" : "0";
            return Content(msg);
        }



    }
}
