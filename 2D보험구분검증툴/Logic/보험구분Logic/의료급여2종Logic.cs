using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    class 의료급여2종Logic : I보험구분검증
    {
        public IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt)
        {
            return new List<오류목록Model> { new 오류목록Model { No = ++cnt, 유형 = "보험구분", 메세지 = "의료급여 1종 바코드는 보험구분이 2이고 의료급여종별은 2이여야 합니다." } };
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "2" && model.IN1.의료급여종별 == "2";
        }
    }
}
