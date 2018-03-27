using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;
using CuriousGremlin.CRTP;

namespace CuriousGremlin
{
    public class DictionaryQuery<TKey, TValue, From> : DictionaryQueryTemplate<TKey, TValue, From, DictionaryQuery<TKey, TValue, From>>
    {
        internal DictionaryQuery() : base() { }

        internal DictionaryQuery(ITraversalQuery<From> query) : base(query) { }

        public static implicit operator DictionaryQuery<TKey, TValue, From>(CollectionQuery<Dictionary<TKey, TValue>, From> query)
        {
            return new DictionaryQuery<TKey, TValue, From>(query);
        }
    }

    public class DictionaryQuery<TKey, TValue> : DictionaryQuery<TKey, TValue, GraphQuery>
    {
        internal DictionaryQuery() : base() { }

        internal DictionaryQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        public static implicit operator DictionaryQuery<TKey, TValue>(CollectionQuery<Dictionary<TKey, TValue>, GraphQuery> query)
        {
            return new DictionaryQuery<TKey, TValue>(query);
        }
    }
}
