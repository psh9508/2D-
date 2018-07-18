using _2D보험구분검증툴.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2D보험구분검증툴.Logic
{
    public class ValidationLogic
    {
        private static int _errorCnt = 1;

        public static List<오류목록Model> GetErrorMessage(string[] splitedData)
        {
            _errorCnt = 0;

            var parsedModel = ParseLogic.Parse(splitedData);
            var returnModel = new List<오류목록Model>();

            returnModel.AddRange(Get누락헤더여부(parsedModel));

            if(returnModel.Count <= 0) // 헤더 누락이 없을 때만 확인한다.
            {
                var data = Get줄바꿈오류(splitedData);

                if(data != null)
                    returnModel.Add(data);

                if(returnModel.Count <= 0) // 줄바꿈 문제가 없을 때만 확인한다.
                {
                    returnModel.AddRange(Check구분자개수오류(splitedData));
                    returnModel.AddRange(CheckBasicDataValidation(parsedModel));
                }
            }

            return returnModel;
        }

        private static List<오류목록Model> Get누락헤더여부(BarcodeModel model)
        {
            var retv = new List<오류목록Model>();

            if (model == null)
                return retv;

            var allHeaderProps = model.GetType().GetProperties();

            foreach (var headerProp in allHeaderProps)
            {
                var headerValue = headerProp.GetValue(model, null);

                if(headerValue == null)
                {
                    retv.Add(new 오류목록Model
                    {
                        No = _errorCnt++,
                        유형 = "헤더 누락",
                        메세지 = $"[{headerProp.Name}] 헤더가 누락 되었습니다.",
                    });
                }
                else if (headerValue is List<RXD>)
                {
                    var RXDs = headerValue as List<RXD>;

                    if(RXDs.Count <= 0)
                    {
                        retv.Add(new 오류목록Model
                        {
                            No = _errorCnt++,
                            유형 = "헤더 누락",
                            메세지 = $"[RXD] 헤더가 누락 되었습니다.",
                        });
                    }
                }
            }

            return retv;
        }

        public static List<오류목록Model> CheckBasicDataValidation(BarcodeModel model)
        {
            var sb = new StringBuilder();
            var retv = new List<오류목록Model>();

            //환자명에 공백 있는 경우 검출.
            if (model.PID.수진자성명.Contains(" "))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "환자명에 공백이 포함되어 있습니다. 공백 제거 후 입력 바랍니다." });

            //의사명에 공백 있는 경우 검출.
            if (model.PRD.처방의료인성명.Contains(" "))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "처방의사명에 공백이 포함되어 있습니다. 공백 제거 후 입력 바랍니다." });

            //교부번호 날짜타입 확인.   
            if (model.ORC.교부번호.Trim().Length < 8 || !CheckTypeOfDate(model.ORC.교부번호.Trim().Substring(0, 8)))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "교부번호 앞 8 자리가 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다." });

            //보험구분 값 범위 초과.
            string[] typeList = { "1", "2", "3", "4", "5" };
            if (!CheckScope(model.IN1.보험구분.Trim(), typeList))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "보험구분 값이 유효하지 않습니다. 1~5 범위 내에서 입력 바랍니다." });

            //공상 및 보훈 구분 값 범위 초과.
            string[] subTypeList = { "1", "4", "7", "15", "16", "17", "18", "" };
            if (!CheckScope(model.IN1.공상및보훈구분.Trim(), subTypeList))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "공상 및 보훈구분 값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다." });

            //의료급여 종별 값 범위 초과.
            string[] specialTypeList = { "1", "2", "" };
            if (!CheckScope(model.IN1.의료급여종별.Trim(), specialTypeList))
                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "의료급여종별 값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다." });

            if(model.IN1.보험구분.Trim() == "3")
                if (!CheckTypeOfDate(model.ORC.재해발생일.Trim()))
                    retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = "재해발생일 값이 날짜 타입이 아닙니다. YYYYMMDD 형식으로 입력 바랍니다." });

            for(int i = 0; i < model.RXDs.Count; i++)
            {
                if (!string.IsNullOrEmpty(model.RXDs[i].청구코드사용자코드))
                {
                    if (!string.IsNullOrEmpty(model.RXDs[i].약품명))
                    {
                        retv.Add(new 오류목록Model
                        {
                            No = _errorCnt++,
                            메세지 = $"{i + 1}번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.",
                        });
                    }
                }

                //처방구분 값 범위 확인.
                string[] drugTypeList = { "1", "2", "3" };
                if (!CheckScope(model.RXDs[i].처방구분, drugTypeList))
                    retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = $"{i + 1}번째 약품의 처방구분값이 유효하지 않습니다.API문서의 데이터 범위를 다시한번 확인 바랍니다." });

                //급여구분 값 범위 확인.
                string[] insuranceTypeList = { "1", "2", "3", "4", "5" };
                if (!CheckScope(model.RXDs[i].급여구분, insuranceTypeList))
                    retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = $"{i + 1}번째 약품의 급여구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다." });

                //코드구분 값 범위 확인.
                string[] codeTypeList = { "1", "2" };
                if (!CheckScope(model.RXDs[i].코드구분, codeTypeList))
                    retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = $"{i + 1}번째 약품의 코드구분값이 유효하지 않습니다. API문서의 데이터 범위를 다시한번 확인 바랍니다." });
            }

            return retv;
        }

        public static List<오류목록Model> Check구분자개수오류(string[] splitedData)
        {
            int nowSpliterNum = 0;
            int RXDcnt = 0;
            string nowHeaderName = "MSH";
            var retv = new List<오류목록Model>();

            for (int i = 0;  i < splitedData.Count(); i++)
            {
                if (!ParseLogic.IsHeader(splitedData[i]))
                    nowSpliterNum++;

                if (IsLastElement(splitedData, i) || ParseLogic.IsHeader(splitedData[i]))
                {
                    if (splitedData[i] != "MSH")
                    {
                        if (nowHeaderName == "RXD")
                            RXDcnt++;

                        if (nowSpliterNum != Get구분자개수(nowHeaderName))
                        {
                            if (nowHeaderName == "RXD")
                                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = $"{RXDcnt}번째 RXD의 구분자 개수가 다릅니다." });
                            else
                                retv.Add(new 오류목록Model { No = _errorCnt++, 메세지 = $"{nowHeaderName}의 구분자 개수가 다릅니다." });
                        }
                    }

                    nowHeaderName = splitedData[i];
                    nowSpliterNum = 0;
                }
            }

            return retv;
        }

        private static bool IsLastElement(string[] splitedData, int i)
        {
            return i == splitedData.Count() - 1;
        }

        public static 오류목록Model Get줄바꿈오류(string[] splitedData)
        {
            오류목록Model retv = null;

            foreach (var item in splitedData)
            {
                if (item.Contains("\r") || item.Contains("\n"))
                {
                    retv = new 오류목록Model
                    {
                        No = _errorCnt++,
                        유형 = "줄바꿈 문자 에러",
                        메세지 = @"[\r\n]로 이우러진 줄바꿈 문자가 없습니다.[\n]문자 대신 [\r\n]문자를 사용해주시기 바랍니다.",
                    };

                    break;
                }
            }

            return retv;
        }

        private static int Get구분자개수(string headerName)
        {
            var type = Type.GetType("_2D보험구분검증툴.Class." + headerName);

            return type == null ? 0 : type.GetProperties().Count();
        }

        private static 오류목록Model Get오류목록Model(int cnt, string msg)
        {
            return new 오류목록Model()
            {
                No = cnt,
                메세지 = msg,
            };
        }

        private static bool CheckTypeOfDate(string input)
        {
            return Regex.IsMatch(input, @"^(19|20)\d{2}(0[1-9]|1[012])(0[1-9]|[12][0-9]|3[0-1])$");
        }

        private static bool CheckScope(string target, string[] bounds)
        {
            bool corrected = false;

            foreach (var item in bounds)
            {
                if (item.Equals(target))
                    corrected = true;
            }

            return corrected;
        }
    }
}
