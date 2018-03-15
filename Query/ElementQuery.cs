using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public abstract class ElementQuery<T, From, Query> : CollectionQuery<T, From, Query>
        where Query: ElementQuery<T, From, Query>
    {
        protected ElementQuery(ITraversalQuery<From> query) : base(query) { }

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

        public StringQuery<From>Id()
        {
            Steps.Add("id()");
            return new StringQuery<From>(this);
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

        public StringQuery<From> Label()
        {
            Steps.Add("label()");
            return new StringQuery<From>(this);
        }

        public DictionaryQuery<string, TValue, From> ValueMap<TValue>()
        {
            Steps.Add("valueMap()");
            return new DictionaryQuery<string, TValue, From>(this);
        }

        public ValueQuery<object, From>Properties()
        {
            Steps.Add("properties()");
            return new ValueQuery<object, From>(this);
        }

        public ValueQuery<object, From> Properties(string key)
        {
            Steps.Add("properties(" + Sanitize(key) + ")");
            return new ValueQuery<object, From>(this);
        }

        public Query Values(string key)
        {
            Steps.Add("values('" + Sanitize(key) + "')");
            return this as Query;
        }
    }

    public abstract class ElementQuery<T, From> : ElementQuery<T, From, ElementQuery<T, From>>
    {
        internal ElementQuery(ITraversalQuery<From> query) : base(query) { }

        internal ElementQuery() : base() { }
    }
}
