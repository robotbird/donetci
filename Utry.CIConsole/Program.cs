using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Threading;
using Microsoft.Win32;
//using System.Drawing;

using SharpSvn;
using Utry.Framework.Configuration;
using Utry.Framework.Log;
using Utry.Framework.Utils;
using Utry.Core;
using Utry.Core.Domain;
using Utry.Core.Services;

namespace Utry.CIConsole
{
    class Program
    {
        //TODO:增加ftp方式根据svn获取记录增量部署功能T
        //TODO:增加基于svn或者git的自动部署功能
        //TODO:生成包移动到源码包外面单独建文件夹
        //TODO:增加自定义路径和生成压缩包功能，改造由bat文件编译打包为.net console程序编译打包
        #region 私有变量
        /// <summary>
        /// 分支名称
        /// </summary>
        private static string BranchName = "";

        /// <summary>
        /// 项目编号
        /// </summary>
        private static string ProjCode = "";

        /// <summary>
        /// 本地目录
        /// </summary>
        private static string WorkPath = "";
        /// <summary>
        /// 解决方案名称
        /// </summary>
        private static string SlnName = "";
        /// <summary>
        /// 当前解决方案下需要打包的文件夹
        /// </summary>
        private static string PackagePath = "";
        #endregion
        
        static void Main(string[] args)
        {
            Console.Title = "CCMS持续集成客户端";
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(timer1_Tick);
           // Icon icon1 = new Icon("c:\temp.ico");
           // Bitmap bmp = icon1.ToBitmap();

            // Draw the bitmap.

           // e.Graphics.DrawImage(bmp, new Point(30, 30));

            int min = 1000 * 60;//一分钟
            aTimer.Interval = 0.1 * min;//轮询时间间隔1秒 30秒
            //设置是执行一次（false）还是一直执行(true)；
            aTimer.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；
            aTimer.Enabled = true;
            Console.WriteLine("CI已启动：" + DateTime.Now);
            Console.ReadLine();
        }
        /// <summary>
        /// 定时事件
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="e">ElapsedEventArgs事件对象</param>
        protected static void timer1_Tick(object source, ElapsedEventArgs e)
        {

            int intHour = e.SignalTime.Hour;
            int intMinute = e.SignalTime.Minute;
            int intSecond = e.SignalTime.Second;
            bool isTime = (1 <= intHour && intHour <= 24);//时间间隔 暂时24小时同步 后期写到系统配置全局表里
            if (isTime) ///定时设置,判断分时秒 if 
            {
                System.Timers.Timer tt = (System.Timers.Timer)source;
                tt.Enabled = false;
                try
                {
                    Integration();
                    tt.Enabled = true;

                    //if (DateTime.Now.Hour >= 23)
                    //{
                    //    System.Environment.Exit(-1);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.WriteLog(LogType.ERROR, ex);
                    tt.Enabled = true;
                }
            }

        }

        /// <summary>
        /// 持续集成入口
        /// </summary>
        static void Integration() 
        {

            WorkPath = ConfigHelper.GetValue("WorkPath");
            
            var releaseService = new ReleaseService();
            var projService = new ProjectService();

            var list = releaseService.GetReleaseListByStatus("发布中");//获取需要发布版本的请求

            //list.Clear();
            //list.Add(new CIRelease
            //{
            //    ID = "9a44c1d9-6c8a-4be9-b964-7e5978cad1b7",
            //    ProjectID = "bf5d0925-76d4-4936-927f-d22da9da30df",
            //    Type = "测试版本"
            //});//测试用

            foreach(var item in list)
            {
                var project = new CIProject();
                project = projService.GetById(item.ProjectID);
                var projectSvnUrl = string.Empty;
                if (item.Type == "测试版本") 
                {
                    projectSvnUrl = project.ProjectSvnURL;
                }
                if (item.Type == "正式版本") 
                {
                    projectSvnUrl = project.ProjectSvnURLRelease;
                }
                SlnName = project.SlnName;//从数据库读取解决方案名称
                PackagePath = project.PackagePath;//从数据库中读取打包路径名称

                //var path = project.ProjectSvnURL.Split('/');
                var path = projectSvnUrl.Split('/');
                BranchName = path[path.Length - 1];
                ProjCode = path[path.Length - 2];

                item.BeginTime = DateTime.Now;

                Console.WriteLine(project.ProjectName+"代码获取中...");
                string svnlog = "";
                long reversion = -1;
                SVNUpdate(projectSvnUrl,out svnlog,out reversion);//svn代码获取
                item.Remark = svnlog;//svn 日志
                item.Reversion = reversion;//版本号
                Console.WriteLine(item.Remark);
                Console.WriteLine("代码获取成功");

                Console.WriteLine(project.ProjectName + "代码编译打包中...");
                MSBuildAndCompress();//编译打包

                #region 发布记录更新
                var localpath = WorkPath + @"\" + ProjCode + @"\" + BranchName;//当前发布记录地址

                item.EndTime = DateTime.Now;

                //获取发布日志
                var logpath = localpath + @"\log.log";
                string s = "";

               // FileStream fs = new FileStream("f:\\aaa.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
               // StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);

                using (StreamReader sr = new StreamReader(logpath, System.Text.Encoding.Default))
                {
                    item.LogContent = sr.ReadToEnd();//发布日志
                } 

                //判断发布状态逻辑
                if (item.LogContent.IndexOf("成功生成") > 0 || item.LogContent.IndexOf("Build succeeded") > 0)
                {
                    item.Status = "发布成功";

                    var filename = "";
                    DirectoryInfo dirInfo = new DirectoryInfo(WorkPath + @"\" + ProjCode + @"\" + BranchName);
                    FileSystemInfo[] files = dirInfo.GetFileSystemInfos();
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo file = files[i] as FileInfo;
                        if (file != null&& file.Name.ToLower().LastIndexOf(".rar")>-1) //是文件
                        {
                            filename = file.Name;
                        }
                    }

                    //获取下载地址逻辑
                    item.VersionURL = "/" + ProjCode + "/" + BranchName + "/" + filename;//下载地址url
                }
                else 
                {
                    item.Status = "发布失败";                    
                }
                releaseService.UpdatePro(item);
                #endregion
            }

        }

