using _2D보험구분검증툴.Interface;
using _2D보험구분검증툴.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class.Logic.DecryptLogic
{
    public class DecryptByFile : IDecryptable
    {
        private readonly I외부모듈 _외부모듈;
        private readonly I검증하기 _검증하기;
        public string FilePath { get; set; }

        public DecryptByFile(string filePath, I외부모듈 외부모듈, I검증하기 검증하기)
        {
            FilePath = filePath;
            _외부모듈 = 외부모듈;
            _검증하기 = 검증하기;
        }

        public string GetDecryptedString()
        {
            var data = _검증하기.Get바코드Data(FilePath);

            if (string.IsNullOrEmpty(data))
                throw new MyLogicException("바코드 이미지에서 데이터를 가져오지 못함.");

            StringBuilder decodeData = new StringBuilder(2048);

            int result = _외부모듈.CallUB2DDecode(data, decodeData);

            return result == 1 ? decodeData.ToString() : null;
        }
    }
}
