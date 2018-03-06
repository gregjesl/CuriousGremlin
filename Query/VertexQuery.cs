using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class VertexQuery : ElementQuery<VertexQuery>
    {
        internal VertexQuery(string query) : base(query) { }

        public EdgeQuery AddEdge(string label, string to)
        {
            Query += ".addE('" + Sanitize(label) + "').to(g.V('" + Sanitize(to) + "'))";
            return new EdgeQuery(Query);
        }

        public VertexQuery Out()
        {
            Query += ".out()";
            return this;
        }

        public VertexQuery Out(string label)
        {
            Query += ".out('" + Sanitize(label) + "')";
            return this;
        }

        public VertexQuery In()
        {
            Query += ".in()";
            return this;
        }

        public VertexQuery In(string label)
        {
            Query += ".in('" + Sanitize(label) + "')";
            return this;
        }

        public VertexQuery Both()
        {
            Query += ".both()";
            return this;
        }

        public VertexQuery Both(string label)
        {
            Query += ".both('" + Sanitize(label) + "')";
            return this;
        }

        public EdgeQuery OutE()
        {
            Query += ".outE()";
            return new EdgeQuery(Query);
        }

        public EdgeQuery OutE(string label)
        {
            Query += ".outE('" + Sanitize(label) + "')";
            return new EdgeQuery(Query);
        }

        public EdgeQuery InE()
        {
            Query += ".inE()";
            return new EdgeQuery(Query);
        }

        public EdgeQuery InE(string label)
        {
            Query += ".inE('" + Sanitize(label) + "')";
            return new EdgeQuery(Query);
        }

        public EdgeQuery BothE()
        {
            Query += ".bothE()";
            return new EdgeQuery(Query);
        }

        public EdgeQuery BothE(string label)
        {
            Query += ".bothE('" + Sanitize(label) + "')";
            return new EdgeQuery(Query);
        }

        public VertexQuery AddListProperty(string key, string value)
        {
            Query += ".property(list, '" + Sanitize(key) + "', " + GetObjectString(value) + ")";
            return this;
        }
    }
}
