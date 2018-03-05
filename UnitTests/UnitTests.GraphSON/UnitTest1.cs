using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gremlin.Net.Driver;

namespace UnitTests.GraphSON
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var gremlinServer = new GremlinServer("localhost", 8182);
            using (var gremlinClient = new GremlinClient(gremlinServer))
            {
                var result = gremlinClient.SubmitWithSingleResultAsync<bool>("g.V().has('name', 'gremlin').hasNext()").Result;
            }
        }
    }
}
