using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public abstract class GraphQuery
    {
        protected static string Sanitize(string input)
        {
            if (input is null)
                throw new ArgumentNullException("Input cannot be null");
            return input.Replace("'", @"\'");
        }

        internal static string GetObjectString(object item)
        {
            if (item is null)
                throw new ArgumentNullException("Provided object is null");
            string prop_type = item.GetType().ToString().ToLower();
            switch (prop_type)
            {
                case "system.string":
                    return "'" + Sanitize(item as string) + "'";
                case "system.boolean":
                    return (bool)item ? "true" : "false";
                case "system.single":
                    return ((float)item).ToString();
                case "system.double":
                    return ((double)item).ToString();
                case "system.decimal":
                    return ((decimal)item).ToString();
                case "system.int32":
                    return ((int)item).ToString();
                case "system.int64":
                    return ((long)item).ToString();
                case "system.datetime":
                    return "'" + Sanitize((item as DateTime?).Value.ToString("s")) + "'";
                default:
                    return GetObjectString(item.ToString());
            }
        }

        protected static string SeralizeProperties(Dictionary<string,object> properties)
        {
            List<string> outputs = new List<string>();
            foreach (var property in properties)
            {
                if (property.Value is null)
                    continue;
                outputs.Add("'" + Sanitize(property.Key) + "', " + GetObjectString(property.Value));
            }
            return string.Join(",", outputs);
        }

        public static VertexQuery<Graph> Vertex(string id)
        {
            var query = new VertexQuery<Graph>();
            query.Steps.Add("V('" + Sanitize(id) + "')");
            return query;
        }

        public static VertexQuery<Graph> Vertices()
        {
            var query = new VertexQuery<Graph>();
            query.Steps.Add("V()");
            return query;
        }

        public static VertexQuery<Graph> AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        public static VertexQuery<Graph> AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        public static VertexQuery<Graph> AddVertex(string label, Dictionary<string, object> properties)
        {
            var query = new VertexQuery<Graph>();
            string step = "addV(";
            if(label != null && label != "")
                step += "'" + Sanitize(label) + "'";

            if(properties.Count > 0)
            {
                step += ", " + SeralizeProperties(properties);
            }
            step += ")";
            query.Steps.Add(step);
            return query;
        }

        public static VertexQuery<Graph> AddVertex(IVertexObject vertex)
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
