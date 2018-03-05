using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class EdgeQuery : GraphQuery
    {
        internal EdgeQuery(string query) : base(query) { }

        public VertexQuery OutV()
        {
            throw new NotImplementedException();
        }

        public VertexQuery InV()
        {
            throw new NotImplementedException();
        }

        public VertexQuery BothV()
        {
            throw new NotImplementedException();
        }

        public VertexQuery OtherV()
        {
            throw new NotImplementedException();
        }
    }
}
