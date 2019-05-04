/*
 Navicat Premium Data Transfer

 Source Server         : ci
 Source Server Type    : SQL Server
 Source Server Version : 10501600
 Source Host           : 10.0.2.16:1433
 Source Catalog        : UTRY_CI
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 10501600
 File Encoding         : 65001

 Date: 04/05/2019 17:18:05
*/


-- ----------------------------
-- Table structure for CICheckItem
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CICheckItem]') AND type IN ('U'))
	DROP TABLE [dbo].[CICheckItem]
GO

CREATE TABLE [dbo].[CICheckItem] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [VersionCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [DemandCode] varchar(100) COLLATE Chinese_PRC_CI_AS  NULL,
  [CodeList] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Attachment] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [Remark] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Developer] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Tester] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [AddTime] datetime  NULL,
  [UpdateTime] datetime  NULL,
  [Status] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [ValidateNote] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [DeployNote] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [GetVssCnt] int  NULL,
  [ItemType] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [PlanId] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CICheckItem] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键guid',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'版本号',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'VersionCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求编号',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'DemandCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'代码清单',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'CodeList'
GO

EXEC sp_addextendedproperty
'MS_Description', N'附件',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'Attachment'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'Remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开发人员',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'Developer'
GO

EXEC sp_addextendedproperty
'MS_Description', N'测试人员',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'Tester'
GO

EXEC sp_addextendedproperty
'MS_Description', N'添加时间',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'AddTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'修改时间',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'UpdateTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'Status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'验证方法描述',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'ValidateNote'
GO

EXEC sp_addextendedproperty
'MS_Description', N'部署方法注意事项',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'DeployNote'
GO

EXEC sp_addextendedproperty
'MS_Description', N'从vss上获取代码次数',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'GetVssCnt'
GO

EXEC sp_addextendedproperty
'MS_Description', N'类型(需求，bug)',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'ItemType'
GO

EXEC sp_addextendedproperty
'MS_Description', N'用户名',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'UserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'版本发布计划编号',
'SCHEMA', N'dbo',
'TABLE', N'CICheckItem',
'COLUMN', N'PlanId'
GO


-- ----------------------------
-- Table structure for CICodeLog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CICodeLog]') AND type IN ('U'))
	DROP TABLE [dbo].[CICodeLog]
GO

CREATE TABLE [dbo].[CICodeLog] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [AddTime] datetime  NULL,
  [CodeContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [DemandCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CheckListId] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CICodeLog] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键guid',
'SCHEMA', N'dbo',
'TABLE', N'CICodeLog',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'记录时间',
'SCHEMA', N'dbo',
'TABLE', N'CICodeLog',
'COLUMN', N'AddTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'代码内容',
'SCHEMA', N'dbo',
'TABLE', N'CICodeLog',
'COLUMN', N'CodeContent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求或者bug编号',
'SCHEMA', N'dbo',
'TABLE', N'CICodeLog',
'COLUMN', N'DemandCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'checklist表主键',
'SCHEMA', N'dbo',
'TABLE', N'CICodeLog',
'COLUMN', N'CheckListId'
GO


-- ----------------------------
-- Table structure for CICodeMenu
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CICodeMenu]') AND type IN ('U'))
	DROP TABLE [dbo].[CICodeMenu]
GO

CREATE TABLE [dbo].[CICodeMenu] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [CodeVersion] varchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
  [CodePath] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL,
  [PrjName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CICodeMenu] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键guid',
'SCHEMA', N'dbo',
'TABLE', N'CICodeMenu',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'代码版本',
'SCHEMA', N'dbo',
'TABLE', N'CICodeMenu',
'COLUMN', N'CodeVersion'
GO

EXEC sp_addextendedproperty
'MS_Description', N'文件相对路径',
'SCHEMA', N'dbo',
'TABLE', N'CICodeMenu',
'COLUMN', N'CodePath'
GO

EXEC sp_addextendedproperty
'MS_Description', N'所属工程名称',
'SCHEMA', N'dbo',
'TABLE', N'CICodeMenu',
'COLUMN', N'PrjName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'CICodeMenu',
'COLUMN', N'Status'
GO


-- ----------------------------
-- Table structure for CIConfig
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIConfig]') AND type IN ('U'))
	DROP TABLE [dbo].[CIConfig]
GO

CREATE TABLE [dbo].[CIConfig] (
  [KeyName] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [KeyValue] nvarchar(200) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIConfig] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for CILog
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CILog]') AND type IN ('U'))
	DROP TABLE [dbo].[CILog]
GO

