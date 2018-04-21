using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Objects
{
    public interface IVertexObject
    {
        string VertexLabel { get; }
        string ID { set; }
    }
}
