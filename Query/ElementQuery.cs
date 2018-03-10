using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public abstract class ElementQuery<From,To,Query> : CollectionQuery<From,To,Query> where Query: ElementQuery<From,To,Query>
    {
        internal ElementQuery(ITraversalQuery query) : base(query) { }

        protected ElementQuery() : base() { }

        public Query AddProperty(string key, object value)
        {
            Steps.Add("property('" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query AddProperties(Dictionary<string, object> properties)
        {
            foreach(var item in properties)
            {
                AddProperty(item.Key, item.Value);
            }
            return this as Query;
        }

        public Query Has(string key, object value)
        {
            Steps.Add("has('" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Has(Dictionary<string, object> properties)
        {
            foreach(var item in properties)
            {
                Has(item.Key, item.Value);
            }
            return this as Query;
        }

        public Query HasLabel(string label)
        {
            Steps.Add("hasLabel('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query Values(string key)
        {
            Steps.Add("values('" + Sanitize(key) + "')");
            return this as Query;
        }
    }
}
