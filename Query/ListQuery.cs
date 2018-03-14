using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ListQuery<T, From> : CollectionQuery<GraphListCollection<T>, From, GraphListCollection<T>, ListQuery<T, From>>
        where From : IGraphObject
    {
        internal ListQuery(ITraversalQuery query) : base(query) { }

        private ListQuery() : base() { }

        public ListQuery<T, GraphListCollection<T>> CreateSubQuery()
        {
            return new ListQuery<T, GraphListCollection<T>>();
        }

        public CollectionQuery<T, From> Unfold()
        {
            Steps.Add("unfold()");
            return new CollectionQuery<T, From>(this);
        }
    }
}
