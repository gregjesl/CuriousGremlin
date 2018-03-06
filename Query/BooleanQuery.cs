using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class BooleanQuery : GraphQuery
    {
        internal BooleanQuery(string query) : base(query) { }
    }
}
