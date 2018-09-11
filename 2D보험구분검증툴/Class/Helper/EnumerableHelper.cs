using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class.Helper
{
    public static class EnumerableHelper
    {
        public static bool None<TSource>(this IEnumerable<TSource> source,
                                    Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}
