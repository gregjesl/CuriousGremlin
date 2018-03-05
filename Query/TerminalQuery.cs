using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class TerminalQuery : IGraphQuery
    {
        public string Query { protected set; get; }

        internal TerminalQuery(string query)
        {
            Query = query;
        }
    }
}
