using _2D보험구분검증툴.Class;
using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _2D보험구분검증툴.Logic
{
    public static class ParseLogic
    {
        private const char _splitter = '¿';
        private const string _headerNamespace = "_2D보험구분검증툴.Class.";

        public static BarcodeModel Parse(string data)
        {
            var splitData = data.Replace("\r\n", _splitter.ToString()).Split(_splitter);

            return Parse(splitData);
        }

        public static BarcodeModel Parse(string[] splitData)
        {
            var parsedModel = new BarcodeModel();
            object header = null;
            int valueOrder = 0; // 값의 순서

            try
            {
                for(int i = 0; i < splitData.Count(); i++)
                {
                    if (IsHeader(splitData[i].Trim()))
                    {
                        valueOrder = 0;

                        SetModel(parsedModel, header);

                        var headerType = Type.GetType(_headerNamespace + splitData[i]);
                        header = Activator.CreateInstance(headerType);
                    }
                    else
                    {
                        if (header != null)
                            SetData(header, splitData[i], valueOrder++);
                        else
                            throw new MyLogicException("[ParseLogic-Parse] header가 없습니다.");
                    }
                }

                // 마지막 파씽된 값을 넣어준다.
                SetModel(parsedModel, header);

                return parsedModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void SetModel(BarcodeModel parsedModel, object header)
        {
            if (header != null)
            {
                if (header is MSH)
                    parsedModel.MSH = (MSH)header;
                else if (header is FAC)
                    parsedModel.FAC = (FAC)header;
                else if (header is PRD)
                    parsedModel.PRD = (PRD)header;
                else if (header is PID)
                    parsedModel.PID = (PID)header;
                else if (header is ORC)
                    parsedModel.ORC = (ORC)header;
                else if (header is DG1)
                    parsedModel.DG1 = (DG1)header;
                else if (header is IN1)
                    parsedModel.IN1 = (IN1)header;
                else 
                    parsedModel.RXDs.Add((RXD)header);
            }
        }

        public static bool IsHeader(string value)
        {
            var names = new string[]
            {
                "MSH", "FAC", "PRD", "PID", "ORC", "DG1", "IN1", "RXD"
            };

            //foreach (var name in names)
            //{
            //    if (value.Contains(name))
            //        return true;
            //}

            //return false;

            return names.Any(x => x == value);
        }

        private static void SetData(object header, string value, int valueOrder)
        {
            var props = Type.GetType(header.ToString()).GetProperties();

            if (GetMaxIndex(props) < valueOrder)
                return;

            props[valueOrder].SetValue(header, value.Trim(), BindingFlags.SetProperty, null, null, null);

            #region Order가지고 넣기
            //for (int i = 0; i < props.Count(); i++)
            //{
            //    var order = ((ValueAttribute)(props[i].GetCustomAttributes(true)[0])).Order;

            //    if (valueOrder == order) // 값의 순서를 비교해서 올바른 곳에 넣어줄 수 있도록 한다.
            //    {
            //        props[i].SetValue(header, value.Trim(), BindingFlags.SetProperty, null, null, null);
            //        return;
            //    }
            //}
            #endregion
        }

        public static IEnumerable<string> GetAllPropertiesValue(BarcodeModel data)
        {
            var headerProps = data.GetType().GetProperties();

            foreach (var headerProp in headerProps)
            {
                var header = headerProp.GetValue(data, null);
                var headerValues = header.GetType().GetProperties();

                // RXD는 따로 처리하자.
                if (headerProp.Name == "RXDs")
                {
                    break;
                    //    foreach (var RXD in header as IEnumerable<RXD>)
                    //    {
                    //        var RXDprops = RXD.GetType().GetProperties();

                    //        foreach (var RXDProp in RXDprops)
                    //        {
                    //            yield return RXDProp.GetValue(RXD, null) as string;
                    //        }
                    //    }
                }
                else
                {
                    foreach (var prop in headerValues)
                    {
                        yield return prop.GetValue(header, null) as string;
                    }
                }
            }
        }

        // 재귀를 이용해 범용적으로 쓸 수 있는 함수를 만들어보자.
        public static IEnumerable<string> Practics<T>(T data)
        {
            var myAssembly = Assembly.GetCallingAssembly().FullName;

            var properties = typeof(T).GetProperties();

            var test = GetVaule(data, properties);

            return null;
        }

        // 재귀를 이용해 범용적으로 쓸 수 있는 함수를 만들어보자.
        public static IEnumerable<string> GetVaule(object dataObj, PropertyInfo[] props)
        {
            var retv = new List<string>();
            var myAssembly = Assembly.GetCallingAssembly().FullName;

            foreach (var prop in props)
            {
                if (!Assembly.GetAssembly(prop.PropertyType).FullName.Equals(myAssembly))
                {
                    // 프로퍼티 중에 List, Enumerable, 등이 있다면 값을 가져와 다시 for문을 돌아야한다.
                    //if(prop.PropertyType == typeof(List<>))
                    //{
                    //    retv.AddRange(NewMethod(dataObj, prop));
                    //    continue;
                    //}

                    //yield return prop.GetValue(dataObj, null) as string;
                    retv.Add(prop.GetValue(dataObj, null) as string);
                    continue;
                }

                retv.AddRange(GetPropertiesValues(dataObj, prop));
            }

            return retv;
        }

        private static IEnumerable<string> GetPropertiesValues(object dataObj, PropertyInfo prop)
        {
            var value = prop.GetValue(dataObj, null);
            var valueProps = value.GetType().GetProperties();

            return GetVaule(value, valueProps);
        }

        private static int GetMaxIndex<T>(T[] itmes)
        {
            return itmes.Count() - 1;
        }
    }
}

