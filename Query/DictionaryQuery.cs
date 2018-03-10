using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class DictionaryQuery<From> : ListQuery<From>
    {
        internal DictionaryQuery(ITraversalQuery query) : base(query) { }
    }
}
