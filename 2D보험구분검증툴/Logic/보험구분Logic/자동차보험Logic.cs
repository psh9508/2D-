using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    public class 자동차보험Logic : I보험구분검증
    {
        public IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt)
        {
            return new List<오류목록Model> { new 오류목록Model { No = ++cnt, 유형 = "보험구분", 메세지 = "자동차보험 바코드는 보험구분이 4, 공상및보훈구분에 값이 없어야합니다." } };
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "4" && model.IN1.공상및보훈구분.Trim() == "";
        }
    }
}