CREATE TABLE [dbo].[CILog] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [LogTime] datetime  NULL,
  [Contents] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [UserName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [DemandCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [CodeFile] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CILog] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'日志记录时间',
'SCHEMA', N'dbo',
'TABLE', N'CILog',
'COLUMN', N'LogTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'操作内容',
'SCHEMA', N'dbo',
'TABLE', N'CILog',
'COLUMN', N'Contents'
GO

EXEC sp_addextendedproperty
'MS_Description', N'用户名',
'SCHEMA', N'dbo',
'TABLE', N'CILog',
'COLUMN', N'UserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'需求编号',
'SCHEMA', N'dbo',
'TABLE', N'CILog',
'COLUMN', N'DemandCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'代码文件名称',
'SCHEMA', N'dbo',
'TABLE', N'CILog',
'COLUMN', N'CodeFile'
GO


-- ----------------------------
-- Table structure for CIProject
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIProject]') AND type IN ('U'))
	DROP TABLE [dbo].[CIProject]
GO

CREATE TABLE [dbo].[CIProject] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ProjectCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ProjectName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectManager] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectMember] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectTestURL] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [DBTestURL] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectSvnURL] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [AddTime] datetime  NULL,
  [Remark] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Executive] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UpdateTime] datetime  NULL,
  [DBDevelopURL] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [DBFormalURL] varchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectSvnURLRelease] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [ProjectFormalURL] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [SlnName] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [PackagePath] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIProject] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目编号',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目名称',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目经理',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectManager'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目成员',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectMember'
GO

EXEC sp_addextendedproperty
'MS_Description', N'测试地址',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectTestURL'
GO

EXEC sp_addextendedproperty
'MS_Description', N'数据库地址',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'DBTestURL'
GO

EXEC sp_addextendedproperty
'MS_Description', N'SVN地址',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'ProjectSvnURL'
GO

EXEC sp_addextendedproperty
'MS_Description', N'添加日期',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'AddTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'Remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'打包路径',
'SCHEMA', N'dbo',
'TABLE', N'CIProject',
'COLUMN', N'PackagePath'
GO


-- ----------------------------
-- Table structure for CIRelease
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIRelease]') AND type IN ('U'))
	DROP TABLE [dbo].[CIRelease]
GO

CREATE TABLE [dbo].[CIRelease] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ProjectID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [BeginTime] datetime  NULL,
  [EndTime] datetime  NULL,
  [LogContent] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Operator] nchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [AddTime] datetime  NOT NULL,
  [VersionURL] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [Type] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Remark] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [TestStatus] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Reversion] bigint  NULL
)
GO

ALTER TABLE [dbo].[CIRelease] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目编号',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'ProjectID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始时间',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'BeginTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束时间',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'EndTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'日志',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'LogContent'
GO

EXEC sp_addextendedproperty
'MS_Description', N'发布状态',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'Status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'操作人员',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'Operator'
GO

EXEC sp_addextendedproperty
'MS_Description', N'添加时间',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'AddTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'版本地址',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'VersionURL'
GO

EXEC sp_addextendedproperty
'MS_Description', N'版本类型',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'Type'
GO

EXEC sp_addextendedproperty
'MS_Description', N'备注',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'Remark'
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIRelease',
'COLUMN', N'Reversion'
GO


-- ----------------------------
-- Table structure for CIReport
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIReport]') AND type IN ('U'))
	DROP TABLE [dbo].[CIReport]
GO

CREATE TABLE [dbo].[CIReport] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ReportName] varchar(500) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ReportSQL] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [AddTime] datetime  NOT NULL
)
GO

ALTER TABLE [dbo].[CIReport] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIReport',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'报表名称',
'SCHEMA', N'dbo',
'TABLE', N'CIReport',
'COLUMN', N'ReportName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'报表SQL语句',
'SCHEMA', N'dbo',
'TABLE', N'CIReport',
'COLUMN', N'ReportSQL'
GO

EXEC sp_addextendedproperty
'MS_Description', N'添加时间',
'SCHEMA', N'dbo',
'TABLE', N'CIReport',
'COLUMN', N'AddTime'
GO


-- ----------------------------
-- Table structure for CIReview
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIReview]') AND type IN ('U'))
	DROP TABLE [dbo].[CIReview]
GO

CREATE TABLE [dbo].[CIReview] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ProjectCode] varchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Purpose] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Method] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Attachment] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NULL,
  [Result] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NULL,
  [Scale] nvarchar(2000) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [IfReview] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [PrepareTime] varchar(10) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Member] varchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [BeginDate] datetime  NOT NULL,
  [EndDate] datetime  NOT NULL,
  [AddTime] datetime  NULL,
  [UpdateTime] datetime  NULL
)
GO

