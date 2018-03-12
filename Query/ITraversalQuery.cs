using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    /// <summary>
    /// Interface for a traversal graph query
    /// </summary>
    public interface ITraversalQuery
    {
        StepList Steps { get; }
    }

    /// <summary>
    /// Extention of <seealso cref="ITraversalQuery"/> for traversing from a graph object to a graph output
    /// </summary>
    /// <typeparam name="From">The type of graph object the traversal starts from</typeparam>
    /// <typeparam name="To">The type of graph object the traversal returns</typeparam>
    public interface ITraversalQuery<out From, out To> : ITraversalQuery
        where From: IGraphObject
        where To: IGraphOutput
    {
        
    }
}
