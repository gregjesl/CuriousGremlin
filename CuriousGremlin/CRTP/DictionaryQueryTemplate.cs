using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.CRTP
{
    public class DictionaryQueryTemplate<TKey, TValue, From, Query> : CollectionQueryTemplate<Dictionary<TKey, TValue>, From, Query>
        where Query : DictionaryQueryTemplate<TKey, TValue, From, Query>
    {
        protected DictionaryQueryTemplate() : base() { }

        protected DictionaryQueryTemplate(ITraversalQuery<From> query) : base(query) { }
    }
}
