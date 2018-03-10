using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query.Objects
{
    public class GraphEdge : GraphElement
    {
        public string inVLabel;
        public string outVLabel;
        public string inV;
        public string outV;
        public Dictionary<string, object> properties;
    }
}
