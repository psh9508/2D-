using _2D보험구분검증툴.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Interface;

namespace _2D보험구분검증툴.Class.Logic.QRCode
{
    public class 검증하기LogicByString : Base검증하기Logic
    {
        public 검증하기LogicByString(string encrpytedData, I외부모듈 외부모듈) : base(외부모듈)
        {
            _encryptedData = encrpytedData;
        }

        public override string GetDecryptedString()
        {
            StringBuilder decodeData = new StringBuilder(2048);

            int result = _외부모듈.CallUB2DDecode(_encryptedData, decodeData);

            return result == 1 ? decodeData.ToString() : null;
        }

        protected override void IsValidByLogic()
        {
            if (string.IsNullOrWhiteSpace(_encryptedData))
                ErrorMessage = "직접입력 창에 값이 없습니다. 바코드를 읽었을 때 나오는 암호화된 문자열을 입력해주세요.";
        }

        public override string GetEncrytedString()
        {
            return _encryptedData;
        }
    }
}
