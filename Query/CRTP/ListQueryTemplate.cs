using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query.CRTP
{
    public class ListQueryTemplate<T, From, Query> : CollectionQueryTemplate<List<T>, From, Query>
        where Query : ListQueryTemplate<T, From, Query>
    {
        protected ListQueryTemplate(ITraversalQuery<From> query) : base(query) { }

        protected ListQueryTemplate() : base() { }

        public new ListQuery<T, List<T>> CreateSubQuery()
        {
            return new ListQuery<T, List<T>>();
        }

        /// <summary>
        /// Maps the list to a collection
        /// </summary>
        /// <returns>The unfold.</returns>
        public CollectionQuery<T, From> Unfold()
        {
            Steps.Add("unfold()");
            return new CollectionQuery<T, From>(this);
        }
    }
}
