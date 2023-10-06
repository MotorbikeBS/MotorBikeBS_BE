using Core.Models;
using Service.Generic;

namespace Service.Service
{
    public interface IRequestService : IGenericRepository<Request>
    {
        Task<HttpResponseMessage> CallRequestRegisterApi(string apiUrl, Request request);
    }
}