ALTER TABLE [dbo].[CIReview] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'项目编号',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'ProjectCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审目的',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Purpose'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审方式',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Method'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审资料',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Attachment'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审结果',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Result'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审规模',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Scale'
GO

EXEC sp_addextendedproperty
'MS_Description', N'是否复审',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'IfReview'
GO

EXEC sp_addextendedproperty
'MS_Description', N'会议准备工作量',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'PrepareTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审成员',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Member'
GO

EXEC sp_addextendedproperty
'MS_Description', N'评审状态',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'Status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始时间',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'BeginDate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束时间',
'SCHEMA', N'dbo',
'TABLE', N'CIReview',
'COLUMN', N'EndDate'
GO


-- ----------------------------
-- Table structure for CIReviewProblem
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIReviewProblem]') AND type IN ('U'))
	DROP TABLE [dbo].[CIReviewProblem]
GO

CREATE TABLE [dbo].[CIReviewProblem] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ReviewID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [DemandCode] varchar(100) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Description] nvarchar(500) COLLATE Chinese_PRC_CI_AS  NULL,
  [Provider] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Deadline] datetime  NULL,
  [Solver] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [IfSolve] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [AddTime] datetime  NULL,
  [UpdateTime] datetime  NULL,
  [DevelopTime] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [DesignTime] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [TestTime] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIReviewProblem] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for CIUser
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIUser]') AND type IN ('U'))
	DROP TABLE [dbo].[CIUser]
GO

CREATE TABLE [dbo].[CIUser] (
  [UserName] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [Email] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [UTMPName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Mobile] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Role] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [PassWord] varchar(32) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [RegTime] datetime  NULL,
  [FullName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [OrgCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIUser] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'用户名',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'UserName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'邮箱',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'Email'
GO

EXEC sp_addextendedproperty
'MS_Description', N'utmp用户名',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'UTMPName'
GO

EXEC sp_addextendedproperty
'MS_Description', N'手机',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'Mobile'
GO

EXEC sp_addextendedproperty
'MS_Description', N'角色',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'Role'
GO

EXEC sp_addextendedproperty
'MS_Description', N'密码',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'PassWord'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'Status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'注册时间',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'RegTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'真实姓名',
'SCHEMA', N'dbo',
'TABLE', N'CIUser',
'COLUMN', N'FullName'
GO


-- ----------------------------
-- Table structure for CIUserOrg
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIUserOrg]') AND type IN ('U'))
	DROP TABLE [dbo].[CIUserOrg]
GO

