using Core.Models;
using Service.Generic;
using Service.Service;
using Service.UnitOfWork;
using System.Net.Http.Json;
using System;

namespace Service.Repository
{
    public class RequestRepository : GenericRepository<Request>, IRequestService
    {
        public RequestRepository(MotorbikeDBContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

        public async Task<HttpResponseMessage> CallRequestRegisterApi(string apiUrl,Request request)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                return await httpClient.PostAsJsonAsync(apiUrl, request);
            }
            //string currentController = this.ControllerContext.ActionDescriptor.ControllerName;
            //var apiUrl = Url.Action("RequestRegister", currentController);
        }
    }
}
