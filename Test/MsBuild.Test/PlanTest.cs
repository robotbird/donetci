using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsBuild.Test
{
    /// <summary>
    /// Plan 的摘要说明
    /// </summary>
    [TestClass]
    public class PlanTest
    {
        public PlanTest()
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

        public void TestAutoPlanCode()
        {
            
            var a = "2014-05-22 17:20";
            var b = "2014-05-29 17:30";
            var a1=Convert.ToDateTime(a);
            var b2=Convert.ToDateTime(b);
            var code1 ="CCMS"+ a1.ToString("yyyyMMdd").Substring(2, 6) + "_" + b2.ToString("yyyyMMdd").Substring(2, 6);
            Console.WriteLine(code1);
        }

        [TestMethod]
        public void TestSubstring()
        {
            var m_connectionstring = "Provider=SQLOLEDB;Data Source=10.0.2.13;Initial Catalog=CCMS_SHJH_V7;User Id=sa;Password=cs";
            m_connectionstring = m_connectionstring.Substring(m_connectionstring.IndexOf("SQLOLEDB") + 9, m_connectionstring.Length - m_connectionstring.IndexOf("SQLOLEDB") - 9);
            Console.Write(m_connectionstring);
        }
    }
}
