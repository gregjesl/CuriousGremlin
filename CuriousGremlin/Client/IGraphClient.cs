using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

namespace CuriousGremlin.Client
{
    public interface IGraphClient
    {
        Task<IEnumerable<object>> Execute(string query);
    }
}
