using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class TraversalQuery<From, To, Query> : GraphQuery, ITraversalQuery<From, To>
    {
        public StepList Steps { set; get; }

        internal TraversalQuery(ITraversalQuery<IGraphObject, IGraphOutput> query)
        {
            if (query is null)
                throw new ArgumentNullException("Step list cannot be null");
            Steps = query.Steps;
        }

        protected TraversalQuery()
        {
            Steps = new StepList();
        }

        public override string ToString()
        {
            if (Steps.Count < 1)
                throw new NullReferenceException("Traversal query contains no steps");
            if (typeof(From) == typeof(Graph) && Steps[0] != "g")
                Steps.Insert(0, "g");
            return Steps.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj.ToString() == Steps.ToString();
        }

        public override int GetHashCode()
        {
            return Steps.ToString().GetHashCode();
        }
    }
}
