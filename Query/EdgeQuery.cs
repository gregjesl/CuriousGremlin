using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class EdgeQuery<From> : ElementQuery<From,GraphEdge,EdgeQuery<From>>
    {
        internal EdgeQuery(ITraversalQuery<From, IGraphOutput> query) : base(query) { }

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
}
