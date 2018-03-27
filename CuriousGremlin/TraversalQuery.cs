using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Objects;

namespace CuriousGremlin
{
    /// <summary>
    /// Implementation of <seealso cref="ITraversalQuery{From, To}"/>
    /// </summary>
    public class TraversalQuery<From, To> : GraphQuery, ITraversalQuery<From, To>
    {
        /// <summary>
        /// A list of steps the traversal will take
        /// </summary>
        public StepList Steps { set; get; }

        internal TraversalQuery(ITraversalQuery query)
        {
            if (query is null)
                throw new ArgumentNullException("Step list cannot be null");
            Steps = query.Steps;
        }

        protected TraversalQuery()
        {
            Steps = new StepList();
        }

        /*
        public TerminalQuery<From> Explain()
        {
            Steps.Add("explain()");
            return new TerminalQuery<From>(this);
        }
        */

        public override string ToString()
        {
            if (Steps.Count < 1)
                throw new NullReferenceException("Traversal query contains no steps");
            if (typeof(From) == typeof(GraphQuery) && Steps[0] != "g")
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
