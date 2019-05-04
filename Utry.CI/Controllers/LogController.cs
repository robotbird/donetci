using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Utry.Core.Domain;
using Utry.Core.Services;
using Utry.CI.Models;
using Utry.Framework.Web;
using Utry.Framework.Mvc;

namespace Utry.CI.Controllers
{
    public class LogController : BaseController
    {
        //
        // 日志列表

        private LogService _logservice = new LogService();

        /// <summary>
        /// 显示日志列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LogList()
        {
            LogListModel model = new LogListModel();
            //判断Log文件夹是否存在，存在则执行下面的代码
            try
            {
                if (Directory.Exists(Server.MapPath(@"~\Log\")))
                {
                    //定义string类型数据来接收Log文件夹下的所有文件
                    string[] files = Directory.GetFiles(Server.MapPath(@"~\Log\"));
                    List<FileInfo> fi = new List<FileInfo>();
                    foreach (var file in files)
                    {
                        fi.Add(new FileInfo(file));
                    }
                    List<FileInfo> fio = fi.OrderByDescending(p => p.CreationTime).ToList();
                    foreach (var item in fio)
                    {
                        var log = new CISysLog { Logname = item.Name, Logsize = System.Math.Round(Convert.ToDouble(item.Length) / 1024, 2).ToString() + "KB", Createtime = item.CreationTime.ToString() };
                        model.Loglist.Add(log);
                    }
                }
                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
            
        }


        /// <summary>
        /// 根据文件名来删除制定Log日志文件
        /// </summary>
        /// <param name="name">指定的Log文件名</param>
        /// <returns></returns>
        public ActionResult DeleteLog(string name)
        {
            try
            {
                if (!string.IsNullOrEmpty(name))
                {
                    var result = _logservice.DeleteLog(Server.MapPath(@"~\Log\" + name));

                    if (result > 0)
                    {
                        SuccessNotification("删除成功");
                    }
                }
                return Redirect(Url.Action("Loglist", "Log"));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
