# 作品背景
这个.net 持续集成作品还是在2014年的时候从事.net 软件项目开发的时候做的，当时部门还用着vs2008用vb.net做项目(现在也是)，项目代码极混乱，版本工具用的vss，而且用的不怎么顺，很多时候发布项目版本还是通过邮件发送代码vs工具手工编译打包的，并且安排了一个所谓的中高级.net开发人员每天的事情就是坐在那通过邮件收代码合并代码，打包编译，这个人请假了其他人还不会打包，主要是整个代码太乱了，只能部门经理打包，想想当年部门经理每天加班合并邮件里的开发代码的时候，觉得都不可思议，甚至有一次一个开发主管打包因为打包的时候代码的时候少发了一个页面文件并且上线到客户生产环境了，直接导致生产故障，被客户投诉，直接扣了20%绩效，而且每次我改完代码提交的时候经常少发文件，被管理员批。

实在忍不住了跟这个版本管理员说“你就不能写个工具自动打包，哪怕弄个批处理也行啊”

得到的答复是“你会写你写啊”，这样的产品研发部门我也是醉了。

吃饭的时候跟部门经理聊到工具对提高生产力的问题的时候，我说我实在忍受不了公司的技术现状了，需要将部门vss工具替换到svn，引入一个持续集成工具或者做个自动打包脚本，部门经理跟我来了一句 “关键是管理流程，工具只是手段而已” ，当时就震惊石化了，部门的技术管理水平都烂成这样了还跟我谈管理，虽然心里一万草泥马奔腾，但是当时还是有勇气担当和闯进的，哪怕是用vss也要把持续集成工具做出来。


# 开发过程
领导还没同意用svn，还得用vss，最后发现vss的第三方接口开发实在问题太多了，几乎寸步难行，仅仅调通了代码获取，提交日志都获取不到。最后终于说服部门领导采用svn来管理代码了，并且同意我做这个持续集成工具，虽然我知道用git更好，但是我清楚公司开发的认知水平和技能，能够成功转到svn就不易了，话说当时三十几人没有一个会用git，要不也直接用git了。

当时我已经知道有jenkins这样优秀的持续集成工具了，但是由于这个工具是基于java开发的，二次扩展方面对我们团队还是比较困难，想想自己如果能够持续造一个轮子把公司的开发管理平台一点点做出来，从需求bug管理、到需求评审、版本打包，版本发布一整套全做完善，也应该是很有意思的事情，如是带着个毕业生就开始干起来了。

做的过程是没人能理解我在干什么，刚开始做出来的东西也是不太稳定，测试人员打包的时候就吐槽“什么烂东西”，最终大概花了2周时间把部门整个从vss切换到svn，并且这个工具已经基本可以使用了，并且花了很大精力把代码库从900多M缩减到100M以下，这个也是纯粹清理垃圾的活，很多项目上定制的代码没用了，都在主版本里，直接弄一各backup文件夹把工程代码放进去，而且一个项目模块方一个，而且是大量的重复，最后不得不写一个批处理来清理。

最终工具还是做出来了，直接导致版本管理员这个古老的角色“失业”，安排写代码去了，并且领导又要求把需求管理和需求评审的登记功能做出来了，前后估计花了将近一个月时间，最终需求评审功能没啥鸟用，实行了一段时间后废弃。

年底进行公司创新大赛评比的时候竟然入选了，当时让我先找个工具报告ppt的时候，内心也是排斥的，这都能算得上创新，只是重复造了个轮子罢了，到年会上竟然在那么热闹的场合介绍这玩意，看到其他小伙伴的创新更是天马行空的ppt之后，还是觉得不管多少还是为公司做了点实事，并且发了2000块钱辛苦费，心想这么简单的东西节省一个岗位成本加时间成本一年好歹有几十万吧，可见创新对公司确实是件好事，但是对个人的价格(不是价值)其实还很低廉的。


# 产品说明
这个作品的最终的展示如下

产品首页，这个页面非常简单，连分页都没有，就是列出所有项目，而且最新发包的项目在最上面。
![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504150422548-587479901.png)

项目管理首页，列出所有的项目清单，并且项目配置页面可以配置项目的svn地址，以及项目的基本信息。
![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504150738612-1746215395.png)


单个项目的版本发布列表，这个界面是核心操作模块，通过点击版本发布可以发布测试版本和正式版本，并且2个版本是基于正式的代码svn分支和测试的分支，代码测试完成后再合并到正式分支发布正式版本，足够简单，老少皆宜。

如果打包失败可以通过“查看日志”来查看编译失败的原因，通过“查看备注”可以查看提交的代码的提交记录和操作人，并且发布成功后才可以下载，由于这个东西还在用着，所以需要马克一下。
![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504151212411-1094100355.png)


发布统计功能，这个功能也是当时领导比较关注的，看到有这么好的工具，赶紧搞一个统计，可以根据这个来做考核（为啥领导的思维都是啥都想到考核呢），做了一个非常好用的报表工具，那就是直接在页面上写sql统计代码（O(∩_∩)O）。

![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504152215588-1860190451.png)


代码打包控制台程序，没有在web页面实现jenkins那样的控制台程序，偷懒的方式在控制台程序里进行svn代码的获取以及调用系统的winrar软件来实现打包。

![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504153148789-1580807603.png)

# 实现机制
这个持续集成工具实现的时候并没有太多参考jenkins，而是根据现有的技术空间和时间，快速的把每天人肉发包的问题解决掉。

![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504162053083-1935290266.png)


