using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Helper
{
    public static class ObjectHelper
    {
        public static T IfNull<T>(this T src, T defaultValue)
        {
            return ReferenceEquals(src, null) ? defaultValue : src;
        }
        
    }
}
