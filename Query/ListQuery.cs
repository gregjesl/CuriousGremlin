using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ListQuery<T, From, Query> : CollectionQuery<List<T>, From, Query>
        where Query : ListQuery<T, From, Query>
    {
        protected ListQuery(ITraversalQuery<From> query) : base(query) { }

        protected ListQuery() : base() { }

        public static implicit operator ListQuery<T, From, Query>(ListQuery<T, From> query)
        {
            return new ListQuery<T, From, Query>(query);
        }

        public CollectionQuery<T, From> Unfold()
        {
            Steps.Add("unfold()");
            return new CollectionQuery<T, From>(this);
        }
    }

    public class ListQuery<T, From> : ListQuery<T, From, ListQuery<T, From>>
    {
        internal ListQuery(ITraversalQuery<From> query) : base(query) { }

        internal ListQuery() : base() { }
    }
}
