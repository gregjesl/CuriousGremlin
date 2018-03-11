using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<From, TValue> : ListQuery<From, KeyValuePair<string, TValue>>
        where From: IGraphObject
    {
        internal DictionaryQuery(ITraversalQuery<IGraphObject, IGraphOutput> query) : base(query) { }
    }
}
