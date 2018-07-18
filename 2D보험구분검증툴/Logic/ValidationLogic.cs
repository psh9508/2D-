using _2D보험구분검증툴.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    returnModel.AddRange(Check약품명생략오류(splitedData, parsedModel));
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

        public static List<오류목록Model> Check약품명생략오류(string[] splitedData, BarcodeModel model)
        {
            var sb = new StringBuilder();
            var retv = new List<오류목록Model>();
            int RXDcnt = 1;

            foreach (var item in model.RXDs)
            {
                if (!string.IsNullOrEmpty(item.청구코드사용자코드))
                {
                    if (!string.IsNullOrEmpty(item.약품명))
                    {
                        retv.Add(new 오류목록Model
                        {
                            No = _errorCnt++,
                            메세지 = $"{RXDcnt}번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.",
                        });
                    }
                }

                RXDcnt++;
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
    }
}
