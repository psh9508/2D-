using _2D보험구분검증툴;
using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using _2D보험구분검증툴.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TEST
{
    [TestClass()]
    class 파일선택버튼
    {
        Form1 frm = null;
        TextBox txt파일경로 = null;
        IMessage _message;
        I파일선택Button _logic;

        [TestInitialize]
        public void SetUp()
        {
            frm = new Form1();
            txt파일경로 = frm.Get파일경로;

            _message = new Messages();
            _logic = new 파일선택Logic();
        }

        [TestMethod()]
        public void 파일선택창을눌러서파일을선택하면Groupbox가꺼져야한다()
        {
            var logic = new Mock파일선택();

            logic.After파일선택 += (imagePath) => {
                logic.is그룹박스닫힘 = true;
                logic.경로명 = imagePath;
            };

            logic.OpenFileDialog();

            Assert.IsTrue(logic.is그룹박스닫힘);
        }
    }
}
