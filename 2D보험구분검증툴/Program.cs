using _2D보험구분검증툴.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _2D보험구분검증툴
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(new 검증하기Logic(new 외부모듈()), new 인증Logic(new 외부모듈()), new FormLogic()));
        }
    }
}
