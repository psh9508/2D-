using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2D보험구분검증툴.Logic
{
    public class 파일선택Logic : I파일선택Button
    {
        private Action<string> _After파일선택;

        public void OpenFileDialog()
        {
            using (var ofd = new OpenFileDialog() { ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                    _After파일선택?.Invoke(ofd.FileName);
            }
        }

        public void SetAfter파일선택(Action<string> action)
        {
            _After파일선택 = action;
        }
    }
}
