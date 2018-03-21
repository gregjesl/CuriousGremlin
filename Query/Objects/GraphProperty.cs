using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query.Objects
{
    public class GraphProperty<T> : GraphElement
    {
        public T value { set; get; }
    }

    public class GraphProperty : GraphProperty<object>
    {
        
    }
}
