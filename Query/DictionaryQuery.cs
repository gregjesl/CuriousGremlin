using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<TKey, TValue, Query> : ListQuery<KeyValuePair<string, TValue>, Query>
        where Query: DictionaryQuery<TKey, TValue, Query>
    {
        internal DictionaryQuery(ITraversalQuery query) : base(query) { }
    }

    public class DictionaryQuery<TKey, TValue> : ListQuery<KeyValuePair<string, TValue>, DictionaryQuery<TKey, TValue>>
    {
        internal DictionaryQuery(ITraversalQuery query) : base(query) { }
    }
}
