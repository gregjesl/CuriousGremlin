using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ValueQuery<From> : CollectionQuery<From,GraphValue,ValueQuery<From>>
    {
        internal ValueQuery(IGraphQuery query) : base(query) { }

        public BooleanQuery Is(float value)
        {
            Query += ".is(" + GetObjectString(value) + ")";
            return new BooleanQuery(Query);
        }

        public BooleanQuery Is(double value)
        {
            Query += ".is(" + GetObjectString(value) + ")";
            return new BooleanQuery(Query);
        }

        public BooleanQuery Is(decimal value)
        {
            Query += ".is(" + GetObjectString(value) + ")";
            return new BooleanQuery(Query);
        }

        public BooleanQuery Is(int value)
        {
            Query += ".is(" + GetObjectString(value) + ")";
            return new BooleanQuery(Query);
        }

        public BooleanQuery Is(long value)
        {
            Query += ".is(" + GetObjectString(value) + ")";
            return new BooleanQuery(Query);
        }

        public BooleanQuery Is(GraphPredicate predicate)
        {
            Query += ".is(" + predicate.ToString() + ")";
            return new BooleanQuery(Query);
        }

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
