using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;
using Newtonsoft.Json.Linq;
using CuriousGremlin.Query.CRTP;

namespace CuriousGremlin.Query
{
    public class CollectionQuery<T, From> : CollectionQueryTemplate<T, From, CollectionQuery<T, From>>
    {
        internal CollectionQuery(ITraversalQuery<From> query) : base(query) { }

        internal CollectionQuery() : base() { }
    }

    public class CollectionQuery<T> : CollectionQueryTemplate<T, GraphQuery, CollectionQuery<T, GraphQuery>>
    {
        internal CollectionQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        internal CollectionQuery() : base() { }

        public static implicit operator CollectionQuery<T>(CollectionQuery<T, GraphQuery> query)
        {
            return new CollectionQuery<T>(query);
        }
    }
}
