using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utry.Core.Domain
{
    public class CIProject
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public string ProjectManager { get; set; }

        /// <summary>
        /// 开发主管
        /// </summary>
        public string Executive { get; set; }

        /// <summary>
        /// 项目成员
        /// </summary>
        public string ProjectMember { get; set; }

        /// <summary>
        /// 项目正式地址
        /// </summary>
        public string ProjectFormalURL { get; set; }

        /// <summary>
        /// 项目测试地址
        /// </summary>
        public string ProjectTestURL { get; set; }

        /// <summary>
        /// 测试库地址
        /// </summary>
        public string DBTestURL { get; set; }

        /// <summary>
        /// 项目SVN测试地址
        /// </summary>
        public string ProjectSvnURL { get; set; }

        /// <summary>
        /// 项目svn地址正式版本
        /// </summary>
        public string ProjectSvnURLRelease { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 开发库地址
        /// </summary>
        public string DBDevelopURL { get; set; }

        /// <summary>
        /// 正式库地址
        /// </summary>
        public string DBFormalURL { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 解决方案文件名称
        /// </summary>
        public string SlnName { get; set; }

        /// <summary>
        /// 需要打包的路径
        /// </summary>
        public string PackagePath { get; set; }

    }
}
