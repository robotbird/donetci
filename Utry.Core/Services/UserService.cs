using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Utry.Core.Domain;
using Utry.Framework.Utils;
using Utry.Framework.Web;
using Utry.Framework.Configuration;
using Utry.Core.Repositories.Repository;
using Utry.Core.Repositories.IRepository;

namespace Utry.Core.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;

        #region 构造函数
        /// <summary>
        /// 构造器方法
        /// </summary>
        public UserService()
            : this(new UserRepository())
        {
        }
        /// <summary>
        /// 构造器方法
        /// </summary>
        /// <param name="userRepository"></param>
        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        #endregion


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public int InsertUser(CIUser _userinfo)
        {
            return _userRepository.Insert(_userinfo);
           
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public int UpdateUser(CIUser _userinfo)
        {
            return _userRepository.Update(_userinfo);
        }

        /// <summary>
        /// 更新开发人员所属小组
        /// </summary>
        /// <param name="CIUser"></param>
        /// <returns></returns>
        public int UpdateUserOrg(CIUser _userinfo)
        {
            return _userRepository.UpdateUserOrg(_userinfo);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int DeleteUser(string userName)
        {
            return _userRepository.Delete(new CIUser { UserName = userName });
        }


        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        public List<CIUser> GetUserList()
        {
            return _userRepository.Table.ToList();
        }

        /// <summary>
        /// 获取包括小组信息的全部人员信息
        /// </summary>
        /// <returns></returns>
        public List<CIUser> GetAllUserInfo()
        {
            return _userRepository.GetAllUserInfo();
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            return _userRepository.ExistsUserName(userName);
        }

        /// <summary>
        /// 根据用户名获取用户 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public CIUser GetUser(string userName)
        {

            return _userRepository.GetById(userName);
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CIUser GetUser(string userName, string password)
        {
            return _userRepository.GetUser(userName, password);
        }

        /// <summary>
        /// 根据全名获取用户 
        /// </summary>
        /// <returns></returns>
        public List<CIUser> GetUserByFullName(string fullname)
        {
            return _userRepository.GetUserByFullName(fullname);
        }

        #region 用户COOKIE操作
        /// <summary>
        /// 用户COOKIE名
        /// </summary>
        private readonly string CookieUser = ConfigHelper.SitePrefix + "utry";
        /// <summary>
        /// 读当前用户COOKIE
        /// </summary>
        /// <returns></returns>
        public HttpCookie ReadUserCookie()
        {
            return HttpContext.Current.Request.Cookies[CookieUser];
        }
        /// <summary>
        /// 根据cookie值获取当前用户
        /// </summary>
        /// <returns></returns>
        public CIUser GetUserFromCookie()
        {
            var cookie = ReadUserCookie();
            var CurrentUserId = TypeConverter.ObjectToString(cookie["username"]);
            return GetUser(CurrentUserId);
        }

        /// <summary>
        /// 移除当前用户COOKIE
        /// </summary>
        /// <returns></returns>
        public bool RemoveUserCookie()
        {
            HttpCookie cookie = new HttpCookie(CookieUser);
            cookie.Values.Clear();
            cookie.Expires = DateTime.Now.AddYears(-1);

            HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }

        /// <summary>
        /// 写/改当前用户COOKIE
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public bool WriteUserCookie(string userName, string password, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieUser];
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieUser);
            }
            if (expires > 0)
            {
                cookie.Values["expires"] = expires.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            else
            {
                int temp_expires = Convert.ToInt32(cookie.Values["expires"]);
                if (temp_expires > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(temp_expires);
                }
            }
            cookie.Values["username"] = HttpContext.Current.Server.UrlEncode(userName);
            cookie.Values["key"] = Utry.Framework.Utils.EncryptHelper.MD5(HttpContext.Current.Server.UrlEncode(userName) + password);

            HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }
        #endregion
    }
}
