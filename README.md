[![Build status](https://ci.appveyor.com/api/projects/status/2i07jnv3pxll8r5u/branch/master?svg=true)](https://ci.appveyor.com/project/gregjesl/curiousgremlin/branch/master)

# CuriousGremlin
CuriousGremlin is an alternative to [Gremlin.Net](http://tinkerpop.apache.org/docs/current/reference/#gremlin-DotNet) that offers:
- Strongly-typed Gremlin queries
- Automatic mapping between C# classes and graph objects (Vertices and Edges)
- Graph client pooling
- Azure CosmosDB support
- .NET Framework and .NET Core support

## Quickstart
1. Generate a query:
```C#
using CuriousGremlin;

var query = VertexQuery.Create("label_name").Property("key_name", "value");
```
2. Execute the query:
```C#
using CuriousGremlin.AzureCosmosDB;
...
var client = new GraphClient(endpoint, authKey);
using(client)
{
	await client.Open("database_name", "collection_name");
	var result = client.Execute(query);
}
```
3. Manipulate results
```C#
using CuriousGremlin.Objects;
...
foreach(GraphVertex item in result)
{
	Console.WriteLine("Vertex ID: " + item.id);
	Console.WriteLine("Vertex Label: " + item.label);
}
```

## Organization
1. [CuriousGremlin](CuriousGremlin) - .NET Standard library for strongly-typed Gremlin queries
2. [CuriousGremlin.AzureCosmosDB](CuriousGremlin.AzureCosmosDB) - A shared library containing a client for Azure CosmosDB used in the following two projects. 
3. [CuriousGremlin.AzureCosmosDB.Framework](CuriousGremlin.AzureCosmosDB.Framework) - .NET Framework library for interfacing with Azure CosmosDB (using the Gremlin.Net NuGet package)
4. [CuriousGremlin.AzureCosmosDB.Core](CuriousGremlin.AzureCosmosDB.Core) - .NET Core library for interfacing with Azure CosmosDB (uses Microsoft Azure CosmosDB NuGet package)
5. [CuriousGremlin.UnitTests] - Shared library containing unit tests
6. [CuriousGremlin.UnitTests.Framework](CuriousGremlin.UnitTests.Framework) - Implementation of unit tests using the [Azure CosmosDB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)

## Testing
In order to run the unit tests, the [Azure CosmosDB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) must be installed and running on the local machine.  The unit tests assume [default credentials](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator#authenticating-requests) when running the tests.  
