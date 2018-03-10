using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class BooleanQuery<From> : GraphQuery<From,bool,BooleanQuery<From>>
    {
        internal BooleanQuery(IGraphQuery query) : base(query) { }
    }
}
