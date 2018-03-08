# AzureCosmosDB
A library for interfacing with the Graph API of Azure CosmosDB

## GraphClient

A GraphClient can be instantiated by providing the CosmosDB endpoint and Auth Key to the constructor. _**Note that GraphClient is disposeable and should be disposed when no longer needed**_.

## Databases

The `GraphClient` class offers the following methods for creating databases:
- `CreateDatabaseAsync(string id)` - Creates a new database. This method will throw an exception if a database with the same id already exists. 
- `CreateDatabaseIfNotExistsAsync(string id)` - Creates a new database if one with the specified id does not exists; otherwise, it returns the database with the specified id. 

Databases can be deleted by calling `DeleteDatabaseAsync(string id)`. This method will throw an exception if a database with the specified id does not exist. 

## Collections
The `GraphClient` class offers the following methods for creating collections inside of a database:
- `CreateDocumentCollectionAsync(string database, string collection)` - Creates a new collection in the specified database. This method will throw an exception if a collection of the same name already exists. 
- `CreateDocumentCollectionIfNotExistsAsync(string database, string collection)` - Creates a new collection in the specified database if a collection of the same name does not exist; otherwise, it returns the collection with the specified name. 

Collections can be deleted by calling `DeleteCollectionAsync(string database, string collection)`. This method will throw an exception if a collection with the specified name does not exist. 

## Executing Queries

The `GraphClient` class provides an `Execute` method for executing queries. The method is overloaded for the different query types available in the [Query](../Query) package. 

## Client Pool

Clients can also be retrieved from a pool of clients. A client pool is instantiated by calling `GraphClient.CreatePool(string endpoint, string authKey, string database, string graph)`. Once instantiated, a client can be retrieved by calling `GraphClient.FromPool()`. _**Note that the client pool should be disposed when no longer needed by calling `GraphClient.DisposePool()`**_.