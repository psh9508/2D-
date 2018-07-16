using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2D보험구분검증툴.Class;

namespace _2D보험구분검증툴.Logic.보험구분Logic
{
    class 산재Logic : I보험구분검증
    {
        public IEnumerable<오류목록Model> GetErrorModel(BarcodeModel model, int cnt)
        {
            var retv = new List<오류목록Model>();

            if(!(model.IN1.보험구분 == "3" && model.IN1.공상및보훈구분 == ""))
                retv.Add(new 오류목록Model() { No = ++cnt, 유형 = "보험구분", 메세지 = "산재 바코드의 보험구분은 3이고 공상및보훈구분에 값이 없어야합니다." });
            else if(model.IN1.증번호.Trim() == "")
                retv.Add(new 오류목록Model() { No = ++cnt, 유형 = "보험구분", 메세지 = "산재 바코드는 증번호란에 관리번호가 필수로 들어가야 합니다." });

            return retv;
        }

        public bool Validation(BarcodeModel model)
        {
            return model.IN1.보험구분 == "3" && model.IN1.공상및보훈구분 == "" && model.IN1.증번호.Trim() != "";
        }
    }
}
