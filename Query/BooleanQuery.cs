using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class BooleanQuery<From> : TraversalQuery<From,bool,BooleanQuery<From>>
    {
        internal BooleanQuery(ITraversalQuery<From,IGraphOutput> query) : base(query) { }
    }
}
