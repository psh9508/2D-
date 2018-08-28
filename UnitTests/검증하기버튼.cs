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

       

    }
}
