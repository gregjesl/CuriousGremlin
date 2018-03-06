using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class EdgeQuery : ElementQuery<EdgeQuery>
    {
        internal EdgeQuery(string query) : base(query) { }

        public VertexQuery OutV()
        {
            return new VertexQuery(Query + ".outV()");
        }

        public VertexQuery InV()
        {
            return new VertexQuery(Query + ".inV()");
        }

        public VertexQuery BothV()
        {
            return new VertexQuery(Query + ".bothV()");
        }

        public VertexQuery OtherV()
        {
            return new VertexQuery(Query + ".otherV()");
        }
    }
}
