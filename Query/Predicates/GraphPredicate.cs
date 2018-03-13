using System;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Query.Predicates
{
    public abstract class GraphPredicate : GraphQuery
    {
        protected string Predicate;
        protected abstract string Command { get; }

        protected GraphPredicate(object item)
        {
            Predicate = Command + "(" + GraphQuery.GetObjectString(item) + ")";
        }

        protected GraphPredicate() { }

        protected GraphPredicate(object lb, object up)
        {
            Predicate = Command + "(" + GraphQuery.GetObjectString(lb) + ", " + GraphQuery.GetObjectString(up) + ")";
        }

        protected GraphPredicate(params object[] items)
        {
            if (items.Length < 1)
                throw new ArgumentException("Must have at least one item");
            List<string> itemStrings = new List<string>();
            foreach (var item in items)
                itemStrings.Add(GraphQuery.GetObjectString(item));
            Predicate = Command + "(" + string.Join(", ", itemStrings) + ")";
        }

        public override bool Equals(object obj)
        {
            return (string)obj == Predicate;
        }

        public override int GetHashCode()
        {
            return Predicate.GetHashCode();
        }

        public override string ToString()
        {
            return Predicate;
        }
    }

    public class GPNot : GraphPredicate
    {
        protected override string Command { get { return "not"; }}
        public GPNot(GraphPredicate predicate) : base(predicate) { }
	}

    public class GPEquals : GraphPredicate
    {
        protected override string Command { get { return "eq"; } }
        public GPEquals(object item) : base(item) { }
    }

    public class GPNotEqual : GraphPredicate
    {
        protected override string Command { get { return "neq"; } }
        public GPNotEqual(object item) : base(item) { }
    }

    public class GPLessThan : GraphPredicate
    {
        protected override string Command { get { return "lt"; } }
        public GPLessThan(int item) : base(item) { }
        public GPLessThan(float item) : base(item) { }
        public GPLessThan(double item) : base(item) { }
        public GPLessThan(decimal item) : base(item) { }
        public GPLessThan(long item) : base(item) { }
    }

    public class GPLessThanOrEqualTo : GraphPredicate
    {
        protected override string Command { get { return "lte"; } }
        public GPLessThanOrEqualTo(int item) : base(item) { }
        public GPLessThanOrEqualTo(float item) : base(item) { }
        public GPLessThanOrEqualTo(double item) : base(item) { }
        public GPLessThanOrEqualTo(decimal item) : base(item) { }
        public GPLessThanOrEqualTo(long item) : base(item) { }
    }

    public class GPGreaterThan : GraphPredicate
    {
        protected override string Command { get { return "gt"; } }
        public GPGreaterThan(int item) : base(item) { }
        public GPGreaterThan(float item) : base(item) { }
        public GPGreaterThan(double item) : base(item) { }
        public GPGreaterThan(decimal item) : base(item) { }
        public GPGreaterThan(long item) : base(item) { }
    }

    public class GPGreaterThanOrEqualTo : GraphPredicate
    {
        protected override string Command { get { return "gte"; } }
        public GPGreaterThanOrEqualTo(int item) : base(item) { }
        public GPGreaterThanOrEqualTo(float item) : base(item) { }
        public GPGreaterThanOrEqualTo(double item) : base(item) { }
        public GPGreaterThanOrEqualTo(decimal item) : base(item) { }
        public GPGreaterThanOrEqualTo(long item) : base(item) { }
    }

    public class GPInside : GraphPredicate
    {
        protected override string Command { get { return "inside"; } }
        public GPInside(int lower, int upper) : base(lower, upper) { }
        public GPInside(float lower, float upper) : base(lower, upper) { }
        public GPInside(double lower, double upper) : base(lower, upper) { }
        public GPInside(decimal lower, decimal upper) : base(lower, upper) { }
        public GPInside(long lower, long upper) : base(lower, upper) { }
    }

    public class GPOutside : GraphPredicate
    {
        protected override string Command { get { return "outside"; } }
        public GPOutside(int lower, int upper) : base(lower, upper) { }
        public GPOutside(float lower, float upper) : base(lower, upper) { }
        public GPOutside(double lower, double upper) : base(lower, upper) { }
        public GPOutside(decimal lower, decimal upper) : base(lower, upper) { }
        public GPOutside(long lower, long upper) : base(lower, upper) { }
    }

    public class GPBetween : GraphPredicate
    {
        protected override string Command { get { return "between"; } }
        public GPBetween(int lower, int upper) : base(lower, upper) { }
        public GPBetween(float lower, float upper) : base(lower, upper) { }
        public GPBetween(double lower, double upper) : base(lower, upper) { }
        public GPBetween(decimal lower, decimal upper) : base(lower, upper) { }
        public GPBetween(long lower, long upper) : base(lower, upper) { }
    }

    public class GPWithin : GraphPredicate
    {
        protected override string Command { get { return "within"; } }
        public GPWithin(params object[] items) : base(items) { }
    }

    public class GPWithout : GraphPredicate
    {
        protected override string Command { get { return "without"; } }
        public GPWithout(params object[] items) : base(items) { }
    }
}
