using _2D보험구분검증툴;
using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Test;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UnitTests
{
    [TestFixture]
    class 검증하기버튼
    {
        [SetUp]
        public void SetUp()
        {
            MessageHelper.IsTest = true;
        }

        [TearDown]
        public void TearDown()
        {
            MessageHelper.IsTest = false;
        }
        
        [Test]
        public void IsValid_경로가Null이거나EmptyString일때올바른메세지창나오나()
        {
            var 검증하기 = new 검증하기Logic(null);
            var frm = new Form1(검증하기, null, null);

            Assert.Multiple(() =>
            {
                frm.검증하기버튼(null);
                Assert.AreEqual("파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.", 검증하기.ErrorMessage);
                frm.검증하기버튼("");
                Assert.AreEqual("파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.", 검증하기.ErrorMessage);
            });
        }

        [Test]
        public void 검증버튼을눌렀을때파일경로가EmptyString이면False를반환()
        {
            var 검증하기 = new 검증하기Logic(null);
            var ret = 검증하기.Has파일경로(string.Empty);

            Assert.IsFalse(ret);
        }

        [Test]
        public void 검증버튼을눌렀을때파일경로가있다면True를반환()
        {
            var 검증하기 = new 검증하기Logic(null);
            var ret = 검증하기.Has파일경로(@"C:\");

            Assert.IsTrue(ret);
        }

        [Test]
        public void 검증버튼을눌렀을때파일경로에실제로파일이없으면False를반환()
        {
            var 검증하기 = new 검증하기Logic(null);
            var ret = 검증하기.Is파일존재(@"c:\asdf23dd4324.png"); // 없는 파일

            Assert.IsFalse(ret);
        }

        [Test]
        public void IsValid_경로에실제로파일이없을때올바른메세지창나오나()
        {
            var 검증하기 = new 검증하기Logic(null);
            검증하기.IsValid(@"c:\asdf23dd4324.png"); // 없는 파일

            Assert.AreEqual("입력하신 경로에 파일이 존재하지 않습니다.", 검증하기.ErrorMessage);
        }
    }
}
