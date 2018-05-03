using System.Net.Http;
using System.Threading.Tasks;

namespace ReferatsDownloader.Interfaces
{
    interface IHttpResponseProcessor
    {
        Task<string> Process(HttpResponseMessage responseMessage);
    }
}
