using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    internal interface IGraphQuery
    {
        StepList Steps { get; }
    }
}
