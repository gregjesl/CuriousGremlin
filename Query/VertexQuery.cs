using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.Query
{
    public class VertexQuery<From, Query> : ElementQuery<GraphVertex, From, Query>
        where Query: VertexQuery<From, Query>
    {
        protected VertexQuery(ITraversalQuery<From> query) : base(query) { }

        protected VertexQuery() : base() { }

        public static implicit operator VertexQuery<From, Query>(VertexQuery<From> query)
        {
            return new VertexQuery<From, Query>(query);
        }

        public static implicit operator VertexQuery<From, Query>(ElementQuery<GraphVertex, From> query)
        {
            return new VertexQuery<From, Query>(query);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <param name="label">The label of the new edge</param>
        /// <param name="vertexID">The identifier of the vertex the edge will connect to</param>
        public EdgeQuery<From> AddEdge(string label, string vertexID)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(g.V('" + Sanitize(vertexID) + "'))");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <param name="label">The label of the new edge</param>
        /// <param name="vertexID">The identifier of the vertex the edge will connect to</param>
        /// <param name="properties">The properties to add to the new edge</param>
        public EdgeQuery<From> AddEdge(string label, string vertexID, Dictionary<string, object> properties)
        {
            return AddEdge(label, vertexID).AddProperties(properties);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <returns>The edge.</returns>
        /// <param name="label">The label of the new edge</param>
        /// <param name="vertices">The vertices to connect to</param>
        public EdgeQuery<From> AddEdge(string label, VertexQuery vertices)
        {
            Steps.Add("addE('" + Sanitize(label) + "').to(" + vertices.ToString() + ")");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <returns>The edge.</returns>
        /// <param name="label">The label of the new edge</param>
        /// <param name="vertices">The vertices to connect to</param>
        /// <param name="properties">The properties to add to the new edges</param>
        public EdgeQuery<From> AddEdge(string label, VertexQuery vertices, Dictionary<string, object> properties)
        {
            return AddEdge(label, vertices).AddProperties(properties);
        }

        /// <summary>
        /// Return a collection of outward vertices
        /// </summary>
        public Query Out()
        {
            Steps.Add("out()");
            return this as Query;
        }

        /// <summary>
        /// Return a collection of outward vertices
        /// </summary>
        /// <param name="label">The label of the vertices to move to</param>
        public Query Out(string label)
        {
            Steps.Add("out('" + Sanitize(label) + "')");
            return this as Query;
        }

        /// <summary>
        /// Returns a collection of inward vertices
        /// </summary>
        /// <returns>The in.</returns>
        public Query In()
        {
            Steps.Add("in()");
            return this as Query;
        }

        /// <summary>
        /// Returns a collection of inward vertices
        /// </summary>
        /// <param name="label">The label of the vertices to move to</param>
        public Query In(string label)
        {
            Steps.Add("in('" + Sanitize(label) + "')");
            return this as Query;
        }

        /// <summary>
        /// Returns a collection of connected vertices
        /// </summary>
        public Query Both()
        {
            Steps.Add("both()");
            return this as Query;
        }

        /// <summary>
        /// Returns a collection of connected vertices
        /// </summary>
        /// <param name="label">The label of the vertices to move to</param>
        public Query Both(string label)
        {
            Steps.Add("both('" + Sanitize(label) + "')");
            return this as Query;
        }

        /// <summary>
        /// Returns a collection of outward edges
        /// </summary>
        public EdgeQuery<From> OutE()
        {
            Steps.Add("outE()");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Returns a collection of outward edges
        /// </summary>
        /// <param name="label">The label of the edges to move to</param>
        public EdgeQuery<From> OutE(string label)
        {
            Steps.Add("outE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Returns a collection of inward edges
        /// </summary>
        public EdgeQuery<From> InE()
        {
            Steps.Add("inE()");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Returns a collection of inward edges
        /// </summary>
        /// <returns>The e.</returns>
        /// <param name="label">The label of the edges to move to</param>
        public EdgeQuery<From> InE(string label)
        {
            Steps.Add("inE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Returns a collection of connected edges
        /// </summary>
        public EdgeQuery<From> BothE()
        {
            Steps.Add("bothE()");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Returns a collected of connected edges
        /// </summary>
        /// <returns>The e.</returns>
        /// <param name="label">The label of the edges to move to</param>
        public EdgeQuery<From> BothE(string label)
        {
            Steps.Add("bothE('" + Sanitize(label) + "')");
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Adds a list property to the vertex which allows for multiple properties under the same key
        /// </summary>
        /// <param name="key">The key of the new property</param>
        /// <param name="value">The value of the new property</param>
        public Query AddListProperty(string key, string value)
        {
            Steps.Add(".property(list, '" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        /// <summary>
        /// Creates a sub query
        /// </summary>
        /// <returns>The sub query</returns>
        public new VertexQuery<GraphVertex> CreateSubQuery()
        {
            return new VertexQuery<GraphVertex>();
        }
    }

    public class VertexQuery<From> : VertexQuery<From, VertexQuery<From>>
    {
        public VertexQuery(ITraversalQuery<From> query) : base(query) { }

        public VertexQuery() : base() { }
    }

    public class VertexQuery : VertexQuery<GraphQuery>
    {
        internal VertexQuery() : base() { }

        public static VertexQuery All()
        {
            var query = new VertexQuery();
            query.Steps.Add("V()");
            return query;
        }

        /// <summary>
        /// Returns all graph vertices with the specified label 
        /// </summary>
        public static VertexQuery Find(string label)
        {
            return All().HasLabel(label) as VertexQuery;
        }

        /// <summary>
        /// Returns all graph vertices with the specified properties
        /// </summary>
        public static VertexQuery Find(Dictionary<string, object> properties)
        {
            return All().Has(properties) as VertexQuery;
        }

        /// <summary>
        /// Returns all graph vertices with the specified label and properties
        /// </summary>
        public static VertexQuery Find(string label, Dictionary<string, object> properties)
        {
            return Find(label).Has(properties) as VertexQuery;
        }

        /// <summary>
        /// Returns the specified vertex
        /// </summary>
        /// <param name="id">The identifier of the vertex</param>
        public static VertexQuery Vertex(string id)
        {
            var query = new VertexQuery();
            query.Steps.Add("V('" + Sanitize(id) + "')");
            return query;
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        /// <param name="label">The label of the new vertex</param>
        public static VertexQuery Create(string label)
        {
            return Create(label, new Dictionary<string, object>());
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        /// <param name="properties">The properties of the new vertex</param>
        public static VertexQuery Create(Dictionary<string, object> properties)
        {
            return Create(null, properties);
        }

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        /// <param name="label">The label of the new vertex</param>
        /// <param name="properties">The properties of the new vertex</param>
        public static VertexQuery Create(string label, Dictionary<string, object> properties)
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

        /// <summary>
        /// Creates a new vertex from the provided object
        /// </summary>
        /// <param name="vertex">The object to map to a vertex</param>
        public static VertexQuery Create(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();
            foreach (var item in properties)
            {
                if (item.Value is null)
                    properties.Remove(item.Key);
            }
            properties.Remove("VertexLabel");
            return Create(vertex.VertexLabel, properties);
        }
    }
}
