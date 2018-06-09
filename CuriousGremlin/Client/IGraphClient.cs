using System;
using System.Threading.Tasks;
using System.Collections;
using System.Text;

namespace CuriousGremlin.Client
{
    public interface IGraphClient
    {
        Task<IEnumerable> Execute(string query);
    }
}
