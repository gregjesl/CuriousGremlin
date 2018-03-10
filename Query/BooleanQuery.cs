using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class BooleanQuery<From> : TraversalQuery<From,bool,BooleanQuery<From>>
    {
        internal BooleanQuery(ITraversalQuery query) : base(query) { }
    }
}
