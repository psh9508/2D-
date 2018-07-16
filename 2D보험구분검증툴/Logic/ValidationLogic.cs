using _2D보험구분검증툴.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Logic
{
    public class ValidationLogic
    {
        public static List<오류목록Model> GetErrorMessage(string[] splitedData)
        {
            List<오류목록Model> model = new List<오류목록Model>();

            int cnt = 1;
            var s = Get줄바꿈오류(splitedData); // 줄바꿈 문제를 가장 처음에 확인한다.

            if (s != string.Empty)
                model.Add(Get오류목록Model(cnt++, s));
            else // 줄바꿈 문제가 없을 때만 확인한다.
            {
                model.AddRange(Check구분자개수오류(ref cnt, splitedData));
                model.AddRange(Check약품명생략오류(ref cnt, splitedData));
            }

            return model;
        }


        public static List<오류목록Model> Check약품명생략오류(ref int cnt, string[] splitedData)
        {
            var sb = new StringBuilder();
            var model = ParseLogic.Parse(splitedData);
            var retv = new List<오류목록Model>();
            int RXDcnt = 1;

            //foreach (var item in model.RXDs)
            //{
            //    if (!string.IsNullOrEmpty(item.청구코드사용자코드))
            //    {
            //        if (!string.IsNullOrEmpty(item.약품명))
            //        {
            //            retv.Add(new 오류목록Model
            //            {
            //                No = cnt++,
            //                메세지 = $"{RXDcnt}번째 RXD헤더의 약품명이 생략되지 않았습니다. 청구코드가 있는 경우 약품명은 생략되어야합니다.",
            //            });
            //        }
            //    }

            //    RXDcnt++;
            //}

            return retv;
        }

        public static List<오류목록Model> Check구분자개수오류(ref int cnt, string[] splitedData)
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
                                retv.Add(new 오류목록Model { No = cnt++, 메세지 = $"{RXDcnt}번째 RXD의 구분자 개수가 다릅니다." });
                            else
                                retv.Add(new 오류목록Model { No = cnt++, 메세지 = $"{nowHeaderName}의 구분자 개수가 다릅니다." });
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

        public static string Get줄바꿈오류(string[] splitedData)
        {
            foreach (var item in splitedData)
            {
                if(item.Contains("\r") || item.Contains("\n"))
                    return @"[\r\n]로 이우러진 줄바꿈 문자가 없습니다.[\n]문자 대신 [\r\n]문자를 사용해주시기 바랍니다.";
            }

            return "";
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
