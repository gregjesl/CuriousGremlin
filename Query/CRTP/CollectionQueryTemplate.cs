using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using CuriousGremlin.Query.Objects;
using CuriousGremlin.Query.Predicates;

namespace CuriousGremlin.Query.CRTP
{
    public class CollectionQueryTemplate<T, From, Query> : TraversalQuery<From, T>
        where Query : CollectionQueryTemplate<T, From, Query>
    {
        protected CollectionQueryTemplate(ITraversalQuery<From> query)
        {
            if (query is null)
                throw new ArgumentException("Step list cannot be null");
            Steps = query.Steps;
        }

        protected CollectionQueryTemplate() : base() { }

        public static implicit operator CollectionQueryTemplate<T, From, Query>(CollectionQuery<T, From> query)
        {
            return new CollectionQueryTemplate<T, From, Query>(query);
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="label">The label of the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(string label)
        {
            return AddVertex(label, new Dictionary<string, object>());
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="properties">The properties to add to the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(Dictionary<string, object> properties)
        {
            return AddVertex(null, properties);
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="label">The label of the new vertex/vertices</param>
        /// <param name="properties">The properties to add to the new vertex/vertices</param>
        public VertexQuery<From> AddVertex(string label, Dictionary<string, object> properties)
        {
            string step = "addV(";
            if (label != null && label != "")
                step += "'" + Sanitize(label) + "'";

            if (properties.Count > 0)
            {
                step += ", " + SeralizeProperties(properties);
            }
            step += ")";
            Steps.Add(step);
            return new VertexQuery<From>(this);
        }

        /// <summary>
        /// For every incoming object, a vertex is created. 
        /// </summary>
        /// <param name="vertex">The object to be serialized</param>
        public VertexQuery<From> AddVertex(IVertexObject vertex)
        {
            var properties = JObject.FromObject(vertex).ToObject<Dictionary<string, object>>();
            foreach (var item in properties)
            {
                if (item.Value is null)
                    properties.Remove(item.Key);
            }
            properties.Remove(nameof(vertex.VertexLabel));
            return AddVertex(vertex.VertexLabel, properties);
        }

        /// <summary>
        /// Aggregates all the objects at a particular point of traversal into a Collection using eager evaluation
        /// </summary>
        /// <param name="label">The label of the aggregated collection</param>
        public Query Aggregate(string label)
        {
            Steps.Add("aggregate('" + Sanitize(label) + "')");
            return this as Query;
        }

        /// <summary>
        /// A traverser is returned if and only if all conditons yield a result
        /// </summary>
        public Query And(IEnumerable<ITraversalQuery<T, T>> conditions)
        {
            return And(conditions.ToArray()) as Query;
        }

        /// <summary>
        /// A traverser is returned if and only if all conditons yield a result
        /// </summary>
        public Query And(params ITraversalQuery<T, T>[] conditions)
        {
            return And<T>(conditions.ToArray()) as Query;
        }

        /// <summary>
        /// A traverser is returned if and only if all conditons yield a result
        /// </summary>
        public CollectionQuery<TOutput, From> And<TOutput>(IEnumerable<ITraversalQuery<T, TOutput>> conditions)
        {
            return And(conditions.ToArray());
        }

        /// <summary>
        /// A traverser is returned if and only if all conditons yield a result
        /// </summary>
        public CollectionQuery<TOutput, From> And<TOutput>(params ITraversalQuery<T, TOutput>[] conditions)
        {
            if (conditions.Length == 0)
                throw new ArgumentNullException("Must provide at least one condition");
            string step = "and(";
            List<string> stepStrings = new List<string>();
            foreach (var condition in conditions)
            {
                stepStrings.Add(condition.ToString());
            }
            step += string.Join(",", stepStrings);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        /// <summary>
        /// A traverser is returned if and only if all conditons yield a result
        /// </summary>
        public Query And<TOutput>(IEnumerable<ITraversalQuery<From, TOutput>> conditions)
        {
            return And(conditions.ToArray());
        }

        /// <summary>
        /// Aggregates all the objects at a particular point of traversal into a Collection
        /// <seealso cref="Aggregate(string)"/>
        /// </summary>
        /// <param name="label">The label of the collection</param>
        public Query As(string label)
        {
            Steps.Add("as('" + Sanitize(label) + "')");
            return this as Query;
        }

        /// <summary>
        /// Turns the lazy traversal pipeline into a bulk-synchronous pipeline
        /// </summary>
        public Query Barrier()
        {
            Steps.Add("barrier()");
            return this as Query;
        }

        /*
        public StringQuery<From> By(string name)
        {
            Steps.Add("by('" + Sanitize(name) + "')");
            return new StringQuery<From>(this);
        }

        public DictionaryQuery<long, List<T>, From> ByCount()
        {
            Steps.Add("by(outE().count())");
            return new DictionaryQuery<long, List<T>, From>(this);
        }


        public DictionaryQuery<long, T> ByEdge()
        {
            Steps.Add("by(outE().count())");
            return new DictionaryQuery<long, T>(this);
        }
        */

        /// <summary>
        /// Routes the current traverser to a particular traversal branch option
        /// </summary>
        /// <returns>The choose.</returns>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="TrueQuery">Path executed if the condition yields a result</param>
        /// <param name="FalseQuery">Path executed if the condition does not yields a result</param>
        public CollectionQuery<T, From> Choose(ITraversalQuery<T> condition, ITraversalQuery<T, T> TrueQuery, ITraversalQuery<T, T> FalseQuery)
        {
            Steps.Add("choose(" + condition.ToString() + ", __." + TrueQuery.ToString() + ", __." + FalseQuery + ")");
            return new CollectionQuery<T, From>(this);
        }

        /// <summary>
        /// Routes the current traverser to a particular traversal branch option
        /// </summary>
        /// <returns>The choose.</returns>
        /// <param name="condition">Condition to evaluate</param>
        /// <param name="TrueQuery">Path executed if the condition yields a result</param>
        /// <param name="FalseQuery">Path executed if the condition does not yields a result</param>
        public CollectionQuery<TOutput, From> Choose<TOutput>(ITraversalQuery<T> condition, ITraversalQuery<T, TOutput> TrueQuery, ITraversalQuery<T, TOutput> FalseQuery)
        {
            Steps.Add("choose(" + condition.ToString() + ", __." + TrueQuery.ToString() + ", __." + FalseQuery + ")");
            return new CollectionQuery<TOutput, From>(this);
        }

        /// <summary>
        /// Evaluates the provided traversals in order and returns the first traversal that emits at least one element
        /// </summary>
        /// <param name="paths">Paths to evaluate</param>
        public Query Coalesce(params ITraversalQuery<T, T>[] paths)
        {
            string step = "coalesce(";
            List<string> pathStrings = new List<string>();
            foreach (var path in paths)
            {
                pathStrings.Add(path.ToString());
            }
            step += string.Join(",", pathStrings);
            step += ")";
            Steps.Add(step);
            return this as Query;
        }

        /// <summary>
        /// Evaluates the provided traversals in order and returns the first traversal that emits at least one element
        /// </summary>
        /// <param name="paths">Paths to evaluate</param>
        public CollectionQuery<TOutput, From> Coalesce<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
        {
            string step = "coalesce(";
            List<string> pathStrings = new List<string>();
            foreach (var path in paths)
            {
                pathStrings.Add(path.ToString());
            }
            step += string.Join(",", pathStrings);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        /*
         * Currently not supported by Azure CosmosDB
         * 
        /// <summary>
        /// Randomly filters out a traverser
        /// </summary>
        /// <returns>Probability of a traverser being allowed to pass</returns>
        /// <param name="probability">Probability.</param>
        public Query Coin(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
                throw new ArgumentException("Probability must be between 0 and 1");
            Steps.Add(string.Format("coin({0:0.0000})", probability));
            return this as Query;
        }
        */

        /// <summary>
        /// Specifies a constant value
        /// </summary>
        public ValueQuery<TOutput, From> Constant<TOutput>(TOutput value)
        {
            Steps.Add("constant(" + GetObjectString(value) + ")");
            return new ValueQuery<TOutput, From>(this);
        }

        /// <summary>
        /// Counts the total number of represented traversers in the streams (i.e. the bulk count)
        /// </summary>
        /// <returns>The count.</returns>
        public IntegerQuery<From> Count()
        {
            Steps.Add("count()");
            return new IntegerQuery<From>(this);
        }

        /// <summary>
        /// Creates a sub query
        /// </summary>
        /// <returns>The sub query</returns>
        public CollectionQuery<T, T> CreateSubQuery()
        {
            return new CollectionQuery<T, T>();
        }

        /// <summary>
        /// Removes traversers that do not have a cyclic path
        /// </summary>
        public Query CyclicPath()
        {
            Steps.Add("cyclicPath()");
            return this as Query;
        }

        /// <summary>
        /// Removes repeatedly seen objects from the traversal stream
        /// </summary>
        public Query Dedup()
        {
            Steps.Add("dedup()");
            return this as Query;
        }

        /// <summary>
        /// Removes the element from the graph
        /// </summary>
        /// <returns>The drop.</returns>
        public TerminalQuery<From> Drop()
        {
            Steps.Add("drop()");
            return new TerminalQuery<From>(this);
        }

        /// <summary>
        /// Aggregates the output of the traversers into a list
        /// <seealso cref="ListQuery{T, From, Query}.Unfold"/>
        /// </summary>
        /// <returns>The fold.</returns>
        public ListQuery<T, From> Fold()
        {
            Steps.Add("fold()");
            return new ListQuery<T, From>(this);
        }

        /// <summary>
        /// Groups elements by the specified property
        /// </summary>
        /// <param name="property">The key of the property to group by</param>
        public DictionaryQuery<string, T, From> Group(string property)
        {
            Steps.Add("group().by('" + Sanitize(property) + "')");
            return new DictionaryQuery<string, T, From>(this);
        }

        /*
        public Query HasNext()
        {
            Steps.Add("hasNext()");
            return this as Query;
        }
        */

        /// <summary>
        /// Returns up to the specified number of results
        /// </summary>
        /// <param name="limit">The maximum number of items to return</param>
        public Query Limit(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit must be at least 0");
            Steps.Add(string.Format("limit({0})", limit));
            return this as Query;
        }

        /// <summary>
        /// Executes the provided query on each incoming item
        /// </summary>
        /// <param name="localQuery">The query to execute on each incoming item</param>
        public CollectionQuery<TOutput, From> Local<TOutput>(ITraversalQuery<T, TOutput> localQuery)
        {
            Steps.Add("local(" + localQuery.ToString() + ")");
            return new CollectionQuery<TOutput, From>(this);
        }

        /*
        public Query Next()
        {
            Steps.Add("next()");
            return this as Query;
        }
        */

        /*
        public Query Next(int count)
        {
            if (count < 1)
                throw new ArgumentException("Count must be greater than zero");
            Steps.Add(string.Format("next({0})", count));
            return this as Query;
        }
        */

        /// <summary>
        /// Removes objects from the traversal stream when the traversal provided as an argument does not return any objects
        /// </summary>
        public Query Not(ITraversalQuery<T, T> subquery)
        {
            Steps.Add("not(" + subquery.ToString() + ")");
            return this as Query;
        }

        /// <summary>
        /// Returns the result of the specified traversal if it yields a result else it returns the calling element
        /// </summary>
        public Query Optional(ITraversalQuery<T, T> subquery)
        {
            Steps.Add("optional(" + subquery.ToString() + ")");
            return this as Query;
        }

        /// <summary>
        /// Ensures that at least one of the provided traversals yield a result
        /// </summary>
        public CollectionQuery<TOutput, From> Or<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
        {
            string step = "or(";
            var pathList = new List<string>();
            foreach (var path in paths)
            {
                pathList.Add("__." + path.ToString());
            }
            step += string.Join(", ", pathList);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        /// <summary>
        /// Returns the history of the traverser
        /// </summary>
        public ListQuery<object, From> Path()
        {
            Steps.Add("path()");
            return new ListQuery<object, From>(this);
        }

        /// <summary>
        /// Properties the map.
        /// </summary>
        /// <returns>The map.</returns>
        public DictionaryQuery<string, object, From> PropertyMap()
        {
            Steps.Add("propertyMap()");
            return new DictionaryQuery<string, object, From>(this);
        }

        /*
        public DictionaryQuery<string, object, From> PropertyMap(params string[] keys)
        {
            string step = "propertyMap(";
            var itemList = new List<string>();
            foreach (var item in keys)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<string, object, From>(this);
        }
        */

        /// <summary>
        /// Limits the results to the elements in the specified range
        /// </summary>
        /// <param name="lowerBound">The number of items to skip</param>
        /// <param name="upperBound">The maximum number of items before the lower bound is executed</param>
        public Query Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentException("Lower bound cannot be less than zero");
            if (upperBound < lowerBound)
                throw new ArgumentException("Upper bound must be greater than or equal to the lower bound");
            Steps.Add(string.Format("range({0},{1})", lowerBound, upperBound));
            return this as Query;
        }

        /// <summary>
        /// Repeats the provided traversal
        /// </summary>
        /// <param name="traversal">The sub query to repeat</param>
        /// <param name="count">The number of times to repeat the sub query</param>
        public CollectionQuery<TOutput, From> Repeat<TOutput>(ITraversalQuery<T, TOutput> traversal, int count)
        {
            if (count < 1)
                throw new ArgumentException("Repeat count must be greater than 0");
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            Steps.Add("repeat(" + traversal.ToString() + ").times(" + count.ToString() + ")");
            return new CollectionQuery<TOutput, From>(this);
        }

        /// <summary>
        /// Repeats the provided traversal until the provided condition is met
        /// </summary>
        /// <returns>The repeat.</returns>
        /// <param name="traversal">The sub query to repeat</param>
        /// <param name="condition">The condition that must be met to end the loop</param>
        /// <param name="type">Specifies a do-while or while-do loop</param>
        public Query Repeat<TOutput>(ITraversalQuery<T, TOutput> traversal, ITraversalQuery<T> condition, RepeatTypeEnum type)
        {
            if (traversal.Steps.Count < 1)
                throw new ArgumentException("Provided traversal must contain at least one step");
            if (condition.Steps.Count < 1)
                throw new ArgumentException("Provided condition must have at least one step");
            string until = "until(" + condition.ToString() + ")";
            string repeat = "repeat(" + traversal.ToString() + ")";
            switch (type)
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

        /// <summary>
        /// Sampes the specified number of samples from the results
        /// </summary>
        /// <param name="samples">The maximum number of items to return</param>
        public Query Sample(int samples)
        {
            if (samples < 1)
                throw new ArgumentException("Number of samples must be greater than zero");
            Steps.Add(string.Format("sample({0})", samples));
            return this as Query;
        }

        /// <summary>
        /// Returns a collection that was previously specified
        /// </summary>
        /// <param name="items">The labels of collections to return</param>
        public DictionaryQuery<string, T, From> Select(string item1, string item2, params string[] items)
        {
            if (string.IsNullOrEmpty(item1) || string.IsNullOrEmpty(item2))
                throw new ArgumentNullException("Input cannot be null or empty");
            string step = "select(";
            var itemList = new List<string>();
            itemList.Add("'" + Sanitize(item1) + "'");
            itemList.Add("'" + Sanitize(item2) + "'");
            foreach (var item in items)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new DictionaryQuery<string, T, From>(this);
        }

        /// <summary>
        /// Returns the values of the specified key, grouped by the provided colleciton labels
        /// </summary>
        /// <returns>The by.</returns>
        /// <param name="key">The key of the property values to return</param>
        /// <param name="labels">The label(s) of aggregated collections</param>
        public DictionaryQuery<string, T, From> SelectBy(string key, IEnumerable<string> labels)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Label cannot be empty or null");
            string step = "select(";
            var itemList = new List<string>();
            foreach (var item in labels)
            {
                itemList.Add("'" + Sanitize(item) + "'");
            }
            step += string.Join(", ", itemList);
            step += ")";
            step += ".by('" + Sanitize(key) + "')";
            Steps.Add(step);
            return new DictionaryQuery<string, T, From>(this);
        }

        /// <summary>
        /// Removes traversers that repeat a path
        /// </summary>
        /// <returns>The path.</returns>
        public Query SimplePath()
        {
            Steps.Add("simplePath()");
            return this as Query;
        }

        /*
         * Currently not supported by CosmosDB
         * 
        /// <summary>
        /// Skips the specified number of results before returning the results
        /// </summary>
        /// <param name="number">Number of items to skip</param>
        public Query Skip(int number)
        {
            Steps.Add("skip(" + number.ToString() + ")");
            return this as Query;
        }
        */

        /// <summary>
        /// Returns the tail of a result collection
        /// </summary>
        /// <param name="limit">The number of items to return</param>
        public Query Tail(int limit)
        {
            if (limit < 0)
                throw new ArgumentException("Limit cannot be less than zero");
            Steps.Add(string.Format("tail({0})", limit));
            return this as Query;
        }

        /// <summary>
        /// Limits the execution time
        /// </summary>
        /// <param name="milliseconds">Number of milliseconds to limit the execution to</param>
        public Query TimeLimit(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentException("Time must be greater than zero");
            Steps.Add(string.Format("timeLimit({0})", milliseconds));
            return this as Query;
        }

        /// <summary>
        /// Tos the bulk set.
        /// </summary>
        /// <returns>The bulk set.</returns>
        public Query ToBulkSet()
        {
            Steps.Add("toBulkSet()");
            return this as Query;
        }

        /*
        public Query ToList()
        {
            Steps.Add("toList()");
            return this as Query;
        }
        */

        /// <summary>
        /// Returns all results in a set (duplicates are removed). Execution is terminal. 
        /// </summary>
        /// <returns>The set.</returns>
        public Query ToSet()
        {
            Steps.Add("toSet()");
            return this as Query;
        }

        /// <summary>
        /// Merges the results of the provided sub queries
        /// </summary>
        public CollectionQuery<TOutput, From> Union<TOutput>(params ITraversalQuery<T, TOutput>[] paths)
        {
            if (paths.Length < 1)
                throw new ArgumentException("Must have at least one path");
            string step = "union(";
            var itemList = new List<string>();
            foreach (var item in paths)
            {
                itemList.Add("__." + item.ToString());
            }
            step += string.Join(", ", itemList);
            step += ")";
            Steps.Add(step);
            return new CollectionQuery<TOutput, From>(this);
        }

        /// <summary>
        /// Filters results based on a sub query
        /// </summary>
        public Query Where(ITraversalQuery<T> condition)
        {
            Steps.Add("where(" + condition.ToString() + ")");
            return this as Query;
        }

        /// <summary>
        /// Filters results based on a sub query
        /// </summary>
        public Query Where(GraphPredicate condition)
        {
            Steps.Add("where(" + condition.ToString() + ")");
            return this as Query;
        }
    }
}
