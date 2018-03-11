using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public interface ITraversalQuery<From,To>
    {
        StepList Steps { get; }
    }
}
