using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net.Driver;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string database = "wakecloud";
            string collection = "v2";
            var gremlinServer = new GremlinServer("wakegraph.gremlin.cosmosdb.azure.com", 443, enableSsl: true,
                                                username: "/dbs/" + database + "/colls/" + collection,
                                                password: "zvxisVParScjkxwSWoTyRfyxQY6UxNsGU5s3JcRO0USEVFWekMM9b863cyUbOu5EsAtLQHyRnrYGgI5O7KCugg==");

            var client = new GremlinClient(gremlinServer);
            var result = client.SubmitWithSingleResultAsync<dynamic>("g.addV('test')").Result;
            
        }
    }
}
