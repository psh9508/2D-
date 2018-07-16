using _2D보험구분검증툴;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClassLibrary1
{
    [TestFixture]
    public class Class1
    {
        Form1 frm = null;
        TextBox txt파일경로 = null;

        [SetUp]
        public void SetUp()
        {
            frm = new Form1();
            txt파일경로 = frm.Get파일경로;
        }

        [Test]
        public void Test_1()
        {
            if (string.IsNullOrEmpty(txt파일경로.Text))
            {

            }

            Assert.AreEqual(1, 1);
        }
    }
}
