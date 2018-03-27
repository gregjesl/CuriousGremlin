using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace CuriousGremlin.CRTP
{
    public class ListQueryTemplate<T, From, Query> : CollectionQueryTemplate<List<T>, From, Query>
        where Query : ListQueryTemplate<T, From, Query>
    {
        protected ListQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected ListQueryTemplate() : base() { }

        public new ListQuery<T, List<T>> CreateSubQuery()
        {
            return new ListQuery<T, List<T>>();
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="label">The label of the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="properties">The properties to add to the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="label">The label of the new vertex/vertices</param>
        /// <param name="properties">The properties to add to the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(string label, Dictionary<string, object> properties)
        {
            string step = "addV(";
            if (label != null && label != "")
                step += "'" + Sanitize(label) + "'";
            step += ")";
            Steps.Add(step);
            return (new VertexQuery<From>(this)).AddProperties(properties);
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="vertex">The object to be serialized</param>
        public VertexQuery<From> AddVertex(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();

            properties.Remove(nameof(vertex.VertexLabel));
            return AddVertex(vertex.VertexLabel, properties.Where(p => p.Value != null).ToDictionary(p => p.Key, p => p.Value));
        }

        /// <summary>
        /// Maps the list to a collection
        /// </summary>
        /// <returns>The unfold.</returns>
        public CollectionQuery<T, From> Unfold()
        {
            Steps.Add("unfold()");
            return new CollectionQuery<T, From>(this);
        }
    }
}
