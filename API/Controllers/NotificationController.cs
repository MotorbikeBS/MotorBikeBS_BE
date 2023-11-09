using API.Hubs;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace SignalRNotifications.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _notificationContext;

        public NotificationController(IHubContext<NotificationHub> notificationContext)
        {
            _notificationContext = notificationContext;
        }

        //[HttpPost]
        //public HttpResponseMessage SendNotification(Notification obj)
        //{
        //    NotificationHub objNotifHub = new NotificationHub();
        //    Notification objNotif = new Notification();
        //    objNotif.UserId = obj.UserId;

        //    context.Configuration.ProxyCreationEnabled = false;
        //    context.Notifications.Add(objNotif);
        //    context.SaveChanges();

        //    objNotifHub.SendNotification(objNotif.SentTo);

        //    //var query = (from t in context.Notifications  
        //    //             select t).ToList();  

        //    return Request.CreateResponse(HttpStatusCode.OK);
        //}
    }
}
