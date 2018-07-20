using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class
{
    public static class MessageHelper
    {
        public static bool IsTest;

        public static void ShowMessageBox(string msg)
        {
            if(!IsTest)
                System.Windows.Forms.MessageBox.Show(msg);
        }
    }
}
