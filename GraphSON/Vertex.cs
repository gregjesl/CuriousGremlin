using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.GraphSON
{
    public class Vertex
    {
        public string id;

        public string label;

        public Dictionary<string, List<VertexProperty>> properties;
    }
}
