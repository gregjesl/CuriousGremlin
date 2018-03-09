using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public abstract class ElementQuery<T> : CollectionQuery<T> where T: ElementQuery<T>
    {
        protected ElementQuery(string query) : base(query) { }

        public T AddProperty(string key, object value)
        {
            Query += ".property('" + Sanitize(key) + "', " + GetObjectString(value) + ")";
            return this as T;
        }

        public T AddProperties(Dictionary<string, object> properties)
        {
            foreach(var item in properties)
            {
                AddProperty(item.Key, item.Value);
            }
            return this as T;
        }

        public T Has(string key, object value)
        {
            Query += ".has('" + Sanitize(key) + "', " + GetObjectString(value) + ")";
            return this as T;
        }

        public T Has(Dictionary<string, object> properties)
        {
            foreach(var item in properties)
            {
                Has(item.Key, item.Value);
            }
            return this as T;
        }

        public T HasLabel(string label)
        {
            Query += ".hasLabel('" + Sanitize(label) + "')";
            return this as T;
        }

        public T Values(string name)
        {
            Query += ".values('" + Sanitize(name) + "')";
            return this as T;
        }
    }
}
