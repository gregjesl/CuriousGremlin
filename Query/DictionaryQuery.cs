using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<TValue, From> : ListQuery<KeyValuePair<string, TValue>, From>
        where From: IGraphObject
    {
        internal DictionaryQuery(ITraversalQuery query) : base(query) { }
    }
}
