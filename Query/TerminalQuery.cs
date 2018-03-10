using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class TerminalQuery<From> : GraphQuery<From,object,TerminalQuery<From>>
    {
        internal TerminalQuery(IGraphQuery query) : base(query) { }
    }
}
