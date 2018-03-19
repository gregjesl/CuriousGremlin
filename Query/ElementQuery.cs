using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;

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

        /// <summary>
        /// Remove the traverser if its element does not have a value for the key.
        /// </summary>
        public Query Has(string key)
        {
            Steps.Add("has('" + Sanitize(key) + "')");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have the provided key or the value does not match for the provided key
        /// </summary>
        /// <returns>The has.</returns>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public Query Has(string key, object value)
        {
            Steps.Add("has('" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        public Query Has(string label, string key, object value)
        {
            Steps.Add("has('" + Sanitize(label) + "', '" + Sanitize(key) + "', " + GetObjectString(value) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have a key value that satisfies the bi-predicate.
        /// </summary>
        /// <param name="key">The key to filter by</param>
        /// <param name="predicate">The predicate to filter by</param>
        public Query Has(string key, GraphPredicate predicate)
        {
            Steps.Add("has('" + Sanitize(key) + "', " + predicate.ToString() + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have the provided key/value properies
        /// </summary>
        /// <param name="properties">The key/value properties to filter by</param>
        public Query Has(Dictionary<string, object> properties)
        {
            foreach(var item in properties)
            {
                Has(item.Key, item.Value);
            }
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have any of the labels.
        /// </summary>
        /// <param name="label">The label(s) that the traverser must have to continue</param>
        public Query HasLabel(params string[] label)
        {
            return HasLabel(label as IEnumerable<string>);
        }

        /// <summary>
        /// Remove the traverser if its element does not have any of the labels.
        /// </summary>
        /// <param name="labels">The label(s) that the traverser must have to continue</param>
        public Query HasLabel(IEnumerable<string> labels)
        {
            var list = new List<string>();
            foreach(var label in labels)
            {
                list.Add(GetObjectString(label));
            }
            Steps.Add("hasLabel(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have any of the ids.
        /// </summary>
        /// <param name="ids">The id(s) that the traverser must have to continue</param>
        public Query HasId(params string[] ids)
        {
            var list = new List<string>();
            foreach (var id in ids)
            {
                list.Add(GetObjectString(id));
            }
            Steps.Add("hasId(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its element does not have any of the ids.
        /// </summary>
        /// <param name="ids">The id(s) that the traverser must have to continue</param>
        public Query HasId(IEnumerable<string> ids)
        {
            return HasId(ids.ToArray());
        }

        /// <summary>
        /// Remove the traverser if the property does not have all of the provided keys.
        /// </summary>
        public Query HasKey(params string[] keys)
        {
            var list = new List<string>();
            foreach (var key in keys)
            {
                list.Add(GetObjectString(key));
            }
            Steps.Add("hasKey(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if the property does not have all of the provided keys.
        /// </summary>
        public Query HasKey(IEnumerable<string> keys)
        {
            return HasKey(keys.ToArray());
        }

        /// <summary>
        /// Remove the traverser if its property does not have all of the provided values.
        /// </summary>
        public Query HasValue(params string[] values)
        {
            var list = new List<string>();
            foreach (var value in values)
            {
                list.Add(GetObjectString(value));
            }
            Steps.Add("hasValue(" + string.Join(",", list) + ")");
            return this as Query;
        }

        /// <summary>
        /// Remove the traverser if its property does not have all of the provided values.
        /// </summary>
        public Query HasValue(IEnumerable<string> values)
        {
            return HasValue(values.ToArray());
        }

        /// <summary>
        /// Remove the traverser if its element has a value for the key.
        /// </summary>
        public Query HasNot(string key)
        {
            Steps.Add("hasNot('" + Sanitize(key) + "')");
            return this as Query;
        }

        public StringQuery<From> Label()
        {
            Steps.Add("label()");
            return new StringQuery<From>(this);
        }

        /// <summary>
        /// Orders the results by the specified property
        /// </summary>
        /// <returns>The by.</returns>
        /// <param name="property">Property.</param>
        /// <param name="ascending">If set to <c>true</c> ascending.</param>
        public Query OrderBy(string property, bool ascending = true)
        {
            string step = "order().by('" + Sanitize(property) + "', ";
            step += ascending ? "incr" : "decr";
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        /// <summary>
        /// Returns a value map
        /// </summary>
        /// <typeparam name="TValue">The type of value to be returned</typeparam>
        public DictionaryQuery<string, TValue, From> ValueMap<TValue>()
        {
            Steps.Add("valueMap()");
            return new DictionaryQuery<string, TValue, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's properties
        /// </summary>
        public ValueQuery<object, From>Properties()
        {
            Steps.Add("properties()");
            return new ValueQuery<object, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's properties
        /// </summary>
        public ValueQuery<object, From> Properties(string key)
        {
            Steps.Add("properties(" + Sanitize(key) + ")");
            return new ValueQuery<object, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's values
        /// </summary>
        public ValueQuery<object, From> Values()
        {
            Steps.Add("values()");
            return new ValueQuery<object, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's values
        /// </summary>
        public ValueQuery<object, From> Values(string key)
        {
            Steps.Add("values('" + Sanitize(key) + "')");
            return new ValueQuery<object, From>(this);
        }

        /// <summary>
        /// Returns a collection of the element's values
        /// </summary>
        public ValueQuery<TOutput, From> Values<TOutput>(string key)
        {
            Steps.Add("values('" + Sanitize(key) + "')");
            return new ValueQuery<TOutput, From>(this);
        }
    }

    public abstract class ElementQuery<T, From> : ElementQuery<T, From, ElementQuery<T, From>>
    {
        internal ElementQuery(ITraversalQuery<From> query) : base(query) { }

        internal ElementQuery() : base() { }
    }
}
