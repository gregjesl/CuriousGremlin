﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;
using Newtonsoft.Json.Linq;
using CuriousGremlin.CRTP;

namespace CuriousGremlin
{
    public class VertexQuery<From> : VertexQueryTemplate<From, VertexQuery<From>>
    {
        internal VertexQuery(ITraversalQuery<From> query) : base(query) { }

        internal VertexQuery() : base() { }

        public static implicit operator VertexQuery<From>(CollectionQuery<GraphVertex, From> query)
        {
            return new VertexQuery<From>(query);
        }
    }

    public class VertexQuery : VertexQueryTemplate<GraphQuery, VertexQuery>
    {
        internal VertexQuery() : base() { }

        public VertexQuery(ITraversalQuery<GraphQuery, GraphVertex> query) : base(query) { }

        public static implicit operator VertexQuery(CollectionQuery<GraphVertex, GraphQuery> query)
        {
            return new VertexQuery(query);
        }

        public static implicit operator VertexQuery(VertexQuery<GraphQuery> query)
        {
            return new VertexQuery(query);
        }

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
            step += ")";
            query.Steps.Add(step);
            query.AddProperties(properties);
            return query;
        }

        /// <summary>
        /// Creates a new vertex from the provided object
        /// </summary>
        /// <param name="vertex">The object to map to a vertex</param>
        public static VertexQuery Create(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();
            properties.Remove(nameof(vertex.VertexLabel));
            return Create(vertex.VertexLabel, properties.Where(p => p.Value != null).ToDictionary(p => p.Key, p => p.Value));
        }
    }
}
