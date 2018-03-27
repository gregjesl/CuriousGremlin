using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;

namespace CuriousGremlin
{
    public class TerminalQuery<From> : TraversalQuery<From, GraphQuery>
    {
        internal TerminalQuery(ITraversalQuery query) : base(query) { }
    }
}
