using _2D보험구분검증툴;
using _2D보험구분검증툴.Class.Logic.QRCode;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests
{
    public class 검증하기LogicByFileTests
    {
        //[Test]
        //public void IsValid_경로가Null이거나EmptyString일때올바른메세지창나오나()
        //{
        //    var Mock_검증하기= new Mock<I검증하기>

        //    var 검증하기 = new 검증하기LogicByFile(null, null);
        //    var frm = new Form1(null, null);

        //    frm.검증하기버튼("", null, null);
        //    Assert.AreEqual("파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.", 검증하기.ErrorMessage);
        //}

        [Test]
        public void 검증버튼을눌렀을때파일경로가EmptyString이면False를반환()
        {
            var 검증하기 = new 검증하기LogicByFile(null, null);
            var ret = 검증하기.Has파일경로(string.Empty);

            Assert.IsFalse(ret);
        }

        [Test]
        public void 검증버튼을눌렀을때파일경로가있다면True를반환()
        {
            var 검증하기 = new 검증하기LogicByFile(null, null);
            var ret = 검증하기.Has파일경로(@"C:\");

            Assert.IsTrue(ret);
        }

        [Test]
        public void 검증버튼을눌렀을때파일경로에실제로파일이없으면False를반환()
        {
            var 검증하기 = new 검증하기LogicByFile(null, null);
            var ret = 검증하기.Is파일존재(@"c:\asdf23dd4324.png"); // 없는 파일

            Assert.IsFalse(ret);
        }

        [Test]
        public void IsValid_경로에실제로파일이없을때올바른메세지창나오나()
        {
            var 검증하기 = new 검증하기LogicByFile(@"c:\asdf23dd4324.png", null);
            검증하기.IsValid(); // 없는 파일

            Assert.AreEqual("입력하신 경로에 파일이 존재하지 않습니다.", 검증하기.ErrorMessage);
        }
    }
}
