using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class TerminalQuery<From> : TraversalQuery<From, GraphQuery>
    {
        internal TerminalQuery(ITraversalQuery query) : base(query) { }
    }
}
