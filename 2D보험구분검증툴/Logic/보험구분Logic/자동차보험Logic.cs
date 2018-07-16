using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    class 자동차보험Logic : I보험구분검증
    {
        public 오류목록Model GetErrorModel(int cnt)
        {
            throw new NotImplementedException();
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "4" && model.IN1.공상및보훈구분.Trim() == "";
        }
    }
}
