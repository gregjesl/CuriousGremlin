using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query.Objects
{
    public abstract class GraphElement
    {
        public string id { set; get; }
        public string label { set; get; }
    }
}
