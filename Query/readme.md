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
And | | :x:
As | Collection -> Collection | :white_check_mark:
Barrier | | :x:
Both | Vertex -> Vertex | :white_check_mark:
BothE | Vertex -> Edge | :white_check_mark:
BothV | Edge -> Vertex | :white_check_mark:
By | | :x:
Cap | | :x:
Coalesce | | :x:
Count | Collection -> Value | :white_check_mark:
Choose | Collection -> Graph | :white_check_mark:
Coin | Collection -> Collection | :white_check_mark:
Constant | Collection -> Collection | :white_check_mark:
CyclicPath | Collection -> Collection | :white_check_mark:
Dedup | Collection -> Collection | :white_check_mark:
Drop | Collection -> Terminal | :white_check_mark:
Fold | Collection -> List | :white_check_mark:
Group | | :x:
GroupCount | | :x:
Has | Element -> Element | :white_check_mark:
HasLabel | Element -> Element | :white_check_mark:
HasNext | Collection -> Boolean | :white_check_mark:
Inject | | :x:
In | Vertex -> Vertex | :white_check_mark:
InE | Vertex -> Edge | :white_check_mark:
InV | Edge -> Vertex | :white_check_mark:
Is  | Value -> Boolean | :white_check_mark:
Limit | Collection -> Collection | :white_check_mark:
Local | | :x:
MapKeys |  | :x:
MapValues | | :x:
Match | | :x:
Max | Value -> Value | :white_check_mark:
Mean | Value -> Value | :white_check_mark:
Min | Value -> Value | :white_check_mark:
Next | Collection -> Collection | :white_check_mark:
Or | | :x:
Order | Value -> Value | :white_check_mark:
Out | Vertex -> Vertex | :white_check_mark:
OutE | Vertex -> Edge | :white_check_mark:
OutV | Edge -> Vertex | :white_check_mark:
Path | Collection -> List | :white_check_mark:
Profile | | :x:
Range | Graph -> Graph | :white_check_mark:
Repeat | | :x:
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
Unfold | | :x:
Union | | :x:
ValueMap | | :x:
Values | Element -> Value | :white_check_mark:
Vertex | Vertex | :white_check_mark:
Where | | :x: