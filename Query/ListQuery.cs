using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class ListQuery<From> : CollectionQuery<From,List<object>,ListQuery<From>>
    {
        internal ListQuery(ITraversalQuery query) : base(query) { }

        public object Unfold()
        {
            throw new NotImplementedException();
        }
    }
}
