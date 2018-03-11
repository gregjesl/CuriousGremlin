using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class VertexQuery<From> : ElementQuery<From, GraphVertex, VertexQuery<From>>
        where From: IGraphObject
    {
        internal VertexQuery(ITraversalQuery query) : base(query) { }

        internal VertexQuery() : base() { }

        public static VertexQuery<From> Find(string label)
        {
            return (Vertices() as VertexQuery<From>).HasLabel(label);
        }

        public static VertexQuery<From> Find(Dictionary<string, object> properties)
        {
            return (Vertices() as VertexQuery<From>).Has(properties);
        }

        public static VertexQuery<From> Find(string label, Dictionary<string, object> properties)
        {
            return Find(label).Has(properties);
        }

        /*
        public EdgeQuery<From> AddEdge(string label, string vertexID)
        {
            return AddEdge(label, VertexQuery<From>(vertexID));
        }

        public EdgeQuery<From> AddEdge(string label, string vertexID, Dictionary<string, object> properties)
        {
            return AddEdge(label, Vertex(vertexID), properties);
        }
        */

        public EdgeQuery<From> AddEdge(string label, VertexQuery<From> vertices)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> AddEdge(string label, VertexQuery<From> vertices, Dictionary<string, object> properties)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")");
            return (new EdgeQuery<From>(this)).AddProperties(properties);
        }

        public VertexQuery<From> Out()
        {
            Steps.Add("out()");
            return this;
        }

        public VertexQuery<From> Out(string label)
        {
            Steps.Add("out('" + Sanitize(label) + "')");
            return this;
        }

        public VertexQuery<From> In()
        {
            Steps.Add("in()");
            return this;
        }

        public VertexQuery<From> In(string label)
        {
            Steps.Add("in('" + Sanitize(label) + "')");
            return this;
        }

        public VertexQuery<From> Both()
        {
            Steps.Add("both()");
            return this;
        }

        public VertexQuery<From> Both(string label)
        {
            Steps.Add("both('" + Sanitize(label) + "')");
            return this;
        }

        public EdgeQuery<From> OutE()
        {
            Steps.Add("outE()");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> OutE(string label)
        {
            Steps.Add("outE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> InE()
        {
            Steps.Add("inE()");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> InE(string label)
        {
            Steps.Add("inE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> BothE()
        {
            Steps.Add("bothE()");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> BothE(string label)
        {
            Steps.Add("bothE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        public VertexQuery<From> AddListProperty(string key, string value)
        {
            Steps.Add(".property(list, '" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this;
        }

        public VertexQuery<GraphVertex> CreateSubQuery()
        {
            return new VertexQuery<GraphVertex>();
        }
    }
}
