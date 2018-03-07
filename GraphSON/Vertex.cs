using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CuriousGremlin.GraphSON
{
    public class Vertex
    {
        public string id;

        public string label;

        public Dictionary<string, List<KeyValuePair<string,object>>> properties;
    }
}
