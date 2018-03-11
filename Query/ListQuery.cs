using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ListQuery<From, Output> : CollectionQuery<From, ListCollection<object>,ListQuery<From, Output>>
        where From : IGraphObject
    {
        internal ListQuery(ITraversalQuery<IGraphObject, IGraphOutput> query) : base(query) { }

        public object Unfold()
        {
            throw new NotImplementedException();
        }
    }
}
