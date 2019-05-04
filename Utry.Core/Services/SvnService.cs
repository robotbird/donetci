using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;
using Utry.Framework.Configuration;
using SharpSvn;

namespace Utry.Core.Services
{

    public class SvnService 
    {
        /// <summary>
        /// 是否合法svn地址
        /// </summary>
        /// <param name="svnuri">svn路径</param>
        /// <returns></returns>
        public bool IsExistsUri(string svnuri)
        {
            if (string.IsNullOrEmpty(svnuri)) 
            {
                return false;
            }
            using (SvnClient client = new SvnClient())
            {
                var username = ConfigHelper.GetValue("SVNUser");
                var pwd = ConfigHelper.GetValue("SVNpwd");
                client.Authentication.DefaultCredentials = new System.Net.NetworkCredential(username, pwd);
                client.LoadConfiguration(Path.Combine(Path.GetTempPath(), "Svn"), true);
                Uri targetUri = new Uri(svnuri);
                var target = SvnTarget.FromUri(targetUri);
                Collection<SvnInfoEventArgs> info;
                bool result = client.GetInfo(target, new SvnInfoArgs { ThrowOnError = false }, out info);
                return result;
            }
        }
        /// <summary>
        /// 获取svn的中文通知方式
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public string GetNotifyAction(SvnNotifyAction act)
        {
            string result = null;
            switch (act)
            {
                case SvnNotifyAction.Add:
                    result = "添加";
                    break;
                case SvnNotifyAction.BlameRevision:
                    break;
                case SvnNotifyAction.ChangeListClear:
                    break;
                case SvnNotifyAction.ChangeListMoved:
                    break;
                case SvnNotifyAction.ChangeListSet:
                    break;
                case SvnNotifyAction.CommitAddCopy:
                    result = "提交添加副本";
                    break;
                case SvnNotifyAction.CommitAdded:
                    break;
                case SvnNotifyAction.CommitDeleted:
                    break;
                case SvnNotifyAction.CommitModified:
                    break;
                case SvnNotifyAction.CommitReplaced:
                    break;
                case SvnNotifyAction.CommitReplacedWithCopy:
                    break;
                case SvnNotifyAction.CommitSendData:
                    break;
                case SvnNotifyAction.Copy:
                    break;
                case SvnNotifyAction.Delete:
                    break;
                case SvnNotifyAction.Excluded:
                    result = "要排除";
                    break;
                case SvnNotifyAction.Exists:
                    result = "已存在";
                    break;
                case SvnNotifyAction.ExternalFailed:
                    result = "外部失败";
                    break;
                case SvnNotifyAction.FailedConflict:
                    result = "失败冲突";
                    break;
                case SvnNotifyAction.FailedForbiddenByServer:
                    break;
                case SvnNotifyAction.FailedLocked:
                    break;
                case SvnNotifyAction.FailedMissing:
                    break;
                case SvnNotifyAction.FailedNoParent:
                    break;
                case SvnNotifyAction.FailedOutOfDate:
                    result = "已过期且失败";
                    break;
                case SvnNotifyAction.FollowUrlRedirect:
                    break;
                case SvnNotifyAction.LockFailedLock:
                    break;
                case SvnNotifyAction.LockFailedUnlock:
                    break;
                case SvnNotifyAction.LockLocked:
                    break;
                case SvnNotifyAction.LockUnlocked:
                    break;
                case SvnNotifyAction.MergeBegin:
                    break;
                case SvnNotifyAction.MergeBeginForeign:
                    break;
                case SvnNotifyAction.MergeCompleted:
                    break;
                case SvnNotifyAction.NonExistentPath:
                    break;
                case SvnNotifyAction.PatchApplied:
                    break;
                case SvnNotifyAction.PatchAppliedHunk:
                    break;
                case SvnNotifyAction.PatchFoundAlreadyApplied:
                    break;
                case SvnNotifyAction.PatchRejectedHunk:
                    break;
                case SvnNotifyAction.PropertyAdded:
                    break;
                case SvnNotifyAction.PropertyDeleted:
                    break;
                case SvnNotifyAction.PropertyDeletedNonExistent:
                    break;
                case SvnNotifyAction.PropertyModified:
                    break;
                case SvnNotifyAction.RecordMergeInfo:
                    break;
                case SvnNotifyAction.RecordMergeInfoElided:
                    break;
                case SvnNotifyAction.RecordMergeInfoStarted:
                    break;
                case SvnNotifyAction.Resolved:
                    break;
                case SvnNotifyAction.Restore:
                    result = "恢复";
                    break;
                case SvnNotifyAction.Revert:
                    result = "还原";
                    break;
                case SvnNotifyAction.RevertFailed:
                    result = "还原失败";
                    break;
                case SvnNotifyAction.RevisionPropertyDeleted:
                    break;
                case SvnNotifyAction.RevisionPropertySet:
                    break;
                case SvnNotifyAction.Skip:
                    result = "跳过";
                    break;
                case SvnNotifyAction.SkipConflicted:
                    result = "跳过冲突";
                    break;
                case SvnNotifyAction.StatusCompleted:
                    result = "状态已完成";
                    break;
                case SvnNotifyAction.StatusExternal:
                    break;
                case SvnNotifyAction.TreeConflict:
                    result = "数冲突";
                    break;
                case SvnNotifyAction.UpdateAdd:
                    result = "新增";
                    break;
                case SvnNotifyAction.UpdateCompleted:
                    result = "更新已完成";
                    break;
                case SvnNotifyAction.UpdateDelete:
                    result = "删除";
                    break;
                case SvnNotifyAction.UpdateExternal:
                    result = "更新外部";
                    break;
                case SvnNotifyAction.UpdateExternalRemoved:
                    result = "更新外部已删除";
                    break;
                case SvnNotifyAction.UpdateReplace:
                    result = "替换";
                    break;
                case SvnNotifyAction.UpdateShadowedAdd:
                    break;
                case SvnNotifyAction.UpdateShadowedDelete:
                    break;
                case SvnNotifyAction.UpdateShadowedUpdate:
                    break;
                case SvnNotifyAction.UpdateSkipAccessDenied:
                    break;
                case SvnNotifyAction.UpdateSkipObstruction:
                    break;
                case SvnNotifyAction.UpdateSkipWorkingOnly:
                    break;
                case SvnNotifyAction.UpdateStarted:
                    result = "更新已开始";
                    break;
                case SvnNotifyAction.UpdateUpdate:
                    result = "修改";
                    break;
                case SvnNotifyAction.UpgradedDirectory:
                    result = "更新目录";
                    break;
            }
            return result ?? (act.ToString());
        }

    }


