using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.CRTP;

namespace CuriousGremlin.Query
{
    public class EdgeQuery<From> : EdgeQueryTemplate<From, EdgeQuery<From>>
    {
        internal EdgeQuery(ITraversalQuery<From> query) : base(query) { }

        internal EdgeQuery() : base() { }

        public static implicit operator EdgeQuery<From>(CollectionQuery<GraphEdge, From> query)
        {
            return new EdgeQuery<From>(query);
        }
    }

    public class EdgeQuery : EdgeQueryTemplate<GraphQuery, EdgeQuery>
    {
        internal EdgeQuery(ITraversalQuery<GraphQuery> query) : base(query) { }

        internal EdgeQuery() : base() { }

        public static implicit operator EdgeQuery(CollectionQuery<GraphEdge, GraphQuery> query)
        {
            return new EdgeQuery(query);
        }

        public static implicit operator EdgeQuery(EdgeQuery<GraphQuery> query)
        {
            return new EdgeQuery(query);
        }

        public static EdgeQuery All()
        {
            var query = new EdgeQuery();
            query.Steps.Add("E()");
            return query;
        }
    }
}
