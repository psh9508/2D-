using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2D보험구분검증툴.Class
{
    internal sealed class ValueAttribute : Attribute
    {
        public int Order { get; set; } = 0;

        public ValueAttribute(int order)
        {
            Order = order;
        }
    }

    internal sealed class HeaderAttribute : Attribute
    {
        public int Order { get; set; }

        public HeaderAttribute(int order)
        {
            Order = order;
        }
    }
}
