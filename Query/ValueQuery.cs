using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ValueQuery<From> : CollectionQuery<From,GraphValue,ValueQuery<From>>
    {
        internal ValueQuery(ITraversalQuery query) : base(query) { }

        public BooleanQuery<From> Is(float value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return new BooleanQuery<From>(this);
        }

        public BooleanQuery<From> Is(double value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return new BooleanQuery<From>(this);
        }

        public BooleanQuery<From> Is(decimal value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return new BooleanQuery<From>(this);
        }

        public BooleanQuery<From> Is(int value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return new BooleanQuery<From>(this);
        }

        public BooleanQuery<From> Is(long value)
        {
            Steps.Add("is(" + GetObjectString(value) + ")");
            return new BooleanQuery<From>(this);
        }

        public BooleanQuery<From> Is(GraphPredicate predicate)
        {
            Steps.Add("is(" + predicate.ToString() + ")");
            return new BooleanQuery<From>(this);
        }

        public ValueQuery<From> Max()
        {
            Steps.Add("max()");
            return this;
        }

        public ValueQuery<From> Mean()
        {
            Steps.Add("mean()");
            return this;
        }

        public ValueQuery<From> Min()
        {
            Steps.Add("min()");
            return this;
        }

        public ValueQuery<From> Sum()
        {
            Steps.Add("sum()");
            return this;
        }
    }
}
