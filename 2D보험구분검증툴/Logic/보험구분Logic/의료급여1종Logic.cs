using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    class 의료급여1종Logic : I보험구분검증
    {
        public 오류목록Model GetErrorModel(int cnt)
        {
            throw new NotImplementedException();
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "2" && model.IN1.의료급여종별 == "1";
        }
    }
}
