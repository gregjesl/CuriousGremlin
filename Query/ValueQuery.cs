using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Predicates;
using CuriousGremlin.Query.Objects;

namespace CuriousGremlin.Query
{
    public class ValueQuery<T, From> : CollectionQuery<T, From,GraphValue,ValueQuery<T, From>>
        where From: IGraphObject
    {
        internal ValueQuery(ITraversalQuery<From, IGraphOutput> query) : base(query) { }

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

        public ValueQuery<string, From> Key()
        {
            Steps.Add("key()");
            return new ValueQuery<string, From>(this);
        }

        public ValueQuery<T, From> Math(string mapping)
        {
            Steps.Add("math(" + Sanitize(mapping) + ")");
            return this;
        }

        public ValueQuery<T, From> Max()
        {
            Steps.Add("max()");
            return this;
        }

        public ValueQuery<T, From> Mean()
        {
            Steps.Add("mean()");
            return this;
        }

        public ValueQuery<T, From> Min()
        {
            Steps.Add("min()");
            return this;
        }

        public ValueQuery<T, From> Sum()
        {
            Steps.Add("sum()");
            return this;
        }
    }
}
