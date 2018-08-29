using _2D보험구분검증툴;
using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
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
    public class formlogictests
    {
        [SetUp]
        public void setup()
        {
            MessageHelper.IsTest = true;
        }

        [TearDown]
        public void teardown()
        {
            MessageHelper.IsTest = false;
        }

        [Test]
        public void 인증실패하면인증재시도버튼떠야한다()
        {
            var mock인증하기 = new Mock<I인증하기>();
            var mockformlogic = Mock.Of<IForm>();

            mock인증하기.Setup(x => x.UB2DCheckAuthProcess(It.IsAny<string>()))
                .Returns(false);

            var form = new Form1(mock인증하기.Object, mockformlogic);

            form.인증시도("x");

            Mock.Get(mockformlogic).Verify(x => x.Show인증하기Button(It.IsAny<Button>()), Times.Once);
        }
    }
}
