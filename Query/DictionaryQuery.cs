using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<From> : ListQuery<From>
    {
        internal DictionaryQuery(ITraversalQuery<From, IGraphOutput> query) : base(query) { }
    }
}
