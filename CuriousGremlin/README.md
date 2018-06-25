#  CuriousGremlin
This project is organized as follows:
- Root directory:  library for making strongly-typed Gremlin traversal queries
- [Objects](Objects):  Contains additional classes for mapping between Vertices/Edges and C# Classes
- [Client](Client):  Contains a graph client interface as well as a client pool class.
- [CRTP](CRTP):  Contains the internal classes required to support strongly-typed queries via [CRTP](https://en.wikipedia.org/wiki/Curiously_recurring_template_pattern)

## Traversal Queries
- Vertex queries, `g.V()` in Gremlin, are started by `VertexQuery.All()`.  
- Edge queries, `g.E()` in Gremlin, are started by `EdgeQuery.All()`. 
- Vertices can be added to the graph by calling `VertexQuery.Create(...)`.  

Examples of all of the available steps can been found in the [unit tests](../CuriousGremlin.UnitTests).  

### Adding edges
Edges can be added in the following manner:
```C#
VertexQuery.All()
    .HasLabel("foo")
    .AddEdge("to", DirectionStep.To(VertexQuery.Find("bar")))
    .AddProperty("key", "value")
```
In this example, any vertex that has the label "foo" (found via `VertexQuery.All().HasLabel("foo")`) will be connected to all vertices with the label `bar` (found via `VertexQuery.Find("bar")`).  The new edges will have the label "to" and a property with a key of "key" and a value of "value". 

The direction step class can be used to map to (via `.To()`) or from (via `.From()`) a vertex or vertices.  The arguement of `To` and `From` is either the vertex ID of the vertex or a vertex traversal (as in the example above). 

### Create if not exists
A common traversal can often be "If this object exists, return it.  If it does not, create and return it.".  Here is how that is accomplished in CuriousGremlin:
```C#
var baseQuery = VertexQuery.All().HasLabel("test").Fold();
var existsQuery = baseQuery.CreateSubQuery().Unfold();
var createQuery = baseQuery.CreateSubQuery().AddVertex("test");
VertexQuery query = baseQuery.Coalesce<GraphVertex>(existsQuery, createQuery);
```
In this example, Gremlin looks for a vertex that has the label "test".  If it does not find one, then it runs the create subquery which creates the vertex.  Credit:  [Stephen Mallette on StackOverflow](https://stackoverflow.com/questions/46027444/gremlin-only-add-a-vertex-if-it-doesnt-exist)

## Vertex/Edge Mapping
To enable mapping to and from verticies, the C# class must implement the `IVertexObject` interface, located in the [CuriousGremlin.Objects](Objects) namespace. If the class implements the interface, an instance of the class can be added as by simply calling `VertexQuery.Create(myvertex)` where `myvertex` is the C# class to be mapped.  

Classes implementing the `IVertexObject` can be mapped back to the C# class in one of two ways:
- Deserialize a collection of results via `GraphVertex.Deserialize(result)` where `result` is the result of a vertex query.
- Deserialize a single vertex via `result[0].Deserialize<T>()` where `result` is the result of a vertex query and `T` is the class type to deserialize to
- Edges are deserialized in the same manner using `GraphEdge`. 

Currently, only basic class types are supported for serialization and deserialization.  Enumerations of basic types are supported and are converted to listed vertex properties.  
