using API.DTO.VnPayDTO;
using Core.VnPayModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service.VnPay.Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentCreateModel model, HttpContext context, int userId);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
