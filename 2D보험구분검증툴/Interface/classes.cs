using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Interface
{
    public class Messages : IMessage
    {
        public string GetMessage가능한버전이아님()
        {
            return "이용가능한 버전의 QR코드가 아닙니다.";
        }

        public string GetMessage파일경로비어있음()
        {
            return "파일경로가 비어있습니다. 파일을 먼저 선택해주시기 바랍니다.";
        }

        public string GetMessage파일이QR코드가아님()
        {
            return "입력하신 경로에 파일이 QR코드가 아닙니다.";
        }

        public string GetMessage파일존재하지않음()
        {
            return "입력하신 경로에 파일이 존재하지 않습니다.";
        }
    }
}