        /// <summary>
        /// svn代码获取
        /// </summary>
        /// <param name="svnuri">svn地址</param>
        static void SVNUpdate(string svnuri,out string logmessage,out long reversion)
        {
            logmessage = "";
            reversion = 0;
            long clientVersion = -1;
            using (SvnClient client = new SvnClient())
            {
                try
                {
                    var localpath = WorkPath + @"\" + ProjCode + @"\" + BranchName;
                    var username = ConfigHelper.GetValue("SVNUser");
                    var pwd = ConfigHelper.GetValue("SVNpwd");
                    client.Authentication.DefaultCredentials = new System.Net.NetworkCredential(username, pwd);
                    //notiny = "正在检查本地版本...";
                    //ShowInfo();
                    //SvnInfoEventArgs clientInfo;
                    //bool okc = SC.GetInfo(local, out clientInfo);
                    //if (oks && okc) //如果客户端服务端都会成功， 则对比服务器版本， 否则返回true 执行更新命令
                    //{
                    //    result = (serverInfo.Revision > clientInfo.Revision);
                    //}

                    //如果项目不存在则checkout，否则进行update
                    if (!Directory.Exists(localpath))
                    {
                        client.CheckOut(new Uri(svnuri), localpath);
                        Console.WriteLine("svn checkout success");
                    }
                    else
                    {
                        SvnInfoEventArgs serverInfo;
                        SvnInfoEventArgs clientInfo;
                        SvnUriTarget repos = new SvnUriTarget(svnuri);
                        SvnPathTarget local = new SvnPathTarget(localpath);
                        SvnUpdateArgs args = new SvnUpdateArgs();
                        args.Depth = SvnDepth.Infinity;
                        var svnService = new SvnService();
                        long version = 0;

                        var msg = "";
                        args.Notify += delegate(object sender, SvnNotifyEventArgs e)
                        {
                            //if (svnService.GetNotifyAction(e.Action) == "添加")
                            //{
                            //    msg += "\r\n添加" + e.FullPath;
                            //}
                            //if (svnService.GetNotifyAction(e.Action) == "更新删除")
                            //{
                            //    msg += "\r\n删除：" + e.FullPath;
                            //}
                            //if (svnService.GetNotifyAction(e.Action) == "更新修改")
                            //{
                            //    msg += "\r\n修改" + e.FullPath;
                            //}
                            msg += "\r\n" + (svnService.GetNotifyAction(e.Action)) + ":" + e.FullPath;
                            version = e.Revision;
                            Console.WriteLine(msg);
                        };


                        client.GetInfo(repos, out serverInfo);
                        client.GetInfo(local, out clientInfo);
                        clientVersion = clientInfo.Revision;



                        //if (clientVersion < version)//客户端version必须小于服务端才更新
                        //{


                        //client.Resolve(localpath, SvnAccept.Base);//解决冲突
                        ////解决冲突
                        //Collection<SvnStatusEventArgs> statuses;
                        //client.GetStatus(localpath, out statuses);

                        //foreach (var item in statuses)
                        //{
                        //    if (item.Conflicted)
                        //    {
                        //        client.Resolve(item.FullPath, SvnAccept.Working);
                        //        Console.WriteLine("处理冲突文件:" + item.FullPath);
                        //        logmessage += "处理冲突文件:" + item.FullPath;
                        //    }
                        //}


                        client.CleanUp(localpath);

                        SvnRevertArgs revertArgs = new SvnRevertArgs() { Depth = SvnDepth.Infinity };//撤销本地所做的修改
                        client.Revert(localpath, revertArgs);
                        client.Update(localpath, args);

                        //获取消息
                        Collection<SvnLogEventArgs> logitems;
                        //if (version > 0 && clientVersion < version)
                        logmessage += "变更文件：" + msg;

                        if (version > 0 && clientVersion < version)
                        {
                            reversion = version;
                            client.GetLog(new Uri(svnuri), new SvnLogArgs(new SvnRevisionRange(clientVersion + 1, version)), out logitems);
                            //client.GetLog(new Uri(svnuri), new SvnLogArgs(new SvnRevisionRange(version, version)), out logitems);
                            foreach (var logentry in logitems)
                            {

                                string author = logentry.Author;
                                string message = logentry.LogMessage;
                                //DateTime checkindate = logentry.Time;
                                logmessage += string.Format("\r\n提交人：{0} ，日志： {1}", author, message);
                            }
                        }
                        //}

                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    logmessage = ex.ToString();
                }
                
            }
        }

        //测试通过.net调用Msbuild
        static void MSBuildAndCompress() 
        {
            var rootpath = WorkPath + @"\" + ProjCode + @"\" + BranchName;

            #region 生成build.xml
            var buildxml = rootpath + @"\" + "build.xml";
            if (!File.Exists(buildxml))
            {
                //读取xml模板
                var dir = System.IO.Directory.GetCurrentDirectory();
                var xmltmp = dir + @"\" + "build.xml";
                var tmpcontent = string.Empty;
                using (StreamReader sr = new StreamReader(xmltmp))
                {
                    while (sr.Peek() >= 0)
                    {
                        tmpcontent = sr.ReadToEnd();
                    }
                }
                if (!string.IsNullOrEmpty(tmpcontent))
                {
                    tmpcontent = tmpcontent.Replace("{BranchName}", BranchName);

                    if (!string.IsNullOrEmpty(SlnName))
                    {
                        tmpcontent = tmpcontent.Replace("{SlnName}", SlnName);
                    }
                    else
                    {
                        var files = Directory.GetFiles(WorkPath + "\\" + ProjCode + "\\" + BranchName, "*.sln");
                        if (files.Any())
                        {
                            tmpcontent = tmpcontent.Replace("{SlnName}", files[0]);
                        }

                        if (BranchName.ToLower().IndexOf("db_transfer") > -1)
                        {
                            tmpcontent = tmpcontent.Replace("{SlnName}", "db_transfer_new\\DB_Transfer.sln");
                        }
                    }
                   
                    using (StreamWriter writer = new StreamWriter(buildxml, false, new UTF8Encoding(true)))
                    {
                        try
                        {
                            writer.Write(tmpcontent);
                            writer.Flush();
                            writer.Close();
                        }
                        catch (Exception)
                        {
                            writer.Flush();
                            writer.Close();
                        }

                    }
                }
            }
            #endregion

            var xmlpath = WorkPath + "\\" + ProjCode + "\\" + BranchName + "\\build.xml";
            
            var buildshell = ConfigHelper.GetValue("buildshell").Replace("{logpath}",rootpath);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ConfigHelper.GetValue("MSBuild"),//执行的文件名  
                    Arguments = xmlpath + " " + buildshell,//需要执行的命令  
                    UseShellExecute = false,//使用Shell执行  
                    WindowStyle = ProcessWindowStyle.Hidden,//隐藏窗体  
                    CreateNoWindow = false,//不显示窗体  
                },
            };


