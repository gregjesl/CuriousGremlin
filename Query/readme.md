# Query
A library for building strongly-typed queries. 

Queries can be initiated by calling `GraphQuery.Vertices()`, `GraphQuery.Vertex(...)`, `GraphQuery.AddVertex(...)`, or `GraphQuery.Edge(...)`. 

## Query Types
The following strongly-typed queries are supported:
- **GraphQuery** - Returns a collection of objects _(Abstract)_
  - **CollectionQuery** - Returns a collection of objects _(Abstract)_
    - **ElementQuery** - Returns a collection of elements _(Abstract)_
      - **VertexQuery** - Returns a collection of GraphSON verticies
      - **EdgeQuery** - Returns a collection of GraphSON edges
    - **ListQuery** - Returns a collection of enumerable lists
	  - **DictionaryQuery** - Returns a collection of dictionaries
  - **ValueQuery** - Returns a collection of values (strings, booleans, and numbers)
  - **BooleanQuery** - Returns true or false
  - **TerminalQuery** - Does not return any objects

Note that subqueries (queries that start with `__`) can be generated from a graph query or any derivative thereof. 

## Vertex Serialization

If an object implements the `IVertexObject` interface, the object can be automatically mapped to a vertex by calling `GraphQuery.AddVertex(object)`. The vertex can then be mapped back to the object using the `Vertex.Deserialize<objecttype>()` method in the [GraphSON](../GraphSON) package. 

## Steps
The following table shows the supported steps:

Step | Mapping | Implemented
---- | ------- | -----------
AddEdge | Edge | :white_check_mark:
AddVertex | Vertex | :white_check_mark:
AddProperty | Element -> Element | :white_check_mark:
AddListProperty | Vertex -> Vertex | :white_check_mark:
Aggregate | Collection -> Collection | :white_check_mark:
And | Collection -> Collection | :white_check_mark:
As | Collection -> Collection | :white_check_mark:
[Barrier](http://tinkerpop.apache.org/docs/current/reference/#barrier-step) | Collection -> Collection | :white_check_mark:
[Both](http://tinkerpop.apache.org/docs/current/reference/#vertex-steps) | Vertex -> Vertex | :white_check_mark:
[BothE](http://tinkerpop.apache.org/docs/current/reference/#vertex-steps) | Vertex -> Edge | :white_check_mark:
[BothV](http://tinkerpop.apache.org/docs/current/reference/#vertex-steps) | Edge -> Vertex | :white_check_mark:
[By](http://tinkerpop.apache.org/docs/current/reference/#by-step) | Collection -> Dictionary | :white_check_mark:
[Cap](http://tinkerpop.apache.org/docs/current/reference/#cap-step) | | :x:
[Coalesce](http://tinkerpop.apache.org/docs/current/reference/#coalesce-step) | Collection -> Collection | :white_check_mark:
[Count](http://tinkerpop.apache.org/docs/current/reference/#count-step) | Collection -> Value | :white_check_mark:
[Choose](http://tinkerpop.apache.org/docs/current/reference/#choose-step) | Collection -> Collection | :white_check_mark:
[Coin](http://tinkerpop.apache.org/docs/current/reference/#coin-step) | Collection -> Collection | :white_check_mark:
[Constant](http://tinkerpop.apache.org/docs/current/reference/#constant-step) | Collection -> Collection | :white_check_mark:
[CyclicPath](http://tinkerpop.apache.org/docs/current/reference/#cyclicpath-step) | Collection -> Collection | :white_check_mark:
[Dedup](http://tinkerpop.apache.org/docs/current/reference/#dedup-step) | Collection -> Collection | :white_check_mark:
[Drop](http://tinkerpop.apache.org/docs/current/reference/#drop-step) | Collection -> Terminal | :white_check_mark:
[Explain](http://tinkerpop.apache.org/docs/current/reference/#explain-step) | Traversal -> Terminal | :white_check_mark:
[Fold](http://tinkerpop.apache.org/docs/current/reference/#fold-step) | Collection -> List | :white_check_mark:
[Group](http://tinkerpop.apache.org/docs/current/reference/#group-step) | | :x:
[GroupCount](http://tinkerpop.apache.org/docs/current/reference/#groupcount-step) | Collection -> Dictionary | :white_check_mark:
Has | Element -> Element | :white_check_mark:
HasLabel | Element -> Element | :white_check_mark:
HasNext | Collection -> Boolean | :white_check_mark:
[Inject](http://tinkerpop.apache.org/docs/current/reference/#inject-step) | Collection -> Collection | :white_check_mark:
In | Vertex -> Vertex | :white_check_mark:
InE | Vertex -> Edge | :white_check_mark:
InV | Edge -> Vertex | :white_check_mark:
Is  | Value -> Boolean | :white_check_mark:
Limit | Collection -> Collection | :white_check_mark:
Local | Collection -> Collection | :white_check_mark:
Match | | :x:
Max | Value -> Value | :white_check_mark:
Mean | Value -> Value | :white_check_mark:
Min | Value -> Value | :white_check_mark:
Next | Collection -> Collection | :white_check_mark:
Or | Collection -> Collection | :white_check_mark:
Order | Value -> Value | :white_check_mark:
Out | Vertex -> Vertex | :white_check_mark:
OutE | Vertex -> Edge | :white_check_mark:
OutV | Edge -> Vertex | :white_check_mark:
Path | Collection -> List | :white_check_mark:
Profile | | :x:
Range | Graph -> Graph | :white_check_mark:
Repeat | Collection -> Collection | :white_check_mark:
Sack | | :x:
Sample | Collection -> Collection | :white_check_mark:
Select | Collection -> List | :white_check_mark:
SimplePath | Element -> Element | :white_check_mark:
Store | | :x:
Subgraph | | :x:
Sum | Value -> Value | :white_check_mark:
Tail | Collection -> Collection | :white_check_mark:
TimeLimit | Graph -> Graph | :x:
ToBulkSet | Collection -> Collection | :white_check_mark:
ToList | Collection -> List | :white_check_mark:
ToSet | Collection -> Collection | :white_check_mark:
Tree | | :x:
Unfold | List -> Collection | :white_check_mark:
Union | Collection -> Collection | :white_check_mark:
ValueMap | Element -> Dictionary | :white_check_mark:
Values | Element -> Value | :white_check_mark:
Vertex | Vertex | :white_check_mark:
Where | Collection -> Collection | :white_check_mark:

## Recipies

In addition to the steps listed, the following recipies are also provided:
- `_CollectionQuery_.Any()` - Creates a BooleanQuery that returns true if the collection contains at least one item and false if the collection is empty
- `VertexQuery.Find(...)` - Creates a VertexQuery that returns vertices matching the provided constraints