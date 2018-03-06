# CuriousGremlin
CuriousGremlin is a collection of .Net libraries for generating strongly-typed Gremlin queries, executing queries using Azure CosmosDB, and parsing GraphSON results.

## Quickstart
1. Generate a query:
```C#
using CuriousGremlin.Query;

var query = GraphQuery.AddVertex("label_name").Property("key_name", "value");
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
using CuriousGremlin.GraphSON;
...
foreach(var item in result)
{
	Console.WriteLine("Vertex ID: " + item.id);
	Console.WriteLine("Vertex Label: " + item.label);
}
```

## Organization
1. [Query](Query) - A library for building strongly-typed Gremlin queries.
2. [AzureCosmosDB](AzureCosmosDB) - A library for creating Gremlin clients for Azure CosmosDB
3. [GraphSON](GraphSON) - A library containing GraphSON objects
4. [UnitTests](UnitTests) - Unit tests for the project