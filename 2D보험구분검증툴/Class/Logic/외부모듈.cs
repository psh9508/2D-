using _2D보험구분검증툴.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace _2D보험구분검증툴.Logic
{
    public class 외부모듈 : I외부모듈
    {
        [DllImport("C:\\Program Files\\Upharm2D\\UB2DDecoder.dll", CharSet = CharSet.Ansi)]
        public static extern int UBSetPharmInfo(string strPharmNum, string strPrdCode, StringBuilder iDay);
        [DllImport("C:\\Program Files\\Upharm2D\\UB2DDecoder.dll", CharSet = CharSet.Ansi)]
        public static extern int UB2DDecode(string data, StringBuilder decrypt);

        public int CallUBSetPharmInfo(string strPharmNum, string strPrdCode, StringBuilder iDay)
        {
            try
            {
                return UBSetPharmInfo(strPharmNum, strPrdCode, iDay);
            }
            catch (DllNotFoundException ex)
            {
                throw new DllNotFoundException(ex.Message);
            }
        }

        public int CallUB2DDecode(string data, StringBuilder decrypt)
        {
            try
            {
                return UB2DDecode(data, decrypt);
            }
            catch (DllNotFoundException ex)
            {
                throw new DllNotFoundException(ex.Message);
            }
        }
    }
}
