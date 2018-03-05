using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class VertexQuery : GraphQuery
    {
        internal VertexQuery(string query) : base(query) { }

        public EdgeQuery AddEdge(string label, string to)
        {
            Query += ".addE('" + Sanitize(label) + "').to(g.V('" + Sanitize(to) + "'))";
            return new EdgeQuery(Query);
        }

        public VertexQuery Out()
        {
            throw new NotImplementedException();
        }

        public VertexQuery Out(string label)
        {
            throw new NotImplementedException();
        }

        public VertexQuery In()
        {
            throw new NotImplementedException();
        }

        public VertexQuery In(string label)
        {
            throw new NotImplementedException();
        }

        public VertexQuery Both()
        {
            throw new NotImplementedException();
        }

        public VertexQuery Both(string label)
        {
            throw new NotImplementedException();
        }

        public EdgeQuery OutE()
        {
            throw new NotImplementedException();
        }

        public EdgeQuery OutE(string label)
        {
            throw new NotImplementedException();
        }

        public EdgeQuery InE()
        {
            throw new NotImplementedException();
        }

        public EdgeQuery InE(string label)
        {
            throw new NotImplementedException();
        }

        public EdgeQuery BothE()
        {
            throw new NotImplementedException();
        }

        public EdgeQuery BothE(string label)
        {
            throw new NotImplementedException();
        }
    }
}
