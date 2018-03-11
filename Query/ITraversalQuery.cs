using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public interface ITraversalQuery
    {
        StepList Steps { get; }
    }

    public interface ITraversalQuery<out From, out To> : ITraversalQuery
        where From: IGraphObject
        where To: IGraphOutput
    {
        
    }
}
