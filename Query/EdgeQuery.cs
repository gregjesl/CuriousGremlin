using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class EdgeQuery<From, Query> : ElementQuery<GraphEdge, From, Query>
        where Query: EdgeQuery<From, Query>
    {
        protected EdgeQuery(ITraversalQuery<From> query) : base(query) { }

        protected EdgeQuery() : base() { }

        public VertexQuery<From> OutV()
        {
            Steps.Add("outV()");
            return new VertexQuery<From>(this);
        }

        public VertexQuery<From> InV()
        {
            Steps.Add("inV()");
            return new VertexQuery<From>(this);
        }

        public VertexQuery<From> BothV()
        {
            Steps.Add("bothV()");
            return new VertexQuery<From>(this);
        }

        public VertexQuery<From> OtherV()
        {
            Steps.Add("otherV()");
            return new VertexQuery<From>(this);
        }
    }

    public class EdgeQuery<From> : EdgeQuery<From, EdgeQuery<From>>
    {
        internal EdgeQuery(ITraversalQuery<From> query) : base(query) { }

        internal EdgeQuery() : base() { }
    }
}
