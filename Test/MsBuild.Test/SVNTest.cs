using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text;
using System.Diagnostics;
using System.IO;

using Utry.Core.Services;
using SharpSvn;
using Utry.Core;

namespace MsBuild.Test
{
    /// <summary>
    /// SVNTest 的摘要说明
    /// </summary>
    [TestClass]
    public class SVNTest
    {
        public SVNTest()
        {
            
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

       
        public void TestMethod1()
        {
            //http://docs.sharpsvn.net/current/

            using (SvnClient client = new SvnClient())
            {
                // Checkout the code to the specified directory
                client.Authentication.Clear(); // Clear a previous authentication
                client.Authentication.DefaultCredentials = new System.Net.NetworkCredential("", "");
               // client.CheckOut(new Uri("http://ip:9001/svn/CCMS/trunk/CCMS_V6/DOCUMENT/"),  "d:\\sharpsvn");


                Uri targetUri = new Uri("http://ip:9001/svn/CCMS/trunk/CCMS_V6/DOCUMENT");
                var target = SvnTarget.FromUri(targetUri);
                Collection<SvnInfoEventArgs> info;//System.Collections.ObjectModel
                bool result2 = client.GetInfo(target, new SvnInfoArgs { ThrowOnError = false }, out info);
                //Assert.AreEqual(result2, false);
               // Assert.Equals(info, "");


                SvnUpdateArgs ua = new SvnUpdateArgs();
                SvnLogArgs args2 = new SvnLogArgs(new SvnRevisionRange(500, new SvnRevision(SvnRevisionType.Head)));
                 Collection<SvnLogEventArgs> logitems;
                ua.Notify += delegate(object sender, SvnNotifyEventArgs e)
                {
                    Console.WriteLine(e.Action);
                    Console.WriteLine(e.FullPath);
                };
                // Update the specified working copy path to the head revision
                client.Update("d:\\sharpsvn",ua);
                SvnUpdateResult result;
               
                client.Update("d:\\sharpsvn", out result);
            
                client.GetLog(targetUri,out logitems);
                foreach (var logentry in logitems)
                {
                    string author = logentry.Author;
                    string message = logentry.LogMessage;
                    DateTime checkindate = logentry.Time;
              
                }


              //  client.Move("c:\\sharpsvn\\from.txt", "c:\\sharpsvn\\new.txt");

                // Commit the changes with the specified logmessage
                //SvnCommitArgs ca = new SvnCommitArgs();
               // ca.LogMessage = "Moved from.txt to new.txt";
               // client.Commit("c:\\sharpsvn", ca);
            }
        }

        
        public void TestChangeListAndLog()
        {
            var svnService = new Utry.Core.Services.SvnService();
            //http://docs.sharpsvn.net/current/

            using (SvnClient client = new SvnClient())
            {
                // Checkout the code to the specified directory
                client.Authentication.Clear(); // Clear a previous authentication
                client.Authentication.DefaultCredentials = new System.Net.NetworkCredential("", "");
                //client.CheckOut(new Uri("http://ip:9001/svn/CCMS/trunk/CCMS_V6/DOCUMENT/"), "d:\\sharpsvn");

                Uri targetUri = new Uri("http://ip:9001/svn/CCMS/trunk/CCMS_V6/DOCUMENT");
                var target = SvnTarget.FromUri(targetUri);
                Collection<SvnInfoEventArgs> info;//System.Collections.ObjectModel
                bool result2 = client.GetInfo(target, new SvnInfoArgs { ThrowOnError = false }, out info);
              

                SvnUpdateArgs ua = new SvnUpdateArgs();
                SvnLogArgs args2 = new SvnLogArgs(new SvnRevisionRange(500, new SvnRevision(SvnRevisionType.Head)));
                var sua = new SvnUpdateArgs { Revision = new SvnRevision(0) };

                 Collection<SvnLogEventArgs> logitems;

                 long version=0;
                ua.Notify += delegate(object sender, SvnNotifyEventArgs e)
                {
                    Console.WriteLine(string.Format("{0}:{1}.", svnService.GetNotifyAction(e.Action), e.FullPath));
                    Console.WriteLine(e.Revision);
                    version = e.Revision;
                };

                client.Update("d:\\sharpsvn", ua);

                if (version > 0)
                {
                    client.GetLog(targetUri, new SvnLogArgs(new SvnRevisionRange(version, version)), out logitems);
                    foreach (var logentry in logitems)
                    {
                        string author = logentry.Author;
                        string message = logentry.LogMessage;
                        DateTime checkindate = logentry.Time;
                        Console.WriteLine(string.Format("{0}  :  {1  }:  {2}", author, message, checkindate));
                    }
                }
               
              

            }
        }

        [TestMethod]
        public void TestCommit()
        {
            using (SvnClient client = new SvnClient())
            {
                var localpath = "d:\\sharpsvn\\需求文档\\1.txt";
                localpath = "d:\\sharpsvn\\1.txt";
                //localpath = @"E:\project\ccms\CCMS_V7_HBYD_R";
                var username = "yepeng";
                var pwd = "123456";
                client.Authentication.DefaultCredentials = new System.Net.NetworkCredential(username, pwd);
                //如果项目不存在则checkout，否则进行update
                Console.WriteLine("开始检查是否需要提交新参数表...");
                SvnCommitArgs ca = new SvnCommitArgs();
                SvnStatusArgs sa = new SvnStatusArgs();
                Collection<SvnStatusEventArgs> statuses;
                client.GetStatus(localpath, sa, out statuses);
                int i = 0;
                foreach (var item in statuses)
                {
                    if (item.LocalContentStatus != item.RemoteContentStatus)
                    {
                        i++;
                    }
                    if (!item.Versioned)
                    {
                        client.Add(item.FullPath);
                        Console.WriteLine("新增加文件" + item.FullPath);
                        i++;
                    }
                }
                if (i > 0)
                {
                    ca.LogMessage = "测试提交文档";
                    SvnCommitResult clientr;
                    if (client.Commit(localpath, ca, out clientr))
                    {
                        Console.WriteLine("提交完成");
                    }
                    else
                    {
                        Console.WriteLine("提交失败");
                    }
                }
                else
                {
                    Console.WriteLine("无变化，无需检查");
                }
            }
        }
        
        public void TestSVNUpdate() 
        {
            var releaseService = new ReleaseService();
            var svnurl = "http://ip:9001/svn/CCMS/trunk/CCMS_V6/DOCUMENT/";
          // var svnurl = "http://ip:9001/svn/CCMS/branches/CCMS_ZGRB/CCMS_V6_ZGRB_R";
            svnurl = "http://ip:9001/svn/ccms/branches/CCMS_HBYD/CCMS_V7_HBYD_R";
            var item = new Utry.Core.Domain.CIRelease();
            var remark = "";
            long reversion = -1;
            SVNUpdate(svnurl, out remark, out reversion);
            item.Reversion = reversion;
            item.Remark = remark;

            Console.WriteLine(reversion);
            Console.WriteLine(remark);
           // releaseService.UpdatePro(item);
            
        }

        /// <summary>
        /// svn代码获取
        /// </summary>
        /// <param name="svnuri">svn地址</param>
        static void SVNUpdate(string svnuri, out string logmessage, out long reversion)
        {
            logmessage = "";
            reversion = 0;
            using (SvnClient client = new SvnClient())
            {
                var localpath = "d:\\sharpsvn";
                localpath = @"E:\project\ccms\CCMS_V7_HBYD_R";
                var username = "";
                var pwd = "";
                client.Authentication.DefaultCredentials = new System.Net.NetworkCredential(username, pwd);
                //如果项目不存在则checkout，否则进行update
                if (!Directory.Exists(localpath))
                {
                    client.CheckOut(new Uri(svnuri), localpath);
                    // Console.WriteLine("svn checkout success");
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
                    long clientVersion = -1;
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
                    };


                    client.GetInfo(repos, out serverInfo);
                    client.GetInfo(local, out clientInfo);
                    clientVersion = clientInfo.Revision;

                    //if (clientVersion < version)//客户端version必须小于服务端才更新
                    //{
                    client.CleanUp(localpath);

                    SvnRevertArgs args2 = new SvnRevertArgs() { Depth = SvnDepth.Infinity };

                  
                    client.Revert(localpath,args2);
                    client.Update(localpath, args);

                    //获取消息
                    Collection<SvnLogEventArgs> logitems;
                    //if (version > 0 && clientVersion < version)
                    if (msg.Length > 5)
                    {
                        reversion = version;
                        logmessage += "\r\n变更文件：" + msg;
                        client.GetLog(new Uri(svnuri), new SvnLogArgs(new SvnRevisionRange(clientVersion + 1, version)), out logitems);
                       // client.GetLog(new Uri(svnuri), new SvnLogArgs(new SvnRevisionRange(version, version)), out logitems);
                        foreach (var logentry in logitems)
                        {
                            string author = logentry.Author;
                            string message = logentry.LogMessage;
                            //DateTime checkindate = logentry.Time;
                            logmessage += "\r\n";
                            logmessage += string.Format("提交人：{0} ，日志： {1}", author, message) + "\r\n";
                        }
                    }
                    //}


                    // Console.WriteLine(string.Format("serverInfo revision of {0} is {1}", repos, serverInfo.Revision));
                    //Console.WriteLine(string.Format("clientInfo revision of {0} is {1}", local, clientInfo.Revision));
                    // Console.WriteLine("代码获取成功");
                }
            }//

                
            }
        }
    }


