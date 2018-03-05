using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public abstract class GraphQuery : IGraphQuery
    {
        public string Query { protected set; get; }

        internal GraphQuery(string query)
        {
            this.Query = query;
        }

        public static string Sanitize(string input)
        {
            return input.Replace("'", "\'");
        }

        internal static string GetObjectString(object item)
        {
            string prop_type = item.GetType().ToString().ToLower();
            switch (prop_type)
            {
                case "system.string":
                    return "'" + GraphQuery.Sanitize(item as string) + "'";
                case "system.boolean":
                    return (bool)item ? "true" : "false";
                case "system.float":
                    return string.Format("%f", (float)item);
                case "system.double":
                    return string.Format("%f", (double)item);
                case "system.int":
                    return string.Format("%i", (int)item);
                case "system.int64":
                    return string.Format("%li", (long)item);
                case "system.datetime":
                    return "'" + GraphQuery.Sanitize((item as DateTime?).Value.ToString("s")) + "'";
                default:
                    return GetObjectString(item.ToString());
            }
        }

        internal static string SeralizeProperties(Dictionary<string,object> properties)
        {
            List<string> outputs = new List<string>();
            foreach (var property in properties)
            {
                outputs.Add("'" + Sanitize(property.Key) + "', '" + GetObjectString(property.Value) + "'");
            }
            return string.Join(",", outputs);
        }

        public static VertexQuery Vertex(string id)
        {
            return new VertexQuery("g.V('" + Sanitize(id) + "')");
        }

        public static VertexQuery Vertices()
        {
            return new VertexQuery("g.V()");
        }

        public static VertexQuery AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        public static VertexQuery AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        public static VertexQuery AddVertex(string label, Dictionary<string, object> properties)
        {
            string query = "g.addV(";
            if(label != null && label != "")
                query += "'" + Sanitize(label) + "'";

            if(properties.Count > 0)
            {
                query += ", " + SeralizeProperties(properties);
            }
            query += ")";
            return new VertexQuery(query);
        }

        public GraphQuery AddProperty(string key, object value)
        {
            this.Query += ".property('" + Sanitize(key) + "', " + GetObjectString(value) + ")";
            return this;
        }

        public GraphQuery Aggregate(string label)
        {
            this.Query += ".aggregate('" + Sanitize(label) + "')";
            return this;
        }

        public GraphQuery Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Query += string.Format(".coin({0})", probability);
            return this;
        }

        public GraphQuery CyclicPath()
        {
            Query += ".cyclicPath()";
            return this;
        }

        public GraphQuery Dedup()
        {
            Query += ".dedup()";
            return this;
        }

        public TerminalQuery Drop()
        {
            Query += ".drop()";
            return new TerminalQuery(Query);
        }

        public GraphQuery Has(string key, object value)
        {
            Query += ".has('" + Sanitize(key) + "', " + GetObjectString(value) + ")";
            return this;
        }

        public GraphQuery HasLabel(string label)
        {
            Query += ".hasLabel('" + Sanitize(label) + "')";
            return this;
        }

        public GraphQuery Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Query += string.Format(".limit({0})", limit);
            return this;
        }

        public GraphQuery OrderBy(string property, bool ascending = true)
        {
            Query += ".order().by('" + Sanitize(property) + "', ";
            Query += ascending ? "incr" : "decr";
            Query += ")";
            return this;
        }

        public GraphQuery Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentException("Lower bound cannot be less than zero");
            if (upperBound < lowerBound)
                throw new ArgumentException("Upper bound must be greater than or equal to the lower bound");
            Query += string.Format(".range({0},{1})", lowerBound, upperBound);
            return this;
        }

        public GraphQuery SimplePath()
        {
            Query += ".simplePath()";
            return this;
        }

        public GraphQuery Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Query += string.Format(".tail({0})", limit);
            return this;
        }

        public GraphQuery TimeLimit(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentException("Time must be greater than zero");
            Query += string.Format(".timeLimit({0})", milliseconds);
            return this;
        }
    }
}
