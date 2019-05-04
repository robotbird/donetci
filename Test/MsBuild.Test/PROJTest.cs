using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Build.BuildEngine;


namespace MsBuild.Test
{
    [TestClass]
    public class PROJTest
    {
        
        public void TestMethod1()
        {
            //通过msbuild添加到工程文件

            //Engine.GlobalEngine.BinPath = @"C:\Windows\Microsoft.NET\Framework\v2.0.xxxxx";

            // Create a new empty project
            Project project = new Project();

            // Load a project
            project.Load(@"E:\project\donetCI工具\Utry.CI\ccms\CCMS_V6\web\SCH.vbproj");


                //<Compile Include="reports\yepeng.aspx.designer.vb">
                //  <DependentUpon>yepeng.aspx</DependentUpon>
                //</Compile>
                //<Compile Include="reports\yepeng.aspx.vb">
                //  <DependentUpon>yepeng.aspx</DependentUpon>
                //  <SubType>ASPXCodebehind</SubType>
                //</Compile>
            var include = @"reports\yepeng2.aspx";
            var includedesigner = @"reports\yepeng2.aspx.designer.vb";
            var includevb = @"reports\yepeng2.aspx.vb";
            //var isnewdesigner = true;
            var isnew = true;
            foreach (BuildItemGroup itemGroup in project.ItemGroups)
            {
                foreach (BuildItem item in itemGroup)
                {
                    if (item.Include == include)
                    {
                        isnew=false;
                        break;
                    }
                }
                //foreach (BuildItem item in itemGroup)
                //{
                //    if (item.Include == includedesigner)
                //    {
                //        isnewdesigner = false;
                //        break;
                //    }
                //}
            }
            if (isnew) 
            {
                 var itemGroup = project.AddNewItemGroup();
                 var buildItem = itemGroup.AddNewItem("Content", include);

                 var buildItemdesigner = itemGroup.AddNewItem("Compile", includedesigner);
                 buildItemdesigner.SetMetadata("DependentUpon", "yepeng2.aspx");

                 var buildItemvb = itemGroup.AddNewItem("Compile",includevb);
                 buildItemvb.SetMetadata("DependentUpon", "yepeng2.aspx");
                 buildItemvb.SetMetadata("SubType", "ASPXCodebehind");
            }
            //if (isnewdesigner)
            //{
            //    var itemGroup = project.AddNewItemGroup();
            //    var buildItem = itemGroup.AddNewItem("Compile", includedesigner);
            //    buildItem.SetMetadata("DependentUpon", "yepeng2.aspx");
            //}
            project.Save(@"E:\project\donetCI工具\Utry.CI\ccms\CCMS_V6\web\SCH.vbproj");
        }

       
        public void TestAddClassItem()
        {
            //通过msbuild添加到工程文件

            //Engine.GlobalEngine.BinPath = @"C:\Windows\Microsoft.NET\Framework\v2.0.xxxxx";

            // Create a new empty project
            Project project = new Project();

            // Load a project
            project.Load(@"E:\project\donetCI工具\Utry.CI\ccms\CCMS_V6\Common_Library\Common_Library.vbproj");


            //<Compile Include="reports\yepeng.aspx.designer.vb">
            //  <DependentUpon>yepeng.aspx</DependentUpon>
            //</Compile>
            //<Compile Include="reports\yepeng.aspx.vb">
            //  <DependentUpon>yepeng.aspx</DependentUpon>
            //  <SubType>ASPXCodebehind</SubType>
            //</Compile>
            var include = @"test.vb";
            var itemGroup = project.AddNewItemGroup();
            var buildItem = itemGroup.AddNewItem("Content", include);

            project.Save(@"E:\project\donetCI工具\Utry.CI\ccms\CCMS_V6\Common_Library\Common_Library.vbproj");
        }

        public void TestEnd() 
        {
            var vfile = @"CCMS_V7\QTS\qts.vb";
           var  ccmsversion = "CCMS_V7";
           var vbproj = "";
            if (vfile.EndsWith(".vb") && vfile.IndexOf(".aspx") < 0)
            {
                var t1 = vfile.Substring(ccmsversion.Length, vfile.Length - ccmsversion.Length);
                vbproj = t1.Substring(1, t1.LastIndexOf(@"\")-1);
            }
            Console.WriteLine(vbproj);
        }
        [TestMethod]
        public void TestLastIndexOf() 
        {
            var vfile = @"HRM\HrmStaffScoreOper.vb";
            Console.WriteLine(vfile.LastIndexOf("HrmStaffScoreOper.vb"));
        }
        
        public void TestFile() 
        {
            var vfile = @"CCMS_V7/Web/QTS/qts_plan_edit.aspx";
            var vfiledesigner = @"CCMS_V7/Web/QTS/qts_plan_edit.aspx.designer.vb";
            var vfilevb = @"CCMS_V7/Web/QTS/qts_plan_edit.aspx.vb";
            var foldname = vfile.Substring(0, vfile.LastIndexOf("/"));

            var f = @"QTS/qts_plan_edit.aspx";
            Console.WriteLine((f.Length-5)+"-"+f.LastIndexOf(".aspx"));

            var include = vfile.Substring(foldname.LastIndexOf("/")+1, vfile.Length - foldname.LastIndexOf("/")-1);
            var includedesigner = vfiledesigner.Substring(foldname.LastIndexOf("/") + 1, vfiledesigner.Length - foldname.LastIndexOf("/") - 1);
            var includevb = vfilevb.Substring(foldname.LastIndexOf("/") + 1, vfilevb.Length - foldname.LastIndexOf("/") - 1);
            Console.Write(includevb);
        }
    }
}
