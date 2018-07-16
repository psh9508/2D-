using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    public class 차상위2Logic : I보험구분검증
    {
        public IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt)
        {
            return new List<오류목록Model>() { new 오류목록Model
            {
                No = ++cnt,
                유형 = "보험구분",
                메세지 = "차상위 보험은 보험구분이 1이고 공상및보훈구분은 17(차상위 2종(E)) 이어야 합니다.",
            } };
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "1" && (model.IN1.공상및보훈구분 == "17");
        }
    }
}
