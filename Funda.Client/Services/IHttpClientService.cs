using System.Threading.Tasks;

namespace Funda.Client.Services
{
    public interface IHttpClientService
    {
        //interface method for getting data from api and show on console
        Task Execute();
    }
}
