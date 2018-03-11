using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public interface ITraversalQuery<From,To> 
        where From: IGraphObject
        where To: IGraphOutput
    {
        StepList Steps { get; }
    }
}
