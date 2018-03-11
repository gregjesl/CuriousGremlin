using System;
using System.Collections.Generic;
using System.Text;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;

namespace CuriousGremlin.Query
{
    public abstract class CollectionQuery<T, From, To, Query> : TraversalQuery<From, GraphCollection<T>> where Query : CollectionQuery<T, From, To, Query>
        where From : IGraphObject
        where To : IGraphOutput
    {
        public enum RepeatTypeEnum { DoWhile, WhileDo };

        internal CollectionQuery(ITraversalQuery query) : base(query) { }

        protected CollectionQuery() : base() { }

        public Query Aggregate(string label)
        {
            Steps.Add("aggregate('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query And(params CollectionQuery<T, To>[] conditions)
        {
            return And(conditions);
        }

        public Query And(IEnumerable<CollectionQuery<T, To>> conditions)
        {
            string step = "and(";
            List<string> stepStrings = new List<string>();
            foreach(var condition in conditions)
            {
                stepStrings.Add(condition.ToString());
            }
            step += string.Join(",", stepStrings);
            step += ")";
            return this as Query;
        }

        public Query And(BooleanQuery<To> condition)
        {
            Steps.Add(condition.ToString());
            return this as Query;
        }

        public BooleanQuery<From> Any()
        {
            return Count().Is(new GPGreaterThanOrEqualTo(1));
        }

        public Query As(string label)
        {
            Steps.Add("as('" + Sanitize(label) + "')");
            return this as Query;
        }

        public Query Barrier()
        {
            Steps.Add("barrier()");
            return this as Query;
        }

        public ValueQuery<T, From> Count()
        {
            Steps.Add("count()");
            return new ValueQuery<T, From>(this);
        }

        /*
        public GraphQuery Choose(BooleanQuery condition, GraphQuery TrueQuery, GraphQuery FalseQuery)
        {
            throw new NotImplementedException();
        }
        */

        public ValueQuery<T, From> Constant(string value)
        {
            Steps.Add("constant('" + Sanitize(value) + "')");
            return new ValueQuery<T, From>(this);
        }

        public Query Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Steps.Add(string.Format("coin({0})", probability));
            return this as Query;
        }

        public Query CyclicPath()
        {
            Steps.Add("cyclicPath()");
            return this as Query;
        }

        public Query Dedup()
        {
            Steps.Add("dedup()");
            return this as Query;
        }

        public TerminalQuery<From> Drop()
        {
            Steps.Add("drop()");
            return new TerminalQuery<From>(this);
        }

        public ListQuery<T, From> Fold()
        {
            Steps.Add("fold()");
            return new ListQuery<T, From>(this);
        }

        public BooleanQuery<From> HasNext()
        {
            Steps.Add("hasNext()");
            return new BooleanQuery<From>(this);
        }

        public Query Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Steps.Add(string.Format("limit({0})", limit));
            return this as Query;
        }

        public Query Next()
        {
            Steps.Add("next()");
            return this as Query;
        }

        public Query Next(int count)
        {
            if (count < 1)
                throw new ArgumentException("Count must be greater than zero");
            Steps.Add(string.Format("next({0})", count));
            return this as Query;
        }

        public Query OrderBy(string property, bool ascending = true)
        {
            string step = "order().by('" + Sanitize(property) + "', ";
            step += ascending ? "incr" : "decr";
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        public ListQuery<T, From> Path()
        {
            Steps.Add("path()");
            return new ListQuery<T, From>(this);
        }

        public Query Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentException("Lower bound cannot be less than zero");
            if (upperBound < lowerBound)
                throw new ArgumentException("Upper bound must be greater than or equal to the lower bound");
            Steps.Add(string.Format("range({0},{1})", lowerBound, upperBound));
            return this as Query;
        }

        public Query Repeat(ITraversalQuery<IGraphOutput,IGraphOutput> traversal, int count)
        {
            if (count < 1)
                throw new ArgumentException("Repeat count must be greater than 0");
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            Steps.Add("repeat(" + traversal.ToString() + ").times(" + count.ToString() + ")");
            return this as Query;
        }

        public Query Repeat(TraversalQuery<To, To> traversal, BooleanQuery<To> condition, RepeatTypeEnum type)
        {
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            if (condition.Steps.Count < 1)
                throw new ArgumentException("Provided condition must have at least one step");
            string until = "until(" + condition.ToString() + ")";
            string repeat = "repeat(" + traversal.ToString() + ")";
            switch(type)
            {
                case RepeatTypeEnum.DoWhile:
                    Steps.Add(repeat + "." + until);
                    break;
                case RepeatTypeEnum.WhileDo:
                    Steps.Add(until + "." + repeat);
                    break;
            }   
            return this as Query;
        }

        public Query Sample(int samples)
        {
            if (samples < 1)
                throw new ArgumentException("Number of samples must be greater than zero");
            Steps.Add(string.Format("sample({0})", samples));
            return this as Query;
        }

        public DictionaryQuery<T, From> Select(params string[] items)
        {
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            string step = "select(";
            var itemList = new List<string>();
            foreach(var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<T, From>(this);
        }

        public DictionaryQuery<T, From> SelectBy(string label, params string[] items)
        {
            if (label == null || label == "")
                throw new ArgumentException("Label cannot be empty or null");
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item to select");
            string step = "select(";
            var itemList = new List<string>();
            foreach (var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            step += ".by('" + Sanitize(label) + "')";
            Steps.Add(step);
            return new DictionaryQuery<T, From>(this);
        }

        public Query SimplePath()
        {
            Steps.Add("simplePath()");
            return this as Query;
        }

        public Query Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Steps.Add(string.Format("tail({0})", limit));
            return this as Query;
        }

        public Query TimeLimit(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentException("Time must be greater than zero");
            Steps.Add(string.Format("timeLimit({0})", milliseconds));
            return this as Query;
        }

        public Query ToBulkSet()
        {
            Steps.Add("toBulkSet()");
            return this as Query;
        }

        public CollectionQuery<T, From> ToCollectionQuery()
        {
            return new CollectionQuery<T, From>(this);
        }

        public Query ToList()
        {
            Steps.Add("toList()");
            return this as Query;
        }

        public Query ToSet()
        {
            Steps.Add("toSet()");
            return this as Query;
        }
    }

    public class CollectionQuery<T, From> : CollectionQuery<T, From,IGraphOutput,CollectionQuery<T, From>>
        where From: IGraphObject
    {
        internal CollectionQuery(ITraversalQuery query) : base(query) { }

        internal CollectionQuery() : base() { }

        public CollectionQuery<GraphCollection<T>, From> CreateSubQuery()
        {
            return new CollectionQuery<GraphCollection<T>, From>();
        }
    }
}
