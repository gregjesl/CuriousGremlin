using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query
{
    public class ValueQuery : GraphQuery
    {
        internal ValueQuery(string query) : base(query) { }

        public ValueQuery Max()
        {
            Query += ".max()";
            return this;
        }

        public ValueQuery Mean()
        {
            Query += ".mean()";
            return this;
        }

        public ValueQuery Min()
        {
            Query += ".min()";
            return this;
        }

        public ValueQuery Sum()
        {
            Query += ".sum()";
            return this;
        }
    }
}
