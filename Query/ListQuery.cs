using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class ListQuery : CollectionQuery<ListQuery>
    {
        internal ListQuery(string query) : base(query) { }


        public object Unfold()
        {
            throw new NotImplementedException();
        }
    }
}