CREATE TABLE [dbo].[CIUserOrg] (
  [OrgName] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [OrgCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [ParentCode] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIUserOrg] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for CIVersionPlan
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[CIVersionPlan]') AND type IN ('U'))
	DROP TABLE [dbo].[CIVersionPlan]
GO

CREATE TABLE [dbo].[CIVersionPlan] (
  [ID] varchar(50) COLLATE Chinese_PRC_CI_AS  NOT NULL,
  [BeginTime] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [EndTime] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [OpenDate] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [PlanCode] nvarchar(50) COLLATE Chinese_PRC_CI_AS  NULL,
  [Status] nvarchar(10) COLLATE Chinese_PRC_CI_AS  NULL,
  [Note] nvarchar(max) COLLATE Chinese_PRC_CI_AS  NULL,
  [AddTime] datetime  NULL,
  [UpdateTime] datetime  NULL,
  [UserName] varchar(50) COLLATE Chinese_PRC_CI_AS  NULL
)
GO

ALTER TABLE [dbo].[CIVersionPlan] SET (LOCK_ESCALATION = TABLE)
GO

EXEC sp_addextendedproperty
'MS_Description', N'主键',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'ID'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开始时间',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'BeginTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'结束时间',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'EndTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'开放提交日期',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'OpenDate'
GO

EXEC sp_addextendedproperty
'MS_Description', N'计划编号',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'PlanCode'
GO

EXEC sp_addextendedproperty
'MS_Description', N'状态',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'Status'
GO

EXEC sp_addextendedproperty
'MS_Description', N'说明',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'Note'
GO

EXEC sp_addextendedproperty
'MS_Description', N'添加时间',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'AddTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'更新时间',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'UpdateTime'
GO

EXEC sp_addextendedproperty
'MS_Description', N'用户名',
'SCHEMA', N'dbo',
'TABLE', N'CIVersionPlan',
'COLUMN', N'UserName'
GO


-- ----------------------------
-- View structure for V_Bug
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[V_Bug]') AND type IN ('V'))
	DROP VIEW [dbo].[V_Bug]
GO

CREATE VIEW [dbo].[V_Bug] AS SELECT     BUGNumber, BUGState, VersionNum
FROM         utmp_link.utmp.dbo.vi_getallbugversion AS V_Bug
GO


-- ----------------------------
-- View structure for V_Demand
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[V_Demand]') AND type IN ('V'))
	DROP VIEW [dbo].[V_Demand]
GO

CREATE VIEW [dbo].[V_Demand] AS SELECT     DemandNumber, DemandState, VersionNum
FROM         utmp_link.utmp.dbo.vi_getalldemandversion AS V_Demand
GO


-- ----------------------------
-- View structure for V_DemandAndBug
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[V_DemandAndBug]') AND type IN ('V'))
	DROP VIEW [dbo].[V_DemandAndBug]
GO

CREATE VIEW [dbo].[V_DemandAndBug] AS SELECT     code, DemandState, VersionNum
FROM         (SELECT     DemandNumber AS code, DemandState, VersionNum
                       FROM          dbo.V_Demand
                       UNION ALL
                       SELECT     BUGNumber AS code, BUGState AS demandstate, VersionNum
                       FROM         dbo.V_Bug) AS t
GO


-- ----------------------------
-- Function structure for StrToTable
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[StrToTable]') AND type IN ('FN', 'FS', 'FT', 'IF', 'TF'))
	DROP FUNCTION[dbo].[StrToTable]
GO

CREATE FUNCTION [dbo].[StrToTable](@str varchar(1000))
Returns @tableName Table
(
   str2table varchar(50)
)
As
--该函数用于把一个用逗号分隔的多个数据字符串变成一个表的一列，例如字符串'1,2,3,4,5' 将编程一个表，这个表
Begin
set @str = @str+','
Declare @insertStr varchar(50) --截取后的第一个字符串
Declare @newstr varchar(1000) --截取第一个字符串后剩余的字符串
set @insertStr = left(@str,charindex(',',@str)-1)
set @newstr = stuff(@str,1,charindex(',',@str),'')
Insert @tableName Values(@insertStr)
while(len(@newstr)>0)
begin
   set @insertStr = left(@newstr,charindex(',',@newstr)-1)
   Insert @tableName Values(@insertStr)
   set @newstr = stuff(@newstr,1,charindex(',',@newstr),'')
end
Return
End
GO


-- ----------------------------
-- Function structure for fn_countword
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_countword]') AND type IN ('FN', 'FS', 'FT', 'IF', 'TF'))
	DROP FUNCTION[dbo].[fn_countword]
GO

CREATE FUNCTION [dbo].[fn_countword]
(
    @Word NVARCHAR(200),
    @WordAll NVARCHAR(2000)
)
RETURNS CHAR(4)
AS
BEGIN
    RETURN  len(replace(@WordAll,@Word,@Word+'_'))-len(@WordAll)
END
GO


-- ----------------------------
-- Primary Key structure for table CICheckItem
-- ----------------------------
ALTER TABLE [dbo].[CICheckItem] ADD CONSTRAINT [PK_CICheckList] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CICodeLog
-- ----------------------------
ALTER TABLE [dbo].[CICodeLog] ADD CONSTRAINT [PK_CICodeLog] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CICodeMenu
-- ----------------------------
ALTER TABLE [dbo].[CICodeMenu] ADD CONSTRAINT [PK_CICodeMenu] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIConfig
-- ----------------------------
ALTER TABLE [dbo].[CIConfig] ADD CONSTRAINT [PK_CIConfig] PRIMARY KEY CLUSTERED ([KeyName])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIReview
-- ----------------------------
ALTER TABLE [dbo].[CIReview] ADD CONSTRAINT [PK_CIReview] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIReviewProblem
-- ----------------------------
ALTER TABLE [dbo].[CIReviewProblem] ADD CONSTRAINT [PK_CIReviewProblem] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIUser
-- ----------------------------
ALTER TABLE [dbo].[CIUser] ADD CONSTRAINT [PK_CIUser] PRIMARY KEY CLUSTERED ([UserName])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIUserOrg
-- ----------------------------
ALTER TABLE [dbo].[CIUserOrg] ADD CONSTRAINT [PK_CIUserOrg] PRIMARY KEY CLUSTERED ([OrgCode])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table CIVersionPlan
-- ----------------------------
ALTER TABLE [dbo].[CIVersionPlan] ADD CONSTRAINT [PK_CIVersionPlan] PRIMARY KEY CLUSTERED ([ID])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO

