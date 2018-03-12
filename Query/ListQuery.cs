using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ListQuery<T, From> : CollectionQuery<T, From, ListCollection<T>, ListQuery<T, From>>
        where From : IGraphObject
    {
        internal ListQuery(ITraversalQuery query) : base(query) { }

        public object Unfold()
        {
            Steps.Add("unfold()");
            return new CollectionQuery<T, From>(this);
        }
    }
}
