using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class BooleanQuery<From> : TraversalQuery<From,GraphBoolean>
        where From: IGraphObject
    {
        internal BooleanQuery(ITraversalQuery query) : base(query) { }
    }

    public class BooleanQuery : BooleanQuery<Graph>
    {
        internal BooleanQuery(ITraversalQuery query) : base(query) { }
    }
}
