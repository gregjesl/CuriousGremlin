using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CuriousGremlin.Query.CRTP
{
    public class VertexQueryTemplate<From, Query> : ElementQueryTemplate<GraphVertex, From, Query>
        where Query : VertexQueryTemplate<From, Query>
    {
        protected VertexQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected VertexQueryTemplate() : base() { }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <param name="label">The label of the new edge</param>
        /// <param name="direction">The step specifiying the vertex to connect to</param>
        public EdgeQuery<From> AddEdge(string label, DirectionStep direction)
        {
            Steps.Add("addE('" + Sanitize(label) + "')");
            Steps.Add(direction.ToString());
            return new EdgeQuery<From>(this);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <param name="label">The label of the new edge</param>
        /// <param name="direction">The step specifiying the vertex to connect to</param>
        /// <param name="properties">The properties to add to the new edge</param>
        public EdgeQuery<From> AddEdge(string label, DirectionStep direction, Dictionary<string, object> properties)
        {
            return AddEdge(label, direction).AddProperties(properties);
        }

        /// <summary>
        /// Adds an edge to the vertex
        /// </summary>
        /// <returns>The edge.</returns>
        /// <param name="edge">The object to serialize</param>
        /// <param name="direction">The step specifiying the vertex to connect to</param>
        public EdgeQuery<From> AddEdge(IEdgeObject edge, DirectionStep direction)
        {
            var properties = JObject.FromObject(edge).ToObject<Dictionary<string, object>>();
            properties.Remove(nameof(edge.EdgeLabel));
            return AddEdge(edge.EdgeLabel, direction).AddProperties(properties.Where(p => p.Value != null).ToDictionary(p => p.Key, p => p.Value));
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
        /// Returns a collection of the element's properties
        /// </summary>
        public DictionaryQuery<string, List<GraphProperty>, From> PropertyMap()
        {
            Steps.Add("propertyMap()");
            return new DictionaryQuery<string, List<GraphProperty>, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's values
        /// </summary>
        public DictionaryQuery<string, List<object>, From> ValueMap()
        {
            Steps.Add("valueMap()");
            return new DictionaryQuery<string, List<object>, From>(this);
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
}
