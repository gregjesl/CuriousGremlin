using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query
{
    public class VertexQuery<From, Query> : ElementQuery<From, GraphVertex, Query>
        where From: IGraphObject
        where Query: VertexQuery<From, Query>
    {
        internal VertexQuery(ITraversalQuery query) : base(query) { }

        internal VertexQuery() : base() { }

        public EdgeQuery<From> AddEdge(string label, string vertexID)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(g.V('" + Sanitize(vertexID) + "'))");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> AddEdge(string label, string vertexID, Dictionary<string, object> properties)
        {
            return AddEdge(label, vertexID).AddProperties(properties);
        }

        public EdgeQuery<From> AddEdge(string label, VertexQuery vertices)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")");
            return new EdgeQuery<From>(this);
        }

        public EdgeQuery<From> AddEdge(string label, VertexQuery vertices, Dictionary<string, object> properties)
        {
            return AddEdge(label, vertices).AddProperties(properties);
        }

        public Query Out()
        {
            Steps.Add("out()");
            return this as Query;
        }

        public Query Out(string label)
        {
            Steps.Add("out('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query In()
        {
            Steps.Add("in()");
            return this as Query;
        }

        public Query In(string label)
        {
            Steps.Add("in('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query Both()
        {
            Steps.Add("both()");
            return this as Query;
        }

        public Query Both(string label)
        {
            Steps.Add("both('" + Sanitize(label) + "')");
            return this as Query;
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

        public Query AddListProperty(string key, string value)
        {
            Steps.Add(".property(list, '" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        public VertexQuery<GraphVertex> CreateSubQuery()
        {
            return new VertexQuery<GraphVertex>();
        }
    }

    public class VertexQuery<From> : VertexQuery<From, VertexQuery<From>>
        where From: IGraphObject
    {
        internal VertexQuery(ITraversalQuery query) : base(query) { }

        internal VertexQuery() : base() { }
    }

    public class VertexQuery : VertexQuery<Graph, VertexQuery>
    {
        private VertexQuery() : base() { }

        public static VertexQuery All()
        {
            var query = new VertexQuery();
            query.Steps.Add("V()");
            return query;
        }

        public static VertexQuery Find(string label)
        {
            return All().HasLabel(label);
        }

        public static VertexQuery Find(Dictionary<string, object> properties)
        {
            return All().Has(properties);
        }

        public static VertexQuery Find(string label, Dictionary<string, object> properties)
        {
            return Find(label).Has(properties);
        }

        public static VertexQuery Vertex(string id)
        {
            var query = new VertexQuery();
            query.Steps.Add("V('" + Sanitize(id) + "')");
            return query;
        }

        public new static VertexQuery AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        public new static VertexQuery AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        public new static VertexQuery AddVertex(string label, Dictionary<string, object> properties)
        {
            var query = new VertexQuery();
            string step = "addV(";
            if (label != null && label != "")
                step += "'" + Sanitize(label) + "'";

            if (properties.Count > 0)
            {
                step += ", " + SeralizeProperties(properties);
            }
            step += ")";
            query.Steps.Add(step);
            return query;
        }

        public new static VertexQuery AddVertex(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();
            foreach (var item in properties)
            {
                if (item.Value is null)
                    properties.Remove(item.Key);
            }
            properties.Remove("VertexLabel");
            return AddVertex(vertex.VertexLabel, properties);
        }
    }
}