# 技术架构
## 技术栈
- .net framework 4.0
- .net mvc3
- sqlserver 2005
- razor 模板引擎
- dapper 轻量级orm框架
- vs2010 社区版本(现在换2017打开了)
- SharpSvn (svn接口)
- MsBuild （编译工具)
- winrar （压缩工具)
- 
## 代码结构
![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504145529251-2002217192.png)
从上之下分为以下工程目录结构：

- `Test` 用于开发时候的单元测试
- `Utry.CI` Web应用
- `Utry.CIConsole` 控制台应用
- `Utry.Core` 业务层代码(采用领域架构模式)
- `Utry.Framework` 框架基础类库，其实都来自自己之前开发的cms程序里  [jqpres.cms](https://github.com/robotbird/jqpress.cms)

## 数据结构如下
![](https://img2018.cnblogs.com/blog/94489/201905/94489-20190504173749015-758826244.png)

- `CICheckItem` 需求记录
- `CICodeLog` 好像没用
- `CICodeMenu` 不知道干啥的，好像没用
- `CIConfig` 好像没用
- `CILog` 版本发布的代码提交日志
- `CIProjiect` 项目表
- `CIRelease` 版本发布表
- `CIReport` 报表配置表
- `CIReview` 需求评审表
- `CIReviewProblem` 需求评审记录表
- `CIUser`  用户管理表
- `CIUserOrg` 组织管理表
- `CIVersionPlan` 版本发布计划

 
## 编译配置脚本

这个脚本就是MsBuild 调用的脚本，其中{SlnName}为项目的 sln文件地址，项目会跟进配置信息进行替换。

```
<?xml version="1.0" encoding="utf-8"?>
<Project  xmlns="http://schemas.microsoft.com/developer/msbuild/2003">

<Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />

  <PropertyGroup>
        <VName>{BranchName}_Release</VName>
        <WebAppOutput>..\WebAppPublished</WebAppOutput>
         <WebSiteOutput>..\WebSitePublished</WebSiteOutput>
   
  </PropertyGroup>

<ItemGroup>  
  <MySourceFiles Include="web\**\*.dll;web\**\*.aspx;web\**\*.js;web\**\*.css;web\**\*.jpg;web\**\*.config;web\**\*.gif;web\**\*.config;web\**\*.png;web\**\*.ascx;"/>
</ItemGroup> 

  <Target Name="build">
    <MsBuild Projects="{SlnName}" Targets="$(BuildCmd)" />
   </Target>
   
</Project>

``` 
## 调用winrar压缩打包部分代码
代码会通过根据注册表里是否有winrar安装信息，然后找到路径进行调用。

```
        /// <summary>  
        /// 生成Zip  
        /// </summary>  
        /// <param name="path">文件夹路径</param>  
        /// <param name="rarPath">生成压缩文件的路径</param>  
        /// <param name="rarName">生成压缩文件的文件名</param>
        /// <param name="ignore">忽略的文件</param>
        public static void CompressRar(String path, String rarPath, String rarName,string ignore)
        {
            try
            {
                String winRarPath = null;
                if (!ExistsRar(out winRarPath)) return;//验证WinRar是否安装。  

                //var pathInfo = String.Format("a -afzip -m0 -ep1 \"{0}\" \"{1}\"", rarName, path);
                var pathInfo = String.Format("a {2} -k -r -s -ep1  \"{0}\" \"{1}\"", rarName, path,ignore);

                #region WinRar 用到的命令注释

                //[a] 添加到压缩文件  
                //afzip 执行zip压缩方式，方便用户在不同环境下使用。（取消该参数则执行rar压缩）  
                //-m0 存储 添加到压缩文件时不压缩文件。共6个级别【0-5】，值越大效果越好，也越慢  
                //ep1 依名称排除主目录（生成的压缩文件不会出现不必要的层级）  
                //r   修复压缩档案  
                //t   测试压缩档案内的文件   
                //as  同步压缩档案内容    
                //-p  给压缩文件加密码方式为：-p123456  

                #endregion

                //打包文件存放目录  
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = winRarPath,//执行的文件名  
                        Arguments = pathInfo,//需要执行的命令  
                        UseShellExecute = false,//使用Shell执行  
                        WindowStyle = ProcessWindowStyle.Hidden,//隐藏窗体  
                        WorkingDirectory = rarPath,//rar 存放位置  
                        CreateNoWindow = true,//不显示窗体  
                    },
                };
                process.Start();//开始执行  
                process.WaitForExit();//等待完成并退出  
                process.Close();//关闭调用 cmd 的什么什么  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
```

# 源码地址

https://github.com/robotbird/donetci

为了防止部分同学无法访问Github，所以放oschina的gitee上了

https://gitee.com/robotbirdold/donetci

# 总结
由于这个工具是根据当时的项目管理模式非常定制的，没有完全产品化的去做，况且现在jenkins已经非常好用了，这个轮子权大家学习(观摩)使用，如果真的谁有兴趣再想把这个轮子用起来，请微信robotbird798联系我吧，事隔这么多年才把这些东西发出来，初心还是对技术的不舍，在这个公司这么多年被安排着和妥协着从.net开发主管->项目经理->java开发->产品经理->部门经理->? 的曲折之路，忽然发现人生走了很多的弯路，以前把大把的时间和热情奉献给公司，如今到了中年危机之时，才发现不过是炮灰，没有赢得一场战争，如今只能收拾残破的灵魂，重整旗鼓，为自己而战，为心中那份不灭的理想而战，前途不知道会怎样，但又如何呢，只要勤奋不至于不能养家糊口，至少为理想战斗过。


