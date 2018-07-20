using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class
{
    public class BarcodeModel
    {
        public MSH MSH { get; set; }
        public FAC FAC { get; set; }
        public PRD PRD { get; set; }
        public PID PID { get; set; }
        public ORC ORC { get; set; }
        public DG1 DG1 { get; set; }
        public IN1 IN1 { get; set; }
        public List<RXD> RXDs { get; set; } = new List<RXD>();
    }

    public class HHB
    {
        public string MyProperty { get; set; }
    }

    [Header(0)]
    public class MSH : IHeader<MSH>
    {
        public MSH GetHeaderModel()
        {
            return new MSH();
        }
        
        [Value(0)]
        public string 버전정보 { get; set; }
        [Value(1)]
        public string 병원전산업체코드 { get; set; }
        [Value(2)]
        public string 제품정보 { get; set; }
        [Value(3)]
        public string 생성날짜시간 { get; set; }
    }

    public class FAC : IHeader<FAC>
    {
        public FAC GetHeaderModel()
        {
            return new FAC();
        }

        [Value(0)]
        public string 처방요양기관정보 { get; set; }
        [Value(1)]
        public string 의료기관명칭 { get; set; }
        [Value(2)]
        public string 요양기관주소 { get; set; }
        [Value(3)]
        public string 의료기관전화번호 { get; set; }
        [Value(4)]
        public string 의료기관팩스번호 { get; set; }
        [Value(5)]
        public string 의료기관이메일 { get; set; }
    }

    public class PRD : IHeader<PRD>
    {
        public PRD GetHeaderModel()
        {
            return new PRD();
        }

        [Value(0)]
        public string 처방의료인성명 { get; set; }
        [Value(1)]
        public string 면허종별 { get; set; }
        [Value(2)]
        public string 면허번호 { get; set; }
    }

    public class PID : IHeader<PID>
    {
        public PID GetHeaderModel()
        {
            return new PID();
        }

        [Value(0)]
        public string 수진자성명 { get; set; }
        [Value(1)]
        public string 수진자주민등록번호 { get; set; }
    }

    public class ORC : IHeader<ORC>
    {
        public ORC GetHeaderModel()
        {
            return new ORC();
        }

        [Value(0)]
        public string 교부번호 { get; set; }
        [Value(1)]
        public string 재해발생일 { get; set; }
        [Value(2)]
        public string 진료과목 { get; set; }
        [Value(3)]
        public string 특정기호 { get; set; }
        [Value(4)]
        public string 사용기간 { get; set; }
        [Value(5)]
        public string 용법 { get; set; }
        [Value(6)]
        public string 조제시참고사항 { get; set; }
    }

    public class DG1 : IHeader<DG1>
    {
        public DG1 GetHeaderModel()
        {
            return new DG1();
        }

        [Value(0)]
        public string 상병분류기호1 { get; set; }
        [Value(1)]
        public string 상병분류기호2 { get; set; }
    }

    public class IN1 : IHeader<IN1>
    {
        public IN1 GetHeaderModel()
        {
            return new IN1();
        }

        [Value(0)]
        public string 보험구분 { get; set; }
        [Value(1)]
        public string 공상및보훈구분 { get; set; }
        [Value(2)]
        public string 의료급여종별 { get; set; }
        [Value(3)]
        public string 증번호 { get; set; }
        [Value(4)]
        public string 조합기호산개기관기호 { get; set; }
        [Value(5)]
        public string 조합명칭사업자명칭 { get; set; }
        [Value(6)]
        public string 보훈번호 { get; set; }
        [Value(7)]
        public string 미사용1 { get; set; }
        [Value(8)]
        public string 미사용2 { get; set; }
        [Value(9)]
        public string 미사용3 { get; set; }
        [Value(10)]
        public string 미사용4 { get; set; }
        [Value(11)]
        public string 미사용5 { get; set; }
        [Value(12)]
        public string 미사용6 { get; set; }
    }

    public class RXD : IHeader<RXD>
    {
        public RXD GetHeaderModel()
        {
            return new RXD();
        }

        [Value(0)]
        public string 처방구분 { get; set; }
        [Value(1)]
        public string 급여구분 { get; set; }
        [Value(2)]
        public string 코드구분 { get; set; }
        [Value(3)]
        public string 청구코드사용자코드 { get; set; }
        [Value(4)]
        public string 약품명 { get; set; }
        [Value(5)]
        public string 일회투약량 { get; set; }
        [Value(6)]
        public string 일회투여횟수 { get; set; }
        [Value(7)]
        public string 총투약일수 { get; set; }
        [Value(8)]
        public string 용법코드 { get; set; }
        [Value(9)]
        public string 용법 { get; set; }
    }

    public class 오류목록Model
    {
        public int No { get; set; }
        public string 유형 { get; set; }
        public string 메세지 { get; set; }
    }
}