    /*
    public class SVNUtils
    {
        public static bool Cancel;
        private readonly SvnClient SC;
        private string path;
        private string prog;
        private string svnurl;
        public SVNUtils()
        {
            svnurl = "svn://127.0.0.1:" + TCS.Config.AppConfig.RunTime.SVNPort.ToString();
            SC = new SvnClient();
            SvnUriTarget rem = new SvnUriTarget(svnurl);
            SC.Authentication.ClearAuthenticationCache();
            SC.Authentication.Clear();
            SC.Authentication.DefaultCredentials = new SvnCredentialProvider(
                  TCS.Config.AppConfig.RunTime.SVNUsername,
                   TCS.Config.AppConfig.RunTime.SVNPassword,
                    svnurl);//默认用户名密码
        }
        public bool Commit()
        {
            Console.WriteLine("开始检查是否需要提交新参数表...");
            SvnCommitArgs ca = new SvnCommitArgs();
            SvnStatusArgs sa = new SvnStatusArgs();
            Collection<SvnStatusEventArgs> statuses;
            SC.GetStatus(GetAppLoc(), sa, out statuses);
            int i = 0;
            foreach (var item in statuses)
            {
                if (item.LocalContentStatus != item.RemoteContentStatus)
                {
                    i++;
                }
                if (!item.Versioned)
                {
                    SC.Add(item.FullPath);
                    Console.WriteLine("新增加文件" + item.FullPath);
                    i++;
                }
                else if (item.Conflicted)
                {
                    SC.Resolve(item.FullPath, SvnAccept.Working);
                    Console.WriteLine("处理冲突文件" + item.FullPath);
                }
                else if (item.IsRemoteUpdated)
                {
                    SC.Update(item.FullPath);
                    Console.WriteLine("处理冲突文件" + item.FullPath);
                }
                else if (item.LocalContentStatus == SvnStatus.Missing)
                {
                    SC.Delete(item.FullPath);
                    Console.WriteLine("处理丢失文件" + item.FullPath);
                    i++;
                }
            }
            if (i > 0)
            {
                ca.LogMessage = "";
                SvnCommitResult scr;
                if (SC.Commit(GetAppLoc(), ca, out scr))
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


            return true;
        }
        /// <summary>
        /// 获取本地工作副本svn路径
        /// </summary>
        /// <returns></returns>
        public string GetAppLoc()
        {
            return Comm.parentPath + Comm.issuedPath;
        }
        /// <summary>
        /// 检查版本号，如果版本号不符， 则更新
        /// </summary>
        /// <returns></returns>
        public bool CheckVer()
        {
            bool result = true;
            var repos = new SvnUriTarget(svnurl);
            var local = new SvnPathTarget(GetAppLoc());
            try
            {
                notiny = "正在检查服务器版本...";
                ShowInfo();
                SvnInfoEventArgs serverInfo;
                bool oks = SC.GetInfo(repos, out serverInfo);
                notiny = "正在检查本地版本...";
                ShowInfo();
                SvnInfoEventArgs clientInfo;
                bool okc = SC.GetInfo(local, out clientInfo);
                if (oks && okc) //如果客户端服务端都会成功， 则对比服务器版本， 否则返回true 执行更新命令
                {
                    result = (serverInfo.Revision > clientInfo.Revision);
                }
                ShowInfo(string.Format("检查完毕，服务器版本{0}客户端版本{1}",
                                       (serverInfo != null ? serverInfo.Revision.ToString() : "(未知)"),
                                       (clientInfo != null ? clientInfo.Revision.ToString() : "(未知)")
                             ));
            }
            catch (Exception)
            {
                ShowInfo("检查文件是出现错误...");
            }
            return result;
        }
        /// <summary>
        /// 初始化 如果没有被管理，则签出
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            ShowInfo("正在初始化....");
            bool result = true;
            HandEvents();
            if (!SvnTools.IsManagedPath(GetAppLoc()))//查看某目录是否是受svn管理的状态， 即是否为工作副本
            {
                notiny = "正在检出文件.";
                ShowInfo();
                result = SC.CheckOut(new SvnUriTarget(svnurl), GetAppLoc());
                ShowInfo("文件检出完成.");
            }
            ShowInfo("初始化完毕.");
            return result;
        }
        /// <summary>
        /// 委托各种事件。
        /// </summary>
        private void HandEvents()
        {
            SC.Authentication.UserNamePasswordHandlers += Authentication_UserNamePasswordHandlers;
            SC.Authentication.UserNameHandlers += Authentication_UserNameHandlers;
            SC.Progress += SC_Progress;
            SC.Processing += SC_Processing;
            SC.Notify += SC_Notify;
            SC.SvnError += SC_SvnError;
            SC.Conflict += SC_Conflict;
            SC.Cancel += SC_Cancel;
            SC.Committed += SC_Committed;
            SC.Committing += SC_Committing;
        }
        /// <summary>
        /// 提交过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SC_Committing(object sender, SvnCommittingEventArgs e)
        {
            ShowInfo("正在提交");
        }
        /// <summary>
        /// 提交完成后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SC_Committed(object sender, SvnCommittedEventArgs e)
        {
            ShowInfo("提交完成" + e.Revision);
        }
        /// <summary>
        /// 被取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_Cancel(object sender, SvnCancelEventArgs e)
        {
            e.Cancel = Cancel;
        }
        /// <summary>
        /// 遇到文件冲突
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_Conflict(object sender, SvnConflictEventArgs e)
        {
            ShowInfo("文件冲突" + e.ConflictReason.ToString());
        }
        /// <summary>
        /// 任何svn错误可以在这处理,也可以在这里处理是否取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_SvnError(object sender, SvnErrorEventArgs e)
        {
            e.Cancel = true;
            WriteError(e.Exception);
        }
        /// <summary>
        /// 输出错误
        /// </summary>
        /// <param name="e_Exception"></param>
        public void WriteError(Exception e_Exception)
        {
            ShowInfo(e_Exception.Message + e_Exception.Source + "\r\n"
                     + e_Exception.StackTrace + "\r\n"
                     );
        }
        /// <summary>
        /// 处理某项操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_Processing(object sender, SvnProcessingEventArgs e)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("正在处理操作" + e.CommandType.ToString() + ".......");
        }
        /// <summary>
        /// 处理过程，进度，，比如传送的字节数等显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SC_Progress(object sender, SvnProgressEventArgs e)
        {
            if (e.Progress == 0)
            {
                prog = "";
            }
            else
            {
                if (e.TotalProgress ==
                    -1)
                {
                    if (e.Progress > 1024)
                    {
                        prog = ((float)(e.Progress) / 1024F).ToString("0.00") + "KB";
                    }
                    else
                    {
                        prog = ((float)(e.Progress)).ToString("0.00") + "Bytes";
                    }
                }
                else
                {
                    prog = (e.Progress).ToString("0.00");
                }
            }
            ShowInfo();
        }
        public void ShowInfo()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(notiny + (string.IsNullOrWhiteSpace(prog) ? "" : "(" + prog + ")"));
        }
        public void ShowInfo(string info)
        {
            Console.WriteLine(info);
        }
        public bool Update(int i)
        {
            bool ok = false;
            try
            {
                var sua = new SvnUpdateArgs { Revision = new SvnRevision(i) };
                events();
                ok = SC.CleanUp(GetAppLoc());
                ok = SC.Update(GetAppLoc(), sua);
            }
            catch (Exception ex)
            {
                ShowInfo("更新时遇到问题：" + ex.Message + "\r\n");
                WriteError(ex);
            }
            ShowInfo("更新完成....");
            return ok;
        }
        public bool Update()
        {
            ShowInfo("正在更新....");
            SC.CleanUp(GetAppLoc());
            bool ok = SC.Update(GetAppLoc());
            if (!ok)
            {
                ShowInfo("未成功,正在清理...");
                SC.CleanUp(GetAppLoc());
                ShowInfo("清理完毕,正在更新....");
                ok = SC.Update(GetAppLoc());
                if (!ok)
                {
                    ShowInfo("正在撤销本地修改....");
                    SC.Revert(GetAppLoc());
                    ShowInfo("正在清理....");
                    SC.CleanUp(GetAppLoc());
                    ShowInfo("正在更新....");
                    ok = SC.Update(GetAppLoc());
                }
            }
            ShowInfo("更新操作完成");
            return ok;
        }
        private void Authentication_UserNameHandlers(object sender, SvnUserNameEventArgs e)
        {
            e.UserName = TCS.Config.AppConfig.RunTime.SVNUsername;
        }
        private void Authentication_UserNamePasswordHandlers(object sender, SvnUserNamePasswordEventArgs e)
        {
            e.Password = TCS.Config.AppConfig.RunTime.SVNPassword;
            e.UserName = TCS.Config.AppConfig.RunTime.SVNUsername;
        }
        #region 提示信息
        private string notiny;
        private void SC_Notify(object sender, SvnNotifyEventArgs e)
        {
            notiny = string.Format("{0}:{1}.", getstringact(e.Action), new FileInfo(e.FullPath).Name);
            ShowInfo();
        }
        #endregion
        #region 提示信息中文对照
        public string getstringact(SvnNotifyAction act)
        {
            string result = null;
            switch (act)
            {
                case SvnNotifyAction.Add:
                    result = "添加";
                    break;
                case SvnNotifyAction.BlameRevision:
                    break;
                case SvnNotifyAction.ChangeListClear:
                    break;
                case SvnNotifyAction.ChangeListMoved:
                    break;
                case SvnNotifyAction.ChangeListSet:
                    break;
                case SvnNotifyAction.CommitAddCopy:
                    result = "提交添加副本";
                    break;
                case SvnNotifyAction.CommitAdded:
                    break;
                case SvnNotifyAction.CommitDeleted:
                    break;
                case SvnNotifyAction.CommitModified:
                    break;
                case SvnNotifyAction.CommitReplaced:
                    break;
                case SvnNotifyAction.CommitReplacedWithCopy:
                    break;
                case SvnNotifyAction.CommitSendData:
                    break;
                case SvnNotifyAction.Copy:
                    break;
                case SvnNotifyAction.Delete:
                    break;
                case SvnNotifyAction.Excluded:
                    result = "要排除";
                    break;
                case SvnNotifyAction.Exists:
                    result = "已存在";
                    break;
                case SvnNotifyAction.ExternalFailed:
                    result = "外部失败";
                    break;
                case SvnNotifyAction.FailedConflict:
                    result = "失败冲突";
                    break;
                case SvnNotifyAction.FailedForbiddenByServer:
                    break;
                case SvnNotifyAction.FailedLocked:
                    break;
                case SvnNotifyAction.FailedMissing:
                    break;
                case SvnNotifyAction.FailedNoParent:
                    break;
                case SvnNotifyAction.FailedOutOfDate:
                    result = "已过期且失败";
                    break;
                case SvnNotifyAction.FollowUrlRedirect:
                    break;
                case SvnNotifyAction.LockFailedLock:
                    break;
                case SvnNotifyAction.LockFailedUnlock:
                    break;
                case SvnNotifyAction.LockLocked:
                    break;
                case SvnNotifyAction.LockUnlocked:
                    break;
                case SvnNotifyAction.MergeBegin:
                    break;
                case SvnNotifyAction.MergeBeginForeign:
                    break;
                case SvnNotifyAction.MergeCompleted:
                    break;
                case SvnNotifyAction.NonExistentPath:
                    break;
                case SvnNotifyAction.PatchApplied:
                    break;
                case SvnNotifyAction.PatchAppliedHunk:
                    break;
                case SvnNotifyAction.PatchFoundAlreadyApplied:
                    break;
                case SvnNotifyAction.PatchRejectedHunk:
                    break;
                case SvnNotifyAction.PropertyAdded:
                    break;
                case SvnNotifyAction.PropertyDeleted:
                    break;
                case SvnNotifyAction.PropertyDeletedNonExistent:
                    break;
                case SvnNotifyAction.PropertyModified:
                    break;
                case SvnNotifyAction.RecordMergeInfo:
                    break;
                case SvnNotifyAction.RecordMergeInfoElided:
                    break;
                case SvnNotifyAction.RecordMergeInfoStarted:
                    break;
                case SvnNotifyAction.Resolved:
                    break;
                case SvnNotifyAction.Restore:
                    result = "恢复";
                    break;
                case SvnNotifyAction.Revert:
                    result = "还原";
                    break;
                case SvnNotifyAction.RevertFailed:
                    result = "还原失败";
                    break;
                case SvnNotifyAction.RevisionPropertyDeleted:
                    break;
                case SvnNotifyAction.RevisionPropertySet:
                    break;
                case SvnNotifyAction.Skip:
                    result = "跳过";
                    break;
                case SvnNotifyAction.SkipConflicted:
                    result = "跳过冲突";
                    break;
                case SvnNotifyAction.StatusCompleted:
                    result = "状态已完成";
                    break;
                case SvnNotifyAction.StatusExternal:
                    break;
                case SvnNotifyAction.TreeConflict:
                    result = "数冲突";
                    break;
                case SvnNotifyAction.UpdateAdd:
                    result = "更新新增";
                    break;
                case SvnNotifyAction.UpdateCompleted:
                    result = "更新已完成";
                    break;
                case SvnNotifyAction.UpdateDelete:
                    result = "更新删除";
                    break;
                case SvnNotifyAction.UpdateExternal:
                    result = "更新外部";
                    break;
                case SvnNotifyAction.UpdateExternalRemoved:
                    result = "更新外部已删除";
                    break;
                case SvnNotifyAction.UpdateReplace:
                    result = "更新替换";
                    break;
                case SvnNotifyAction.UpdateShadowedAdd:
                    break;
                case SvnNotifyAction.UpdateShadowedDelete:
                    break;
                case SvnNotifyAction.UpdateShadowedUpdate:
                    break;
                case SvnNotifyAction.UpdateSkipAccessDenied:
                    break;
                case SvnNotifyAction.UpdateSkipObstruction:
                    break;
                case SvnNotifyAction.UpdateSkipWorkingOnly:
                    break;
                case SvnNotifyAction.UpdateStarted:
                    result = "更新已开始";
                    break;
                case SvnNotifyAction.UpdateUpdate:
                    result = "更新修改";
                    break;
                case SvnNotifyAction.UpgradedDirectory:
                    result = "更新目录";
                    break;
            }
            return result ?? (result = act.ToString());
        }
        #endregion
    }
    */
}
