## Queries
The following strongly-typed queries are supported:
- **Graph** - Returns a collection of objects
  - **Vertex** - Returns a collection of GraphSON verticies
  - **Edge** - Returns a collection of GraphSON edges
  - **Value** - Returns a collection of values (strings, booleans, and numbers)
  - **FlatMap** - Returns a collection of enumerable lists
    - **Dictionary** - Returns a collection of dictionaries
- **Terminal** - Does not return any objects

## Steps
----
The following table shows the supported steps

Step | Mapping | Implemented
---- | ------- | -----------
AddEdge | -> Edge | :white_check_mark:
AddVertex | -> Vertex | :white_check_mark:
AddProperty | Vertex -> Vertex<br/>Edge -> Edge | :white_check_mark:
Aggregate | Graph -> Graph | :white_check_mark:
And | | :x:
As | Graph -> Graph | :white_check_mark:
Barrier | | :x:
By | | :x:
Cap | | :x:
Coalesce | | :x:
Count | Graph -> ValueQuery | :white_check_mark:
Choose | | :x:
Coin | Graph -> Graph | :white_check_mark:
Constant | | :x:
CyclicPath | Vertex -> Vertex<br/>Edge -> Edge | :white_check_mark:
Dedup | Graph -> Graph | :white_check_mark:
Drop | Graph -> Terminal | :white_check_mark:
Fold | Graph -> FlatMap | :white_check_mark:
Group | | :x:
GroupCount | | :x:
Has | Vertex -> Vertex<br/>Edge -> Edge | :white_check_mark:
Inject | | :x:
Is | | :x:
Limit | Graph -> Graph | :white_check_mark:
Local | | :x:
MapKeys | | :x:
MapValues | | :x:
Match | | :x:
Max | Value -> Value | :white_check_mark:
Mean | Value -> Value | :white_check_mark:
Min | Value -> Value | :white_check_mark:
Or | | :x:
Order | Value -> Value | :white_check_mark:
Path | Vertex -> FlatMap<br/>Edge -> FlatMap | :x:
Profile | | :x:
Range | Graph -> Graph | :white_check_mark:
Repeat | | :x:
Sack | | :x:
Sample | | :x:
Select | Graph -> Dictionary | :white_check_mark:
SimplePath | Vertex -> Vertex<br/>Edge -> Edge | :white_check_mark:
Store | | :x:
Subgraph | | :x:
Sum | Values -> Values | :white_check_mark:
Tail | Value -> Value | :white_check_mark:
TimeLimit | | :x:
Tree | | :x:
Unfold | FlatMap -> Graph | :white_check_mark:
Union | | :x:
ValueMap | | :x:
Values | Vertex -> Value<br/>Edge -> Value | :white_check_mark:
Vertex | -> Vertex | :white_check_mark:
Where | | :x: