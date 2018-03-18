# Query
A library for building strongly-typed Gremlin queries. __TinkerPop Version: 3.3.1__

Queries can be initiated by calling `VertexQuery.All()`, `GraphVertex.Vertex(...)`, or `GraphVertex.AddVertex(...)`. 

## Structure
- **GraphQuery** - Base class for queries
  - **TraversalQuery** - Base class for a traversal query
    - **CollectionQuery** - Returns a collection of objects
      - **ElementQuery** - Returns a collection of elements
        - **VertexQuery** - Returns a collection of vertices
        - **EdgeQuery** - Returns a collection of edges
      - **ValueQuery** - Returns a collection of values (strings, numbers, and/or booleans)
        - **StringQuery** - Returns a collection of strings
        - **IntegerQuery** - Returns a colleection of integers
      - **ListQuery** - Returns a collection of lists
        - **DictionaryQuery** - Returns a collection of dictionaries
    - **TerminalQuery** - A query where no additional steps are possible

## Vertex Serialization

If an object implements the `IVertexObject` interface, the object can be automatically mapped to a vertex by calling `GraphQuery.AddVertex(object)`. The vertex can then be mapped back to the object using the `Vertex.Deserialize<objecttype>()` method in the [GraphSON](../GraphSON) package. 

## Steps
The list of steps available can be found [on TinkerPop's documentation website](http://tinkerpop.apache.org/docs/3.3.1/reference/#graph-traversal-steps). The following steps are not supported at this time:
- Cap
- Emit
- From
- Loops
- Match
- PageRank
- PeerPressure
- Profile
- Project
- Program
- Sack
- Store
- Subgraph
- Tree
- Unfold
- Value

## Recipies

In addition to the steps listed, the following recipies are also provided:
- `_CollectionQuery_.Any()` - Creates a BooleanQuery that returns true if the collection contains at least one item and false if the collection is empty
- `VertexQuery.Find(...)` - Creates a VertexQuery that returns vertices matching the provided constraints

## Version
This library was built on [TinkerPop Version 3.3.1](http://tinkerpop.apache.org/docs/3.3.1/reference/)