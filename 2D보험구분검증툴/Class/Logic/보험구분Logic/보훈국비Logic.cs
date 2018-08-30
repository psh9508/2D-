using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class.Logic.보험구분Logic
{
    class 보훈국비Logic : I보험구분검증
    {
        public string Name { get { return "보훈국비"; } }

        public IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt)
        {
            var retv = new List<오류목록Model>();

            if (!(model.IN1.보험구분 == "1" && model.IN1.공상및보훈구분 == "7"))
                retv.Add(new 오류목록Model() { No = ++cnt, 유형 = "보험구분", 메세지 = "보훈국비 바코드의 보험구분은 1이고 공상및보훈구분에 7값이 들어가야 합니다." });

            return retv;
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "1" && model.IN1.공상및보훈구분 == "7";
        }
    }
}