            process.Start();//开始执行  
            process.WaitForExit();//等待完成并退出  

            var path = rootpath;

            if (string.IsNullOrEmpty(PackagePath) && BranchName.ToLower().IndexOf("db_transfer") > -1)
            {
                path += "\\" + ConfigHelper.GetValue("db_transfer_rar");
            }
            else if (string.IsNullOrEmpty(PackagePath) && BranchName.ToLower().IndexOf("jsdx_level_rating") > -1)
            {
                path += "\\" + ConfigHelper.GetValue("jsdx_level_rating");
            }
            else if (!string.IsNullOrEmpty(PackagePath))
            {
                path += "\\" + PackagePath;
            }
            else if (BranchName.ToLower().IndexOf("ccms_v") > -1)
            {
                path += "\\web";
            }
            #region UTMP专用
           
            // path += "\\" + ConfigHelper.GetValue("UTMP_rar");
            
            #endregion

            var rarPath = rootpath;
            var rarName = BranchName+"_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".rar";
            var ignore = ConfigHelper.GetValue("rar_ignore");

            //重命名*.config
            var configfiles = Directory.GetFiles(path, "*.config");
            var configfile = configfiles.Any()?configfiles[0]:"";

            if (configfile!="")
            {
                FileHelper.RenameFile(configfile, configfile + ".sample");                
            }
            
            Console.WriteLine("生成打包路径:"+path);
            CompressHelper.CompressRar(path, rarPath, rarName,ignore);
            //重命名*.config.sample
            if (configfile != "")
            {
                FileHelper.RenameFile(configfile + ".sample", configfile);
            }
         
            Console.WriteLine(BranchName + "发布成功:" + DateTime.Now);
            Console.WriteLine("任务等待中...");
        }
    }
}
