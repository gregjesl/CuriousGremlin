using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin
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

        protected GraphPredicate(IEnumerable<object> items)
        {
            if (items.Count() < 1)
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

        public static GPNot Not(GraphPredicate predicate)
        {
            return new GPNot(predicate);
        }

        public static GPEquals EqualTo(object item)
        {
            return new GPEquals(item);
        }

        public static GPNotEqual NotEqualTo(object item)
        {
            return new GPNotEqual(item);
        }

        public static GPLessThan LessThan(int item)
        {
            return new GPLessThan(item);
        }

        public static GPLessThan LessThan(float item)
        {
            return new GPLessThan(item);
        }

        public static GPLessThan LessThan(double item)
        {
            return new GPLessThan(item);
        }

        public static GPLessThan LessThan(decimal item)
        {
            return new GPLessThan(item);
        }

        public static GPLessThan LessThan(long item)
        {
            return new GPLessThan(item);
        }

        public static GPLessThanOrEqualTo LessThanOrEqualTo(int item)
        {
            return new GPLessThanOrEqualTo(item);
        }

        public static GPLessThanOrEqualTo LessThanOrEqualTo(float item)
        {
            return new GPLessThanOrEqualTo(item);
        }

        public static GPLessThanOrEqualTo LessThanOrEqualTo(double item)
        {
            return new GPLessThanOrEqualTo(item);
        }

        public static GPLessThanOrEqualTo LessThanOrEqualTo(decimal item)
        {
            return new GPLessThanOrEqualTo(item);
        }

        public static GPLessThanOrEqualTo LessThanOrEqualTo(long item)
        {
            return new GPLessThanOrEqualTo(item);
        }

        public static GPGreaterThan GreaterThan(int item)
        {
            return new GPGreaterThan(item);
        }

        public static GPGreaterThan GreaterThan(float item)
        {
            return new GPGreaterThan(item);
        }

        public static GPGreaterThan GreaterThan(double item)
        {
            return new GPGreaterThan(item);
        }

        public static GPGreaterThan GreaterThan(decimal item)
        {
            return new GPGreaterThan(item);
        }

        public static GPGreaterThan GreaterThan(long item)
        {
            return new GPGreaterThan(item);
        }

        public static GPGreaterThanOrEqualTo GreaterThanOrEqualTo(int item)
        {
            return new GPGreaterThanOrEqualTo(item);
        }

        public static GPGreaterThanOrEqualTo GreaterThanOrEqualTo(float item)
        {
            return new GPGreaterThanOrEqualTo(item);
        }

        public static GPGreaterThanOrEqualTo GreaterThanOrEqualTo(double item)
        {
            return new GPGreaterThanOrEqualTo(item);
        }

        public static GPGreaterThanOrEqualTo GreaterThanOrEqualTo(decimal item)
        {
            return new GPGreaterThanOrEqualTo(item);
        }

        public static GPGreaterThanOrEqualTo GreaterThanOrEqualTo(long item)
        {
            return new GPGreaterThanOrEqualTo(item);
        }

        public static GPInside Inside(int lower, int upper)
        {
            return new GPInside(lower, upper);
        }

        public static GPInside Inside(float lower, float upper)
        {
            return new GPInside(lower, upper);
        }

        public static GPInside Inside(double lower, double upper)
        {
            return new GPInside(lower, upper);
        }

        public static GPInside Inside(decimal lower, decimal upper)
        {
            return new GPInside(lower, upper);
        }

        public static GPInside Inside(long lower, long upper)
        {
            return new GPInside(lower, upper);
        }

        public static GPOutside Outside(int lower, int upper)
        {
            return new GPOutside(lower, upper);
        }

        public static GPOutside Outside(float lower, float upper)
        {
            return new GPOutside(lower, upper);
        }

        public static GPOutside Outside(double lower, double upper)
        {
            return new GPOutside(lower, upper);
        }

        public static GPOutside Outsidee(decimal lower, decimal upper)
        {
            return new GPOutside(lower, upper);
        }

        public static GPOutside Outside(long lower, long upper)
        {
            return new GPOutside(lower, upper);
        }

        public static GPWithin Within(IEnumerable items)
        {
            return new GPWithin(items);
        }

        public static GPWithout Without(IEnumerable items)
        {
            return new GPWithout(items);
        }
    }

    public class GPNot : GraphPredicate
    {
        protected override string Command { get { return "not"; }}
        internal GPNot(GraphPredicate predicate) : base() 
        {
            Predicate = Command + "(" + predicate.ToString() + ")";
        }
	}

    public class GPEquals : GraphPredicate
    {
        protected override string Command { get { return "eq"; } }
        internal GPEquals(object item) : base(item) { }
    }

    public class GPNotEqual : GraphPredicate
    {
        protected override string Command { get { return "neq"; } }
        internal GPNotEqual(object item) : base(item) { }
    }

    public class GPLessThan : GraphPredicate
    {
        protected override string Command { get { return "lt"; } }
        internal GPLessThan(int item) : base(item) { }
        internal GPLessThan(float item) : base(item) { }
        internal GPLessThan(double item) : base(item) { }
        internal GPLessThan(decimal item) : base(item) { }
        internal GPLessThan(long item) : base(item) { }
    }

    public class GPLessThanOrEqualTo : GraphPredicate
    {
        protected override string Command { get { return "lte"; } }
        internal GPLessThanOrEqualTo(int item) : base(item) { }
        internal GPLessThanOrEqualTo(float item) : base(item) { }
        internal GPLessThanOrEqualTo(double item) : base(item) { }
        internal GPLessThanOrEqualTo(decimal item) : base(item) { }
        internal GPLessThanOrEqualTo(long item) : base(item) { }
    }

    public class GPGreaterThan : GraphPredicate
    {
        protected override string Command { get { return "gt"; } }
        internal GPGreaterThan(int item) : base(item) { }
        internal GPGreaterThan(float item) : base(item) { }
        internal GPGreaterThan(double item) : base(item) { }
        internal GPGreaterThan(decimal item) : base(item) { }
        internal GPGreaterThan(long item) : base(item) { }
    }

    public class GPGreaterThanOrEqualTo : GraphPredicate
    {
        protected override string Command { get { return "gte"; } }
        internal GPGreaterThanOrEqualTo(int item) : base(item) { }
        internal GPGreaterThanOrEqualTo(float item) : base(item) { }
        internal GPGreaterThanOrEqualTo(double item) : base(item) { }
        internal GPGreaterThanOrEqualTo(decimal item) : base(item) { }
        internal GPGreaterThanOrEqualTo(long item) : base(item) { }
    }

    public class GPInside : GraphPredicate
    {
        protected override string Command { get { return "inside"; } }
        internal GPInside(int lower, int upper) : base(lower, upper) { }
        internal GPInside(float lower, float upper) : base(lower, upper) { }
        internal GPInside(double lower, double upper) : base(lower, upper) { }
        internal GPInside(decimal lower, decimal upper) : base(lower, upper) { }
        internal GPInside(long lower, long upper) : base(lower, upper) { }
    }

    public class GPOutside : GraphPredicate
    {
        protected override string Command { get { return "outside"; } }
        internal GPOutside(int lower, int upper) : base(lower, upper) { }
        internal GPOutside(float lower, float upper) : base(lower, upper) { }
        internal GPOutside(double lower, double upper) : base(lower, upper) { }
        internal GPOutside(decimal lower, decimal upper) : base(lower, upper) { }
        internal GPOutside(long lower, long upper) : base(lower, upper) { }
    }

    public class GPBetween : GraphPredicate
    {
        protected override string Command { get { return "between"; } }
        internal GPBetween(int lower, int upper) : base(lower, upper) { }
        internal GPBetween(float lower, float upper) : base(lower, upper) { }
        internal GPBetween(double lower, double upper) : base(lower, upper) { }
        internal GPBetween(decimal lower, decimal upper) : base(lower, upper) { }
        internal GPBetween(long lower, long upper) : base(lower, upper) { }
    }

    public abstract class GraphPredicateList : GraphPredicate
    {
        protected GraphPredicateList(IEnumerable items) : base()
        {
            List<string> itemStrings = new List<string>();
            foreach (var item in items)
                itemStrings.Add(GraphQuery.GetObjectString(item));
            if (itemStrings.Count < 1)
                throw new ArgumentException("Must have at least one item");
            Predicate = Command + "(" + string.Join(", ", itemStrings) + ")";
        }
    }

    public class GPWithin : GraphPredicateList
    {
        protected override string Command { get { return "within"; } }
        internal GPWithin(IEnumerable items) : base(items) { }
    }

    public class GPWithout : GraphPredicateList
    {
        protected override string Command { get { return "without"; } }
        internal GPWithout(IEnumerable items) : base(items) { }
    }
}
