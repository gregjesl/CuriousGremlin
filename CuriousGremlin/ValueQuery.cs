using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin;
using CuriousGremlin.Objects;
using CuriousGremlin.CRTP;

namespace CuriousGremlin
{
    public class ValueQuery<T, From> : ValueQueryTemplate<T, From, ValueQuery<T, From>>
    {
        internal ValueQuery(ITraversalQuery<From> query) : base(query) { }

        internal ValueQuery() : base() { }

        public static implicit operator ValueQuery<T, From>(StringQuery<From> query)
        {
            return new ValueQuery<T, From>(query);
        }

        public static implicit operator ValueQuery<T, From>(IntegerQuery<From> query)
        {
            return new ValueQuery<T, From>(query);
        }
    }

    public class StringQuery<From> : ValueQuery<string, From>
    {
        internal StringQuery(ITraversalQuery<From> query) : base(query) { }

        internal StringQuery() : base() { }

        public static implicit operator StringQuery<From>(CollectionQuery<string, From> query)
        {
            return new StringQuery<From>(query);
        }
    }

    public class StringQuery : StringQuery<GraphQuery>
    {
        internal StringQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        internal StringQuery() : base() { }

        public static implicit operator StringQuery(CollectionQuery<string, GraphQuery> query)
        {
            return new StringQuery(query);
        }
    }

    public class IntegerQuery<From> : ValueQuery<long, From>
    {
        internal IntegerQuery(ITraversalQuery<From> query) : base(query) { }

        internal IntegerQuery() : base() { }

        public static implicit operator IntegerQuery<From>(CollectionQuery<long, From> query)
        {
            return new IntegerQuery<From>(query);
        }
    }

    public class IntegerQuery : ValueQuery<long, GraphQuery>
    {
        internal IntegerQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        internal IntegerQuery() : base() { }

        public static implicit operator IntegerQuery(CollectionQuery<long, GraphQuery> query)
        {
            return new IntegerQuery(query);
        }
    }
}
