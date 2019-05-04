using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
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
    public class HomeController : BaseController
    {
        private UserService _userService = new UserService();
        private ProjectService _projectservice = new ProjectService();

        #region 测试用

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new ProjectModel();
            model.ProjectList =  _projectservice.GetProInfoList();
            return View(model);
            
        }

        /// <summary>
        /// 获取指定代码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCode() 
        {
            ViewBag.Title = "Utry持续集成平台";
            //VSSDatabase vssDatabase = new VSSDatabase(); 
            //try { vssDatabase.Open(txtDbPath.Text, txtAccount.Text, txtPasswd.Text); }
            //catch { MessageBox.Show("Can't login to the VSS database"); return; }
            //try { VSSItem vssitem = vssDatabase.get_VSSItem(txtItemPath.Text, false);
            //    VSSItem vssitemVersion = vssitem.get_Version(int.Parse(txtVersion.Text)); 
            //    string localPath = txtLocalpath.Text; vssitemVersion.Get(ref localPath, 0); 
            //    if (File.Exists(txtLocalpath.Text)) { MessageBox.Show("Succeed!"); } }
            //catch { MessageBox.Show("Download failed"); }

            VSSDatabase vssDatabase = new VSSDatabaseClass();
            //var VSSini = GetValue("VSSini");
            var VSSini = ConfigHelper.GetValue("VSSini");
            var VSSuser = ConfigHelper.GetValue("VSSuser");
            var VSSpwd = ConfigHelper.GetValue("VSSpwd");
            var CCMS_PRJ = ConfigHelper.GetValue("CCMS_PRJ");
            var CCMS_Local = ConfigHelper.GetValue("CCMS_Local");

            //WorkGroupNetwork iss = new WorkGroupNetwork("ccms", "ip", "ccms");

            vssDatabase.Open(@VSSini, VSSuser, VSSpwd);
            VSSItem vssitem = vssDatabase.get_VSSItem(CCMS_PRJ, false);
           

            var code = Request["txtCode"];
            code = code.Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace("新增", "").Replace("修改", "");

            string[] files = code.Split('\n');

            foreach (string file in files)
            {
                var vfile = file.Replace("\r","").Replace(" ","").Trim();
                var localfile = vfile.Replace("/", "\\");
                string vssfile = CCMS_PRJ + vfile;
               string localFile = CCMS_Local + localfile;

               VSSItem vssFolder = vssDatabase.get_VSSItem(vssfile, false);

               //更新之前先删除文件
               if (System.IO.File.Exists(localFile))
               {
                   System.IO.FileInfo f = new System.IO.FileInfo(localFile);
                   f.Attributes = FileAttributes.Normal; 
                   f.Delete();
               }
               vssFolder.Get(localFile, 0);//获取到本地文件夹
            }

            //iss.Dispose();
            //VSSItem vssitemVersion = vssitem.get_Version(1);
            //string s = "c:\\JobMate.sln";
            //vssitemVersion.Get(ref s, 0); 
            ViewBag.Tip = "获取代码成功";
            return Redirect("?act=success");
        }
        #endregion

        #region 登录/退出
        public ActionResult Login() 
        {
            return View();
        }
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginModel model) 
        {
            if (VerifyLogin())
            {
                 if(ViewBag.Role == "开发人员" || ViewBag.Role == "开发组长")
                {
                    return RedirectToAction("projectlist", "project");
                }
                 else if (ViewBag.Role == "版本发布人员" || ViewBag.Role == "管理员")
                 {
                     return RedirectToAction("projectlist", "project");
                 }
                 else 
                 {
                     return Content("当前用户没有角色");
                 }
            }
            else 
            {
                return Redirect(Url.Action("index", "home"));
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        public ActionResult Logout()
        {
            _userService.RemoveUserCookie();
              return Redirect(Url.Action("index","home"));
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        public bool VerifyLogin()
        {
            CIUser user = null;
            string userName = PressRequest.GetFormString("username");
            string password = EncryptHelper.MD5(PressRequest.GetFormString("password"));
            int expires = PressRequest.GetFormString("rememberme") == "forever" ? 43200 : 0;

            user = _userService.GetUser(userName, password);

            if (user != null)
            {
                _userService.WriteUserCookie( user.UserName, user.PassWord, expires);
                ViewBag.Role = user.Role;
                ViewBag.OrgCode = user.OrgCode;
                return true;
            }
            else
            {
                ModelState.AddModelError("", "用户名或密码错误!");
            }
            return false;
        }
        #endregion

        #region 注册
        public ActionResult CreateUser() 
        {
            var model = new CIUser();
            model.UserName = PressRequest.GetFormString("username");
            model.PassWord =  EncryptHelper.MD5(PressRequest.GetFormString("password"));
            model.FullName = PressRequest.GetFormString("fullname");
            model.RegTime = DateTime.Now;
            model.Role = PressRequest.GetFormString("role");

            _userService.InsertUser(model);

            return Redirect(Url.Action("login", "home"));
        }
        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckUser() 
        {
            var username = PressRequest.GetFormString("username");
            var msg = _userService.ExistsUserName(username)?"1":"0";
            return Content(msg);

        }
        #endregion
    }

    public class SourceSafeDatabase
    {
        private readonly string dbPath;
        private readonly string password;
        private readonly string rootProject;
        private readonly string username;
        private readonly VSSDatabaseClass vssDatabase;

        public SourceSafeDatabase(string dbPath, string username, string password, string rootProject)
        {
            this.dbPath = dbPath;
            this.username = username;
            this.password = password;
            this.rootProject = rootProject;

            vssDatabase = new VSSDatabaseClass();
        }

        public List<string> GetAllLabels()
        {
            List<string> allLabels = new List<string>();

            VSSItem item = vssDatabase.get_VSSItem(rootProject, false);
            IVSSVersions versions = item.get_Versions(0);

            foreach (IVSSVersion version in versions)
            {
                if (version.Label.Length > 0)
                {
                    allLabels.Add(version.Label);
                }
            }

            return allLabels;
        }

        public void GetLabelledVersion(string label, string project, string directory)
        {
            string outDir = directory;
            vssDatabase.get_VSSItem(rootProject, false).get_Version(label).Get(ref outDir, (int)VSSFlags.VSSFLAG_RECURSYES + (int)VSSFlags.VSSFLAG_USERRONO);
        }

        public void Open()
        {
            vssDatabase.Open(dbPath, username, password);
        }

        public void Close()
        {
            vssDatabase.Close();
        }

    }


}
