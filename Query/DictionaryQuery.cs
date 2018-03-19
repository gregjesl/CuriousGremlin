using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<TKey, TValue, From, Query> : CollectionQuery<Dictionary<TKey, TValue>, From, Query>
        where Query: DictionaryQuery<TKey, TValue, From, Query>
    {
        internal DictionaryQuery(ITraversalQuery<From> query) : base(query) { }
    }

    public class DictionaryQuery<TKey, TValue, From> : DictionaryQuery<TKey, TValue, From, DictionaryQuery<TKey, TValue, From>>
    {
        internal DictionaryQuery(ITraversalQuery<From> query) : base(query) { }
    }
}
