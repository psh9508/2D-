using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _2D보험구분검증툴.Logic
{
    public class FormLogic : IForm
    {
        private Action<string> _After파일선택;
        private const string PATH = @"C:\보험구분검증결과\";

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

        public void SaveResult(string insuranceName, string data)
        {
            var fullPath = PATH + insuranceName + ".txt";

            if (!Directory.Exists(PATH))
                Directory.CreateDirectory(PATH);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            File.WriteAllText(fullPath, data);
        }

        public void Show인증하기Button()
        {
            throw new NotImplementedException();
        }
    }
}
