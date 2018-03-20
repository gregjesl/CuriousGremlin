using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query.CRTP
{
    public class EdgeQueryTemplate<From, Query> : ElementQueryTemplate<GraphEdge, From, Query>
        where Query : EdgeQueryTemplate<From, Query>
    {
        protected EdgeQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected EdgeQueryTemplate() : base() { }

        /// <summary>
        /// Returns the vertex in the outgoing direction of the edge
        /// </summary>
        public VertexQuery<From> OutV()
        {
            Steps.Add("outV()");
            return new VertexQuery<From>(this);
        }

        /// <summary>
        /// Returns the vertex in the incoming direction of the edge
        /// </summary>
        public VertexQuery<From> InV()
        {
            Steps.Add("inV()");
            return new VertexQuery<From>(this);
        }

        /// <summary>
        /// Returns both vertices connected to the edge
        /// </summary>
        public VertexQuery<From> BothV()
        {
            Steps.Add("bothV()");
            return new VertexQuery<From>(this);
        }

        /// <summary>
        /// Returns the vertex that was not traversed from
        /// </summary>
        public VertexQuery<From> OtherV()
        {
            Steps.Add("otherV()");
            return new VertexQuery<From>(this);
        }
    }
}
