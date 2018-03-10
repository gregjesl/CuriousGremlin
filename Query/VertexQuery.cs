using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class VertexQuery<From> : ElementQuery<From,GraphVertex,VertexQuery<From>>
    {
        internal VertexQuery(IGraphQuery query) : base(query) { }

        public static VertexQuery Find(string label)
        {
            return Vertices().HasLabel(label);
        }

        public static VertexQuery Find(Dictionary<string, object> properties)
        {
            return Vertices().Has(properties);
        }

        public static VertexQuery Find(string label, Dictionary<string, object> properties)
        {
            return Find(label).Has(properties);
        }

        public EdgeQuery AddEdge(string label, string vertexID)
        {
            return AddEdge(label, Vertex(vertexID));
        }

        public EdgeQuery AddEdge(string label, string vertexID, Dictionary<string, object> properties)
        {
            return AddEdge(label, Vertex(vertexID), properties);
        }

        public EdgeQuery AddEdge(string label, VertexQuery vertices)
        {
            Query += ".addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")";
            return new EdgeQuery(Query);
        }

        public EdgeQuery AddEdge(string label, VertexQuery vertices, Dictionary<string, object> properties)
        {
            Query += ".addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")";
            return (new EdgeQuery(Query)).AddProperties(properties);
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
