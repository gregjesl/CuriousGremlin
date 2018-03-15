using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class TerminalQuery<T, Query> : CollectionQuery<T, Query>
        where Query: TerminalQuery<T, Query>
    {
        internal TerminalQuery(ITraversalQuery query) : base(query) { }
    }
}
