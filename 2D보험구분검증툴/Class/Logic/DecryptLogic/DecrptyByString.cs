using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class.Logic.DecryptLogic
{
    public class DecrptyByString : IDecryptable
    {
        private readonly I외부모듈 _외부모듈;
        public string EncrytedData { get; set; }

        public DecrptyByString(string data, I외부모듈 외부모듈)
        {
            EncrytedData = data;
            _외부모듈 = 외부모듈;
        }

        public string GetDecryptedString()
        {
            StringBuilder decodeData = new StringBuilder(2048);

            int result = _외부모듈.CallUB2DDecode(EncrytedData, decodeData);

            return result == 1 ? decodeData.ToString() : null;
        }
    }
}
