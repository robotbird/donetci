using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SourceSafe.Interop;


namespace Utry.Framework
{
    /// <summary>
    /// 说明：提供vss操作的接口,可以作为单独的DLL发布
    /// 作者：ChengNing
    /// 日期：2012-12-27
    /// </summary>
    public class VssHelper
    {
        private string srcSafeIni = @"";

        public string SrcSafeIni
        {
            get { return srcSafeIni; }
            set { srcSafeIni = value; }
        }
        private string username = "";

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        private string password = "";

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //TODO:需要加入log4net来记录每一步的操作日志

        public VssHelper()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcsafeIni"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public VssHelper(string srcsafeIni, string username, string password)
        {
            this.srcSafeIni = srcsafeIni;
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// 格式化为vss的目录格式
        /// </summary>
        /// <param name="dir">s\f\a</param>
        /// <returns>s/f/a</returns>
        private string FormatToVssDir(string dir)
        {
            return dir.Replace(@"\", @"/");
        }

        /// <summary>
        /// 格式化为本地目录格式
        /// </summary>
        /// <param name="dir">s/f/a</param>
        /// <returns>s\f\a</returns>
        private string FormatToLocalDir(string dir)
        {
            return dir.Replace(@"/", @"\");
        }

        /// <summary>
        /// Add file 命令的接口实现，省掉注释comment
        /// </summary>
        /// <param name="vssWorkFolder">vss上的目录，文件需要添加到的vss目录</param>
        /// <param name="localFile">本地需要添加的文件，完整路径和名称</param>
        public void AddFile(string vssWorkFolder, string localFile)
        {
            AddFile(vssWorkFolder, localFile, "Adding a new file");
        }

        /// <summary>
        /// Add file 命令的接口核心实现
        /// </summary>
        /// <param name="vssWorkFolder"></param>
        /// <param name="localFile"></param>
        /// <param name="comment"></param>
        public void AddFile(string vssWorkFolder, string localFile, string comment)
        {
            //TODO：添加判断文件是否存在

            IVSSDatabase vssDatabase = new VSSDatabase();
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);
            VSSItem vssFolder = vssDatabase.get_VSSItem(vssWorkFolder, false);
            DisplayFolderContent(vssFolder);
            VSSItem vssTestFile = vssFolder.Add(localFile, comment, 0);
            DisplayFolderContent(vssFolder);
            vssTestFile.Destroy();
            DisplayFolderContent(vssFolder);
        }

        /// <summary>
        /// 从vss目录获取文件
        /// </summary>
        /// <param name="vssPath">vss文件目录</param>
        /// <param name="localPath">本地保存目录</param>
        public void Get(string vssPath, string localPath)
        {
            // Create a VSSDatabase object.
            IVSSDatabase vssDatabase = new VSSDatabase();

            // Open a VSS database using network name 
            // for automatic user login.
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssFile =
                     vssDatabase.get_VSSItem(vssPath, false);

            // Get a file into a specified folder.
            //string testFile = @"C:\1\test.txt";
            vssFile.Get(ref localPath, 0);

            //// Get a file into a working folder.
            //localPath = null;
            //vssFile.Get(ref localPath, 0);
            Console.WriteLine("The Get operation is completed");
        }

        /// <summary>
        /// vss check out命令
        /// </summary>
        /// <param name="vssFilePath">vss上的文件路径</param>
        /// <param name="localPath">本地文件路径</param>
        /// <param name="comment">checkout注释</param>
        public void CheckOut(string vssFilePath, string localPath, string comment)
        {
            vssFilePath = this.FormatToVssDir(vssFilePath);
            localPath = this.FormatToLocalDir(localPath);
            //string testFile = "$/TestFolder/test.txt";

            // Create a VSSDatabase object.
            IVSSDatabase vssDatabase = new VSSDatabase();

            // Open a VSS database using network name 
            // for automatic user login.
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssFile = vssDatabase.get_VSSItem(vssFilePath, false);

            vssFile.Checkout(comment, localPath, 0);

            if ((VSSFileStatus)vssFile.IsCheckedOut ==
               VSSFileStatus.VSSFILE_NOTCHECKEDOUT)
                Console.WriteLine(vssFile.Spec + " is checked in.");
            else
                Console.WriteLine(vssFile.Spec + " is checked out.");

            Console.WriteLine("Now alter the file and hit any key.");
            Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vssFilePath"></param>
        /// <param name="localPath"></param>
        /// <param name="comment"></param>
        public void UndoCheckOut(string vssFilePath, string comment)
        {
            vssFilePath = this.FormatToVssDir(vssFilePath);

            IVSSDatabase vssDatabase = new VSSDatabase();
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssFile = vssDatabase.get_VSSItem(vssFilePath, false);
            //只有取消自己检出的
            if ((VSSFileStatus)vssFile.IsCheckedOut == VSSFileStatus.VSSFILE_CHECKEDOUT_ME)
            {
                vssFile.UndoCheckout();

                Console.WriteLine("undo check out ." + vssFilePath);
            }
            Console.ReadLine();
        }



        /// <summary>
        /// vss check in命令
        /// </summary>
        /// <param name="localFilePath">本地文件路径</param>
        /// <param name="vssFilePath">vss上的文件路径</param>
        /// <param name="comment">checkin注释</param>
        public void CheckIn(string localFilePath, string vssFilePath, string comment)
        {
            vssFilePath = this.FormatToVssDir(vssFilePath);
            localFilePath = this.FormatToLocalDir(localFilePath);
            //string testFile = "$/TestFolder/test.txt";

            // Create a VSSDatabase object.
            IVSSDatabase vssDatabase = new VSSDatabase();

            // Open a VSS database using network name 
            // for automatic user login.
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssFile = vssDatabase.get_VSSItem(vssFilePath, false);

            //vssFile.Checkout("Checkout comment", @"C:\1\test.txt", 0);

            //if ((VSSFileStatus)vssFile.IsCheckedOut ==
            //   VSSFileStatus.VSSFILE_NOTCHECKEDOUT)
            //    Console.WriteLine(vssFile.Spec + " is checked in.");
            //else
            //    Console.WriteLine(vssFile.Spec + " is checked out.");

            //Console.WriteLine("Now alter the file and hit any key.");
            //Console.ReadLine();

            vssFile.Checkin(comment, localFilePath, 0);

            if ((VSSFileStatus)vssFile.IsCheckedOut ==
               VSSFileStatus.VSSFILE_NOTCHECKEDOUT)
                Console.WriteLine(vssFile.Spec + " is checked in.");
            else
                Console.WriteLine(vssFile.Spec + " is checked out.");
        }

        /// <summary>
        /// 
        /// </summary>
        public void NewProject(string foldName, string vssParentPath, string comment)
        {
            // Create a VSSDatabase object.
            IVSSDatabase vssDatabase = new VSSDatabase();

            // Open a VSS database using network name 
            // for automatic user login.
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssParentFolder = vssDatabase.get_VSSItem(vssParentPath, false);
            DisplayFolder(vssParentFolder);

            // Create folder C in folder A.
            Console.WriteLine("- Creating folder C in folder A");
            IVSSItem vssChildFolder =
                     vssParentFolder.NewSubproject(foldName, comment);
            DisplayFolder(vssParentFolder);

            //// Remove folder C from folder A.
            //Console.WriteLine("- Remove folder C from folder A");
            //vssChildFolder.Destroy();
            //DisplayFolder(vssParentFolder);
        }

        /// <summary>
        /// 根据vss的绝对路径创建一个Folder
        /// </summary>
        /// <param name="absoluteVssPath"></param>
        /// <param name="comment"></param>
        public void NewProject(string absoluteVssPath, string comment)
        {
            //string vssFolder = @"$/LIS－CM－AB/05-项目任务/B-契约模块/ABLREQUEST-1396-团险添加投保率不足0.75的控制提示/test";
            string vssFolder = @"$";//根目录
            string[] dir = absoluteVssPath.Split('/');

            IVSSDatabase vssDatabase = new VSSDatabase();
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);
            IVSSItem vssParentFolder = null;// = vssDatabase.get_VSSItem(vssFolder, false);
            bool existProject = false;
            for (int i = 1; i < dir.Length; i++)
            {
                vssFolder += @"/" + dir[i];
                //暂时使用异常来判断是否存在，之后优化
                try
                {
                    vssParentFolder = vssDatabase.get_VSSItem(vssFolder, false);
                    existProject = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    existProject = false;
                }
                if (!existProject) //不存在
                {
                    IVSSItem vssChildFolder = vssParentFolder.NewSubproject(dir[i], comment);
                    vssParentFolder = vssChildFolder;
                }
            }

        }

        /// <summary>
        /// vss Destroy 命令
        /// 删除销毁文件
        /// </summary>
        /// <param name="destroyFolder">vss目录中的路径</param>
        /// <param name="comment">destroy注释说明</param>
        public void Destroy(string destroyFolder, string comment)
        {
            // Create a VSSDatabase object.
            IVSSDatabase vssDatabase = new VSSDatabase();

            // Open a VSS database using network name 
            // for automatic user login.
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);

            IVSSItem vssDestroyFolder = vssDatabase.get_VSSItem(destroyFolder, false);

            // Remove folder C from folder A.
            Console.WriteLine("- Remove folder C from folder A");
            vssDestroyFolder.Destroy();
            DisplayFolder(vssDestroyFolder.Parent);
        }

        /// <summary>
        /// 判断指定的文件路径是否存在vss中
        /// </summary>
        /// <param name="vssFolderPath"></param>
        /// <returns></returns>
        public bool ExistFolder(string vssFolderPath)
        {
            IVSSDatabase vssDatabase = new VSSDatabase();
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);
            //暂时使用异常来判断是否存在，之后优化
            try
            {
                IVSSItem vssParentFolder = vssDatabase.get_VSSItem(vssFolderPath, false);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool IsCheckOut(string vssFilePath)
        {
            IVSSDatabase vssDatabase = new VSSDatabase();
            vssDatabase.Open(this.srcSafeIni, this.username, this.password);
            IVSSItem vssFile = vssDatabase.get_VSSItem(vssFilePath, false);

            if ((VSSFileStatus)vssFile.IsCheckedOut ==
               VSSFileStatus.VSSFILE_CHECKEDOUT)
                return true;
            else
                return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vssFolder"></param>
        private static void DisplayFolderContent(IVSSItem vssFolder)
        {
            Console.Write("\n{0} contains:", vssFolder.Spec);
            foreach (VSSItem vssItem in vssFolder.get_Items(false))
                Console.Write(" {0}", vssItem.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vssFolder"></param>
        private static void DisplayFolder(IVSSItem vssFolder)
        {
            Console.Write("{0} folder contains:", vssFolder.Spec);
            foreach (IVSSItem vssItem in vssFolder.get_Items(false))
                Console.Write(" {0}", vssItem.Name);
            Console.WriteLine();
        }
    }
}
