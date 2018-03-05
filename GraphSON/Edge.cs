using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.GraphSON
{
    public class Edge
    {
        public string id;
        public string label;
        public string inVLabel;
        public string outVLabel;
        public string inV;
        public string outV;
        public Dictionary<string, object> properties;
    }
}
