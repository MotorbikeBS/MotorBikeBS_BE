﻿using Core.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace API.Hubs
{
    public class NotificationHub : Hub
    {
        //private static readonly ConcurrentDictionary<string, User> Users =
        //    new ConcurrentDictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);


        //public override Task OnConnected()
        //{
        //    string userName = Context.User.Identity.Name;
        //    string connectionId = Context.ConnectionId;

        //    var user = Users.GetOrAdd(userName, _ => new User
        //    {
        //        UserName = userName,
        //        UserId = new HashSet<string>()
        //    });

        //    lock (user.ConnectionIds)
        //    {
        //        user.ConnectionIds.Add(connectionId);
        //        if (user.ConnectionIds.Count == 1)
        //        {
        //            Clients.Others.userConnected(userName);
        //        }
        //    }

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string userName = Context.User.Identity.Name;
        //    string connectionId = Context.ConnectionId;

        //    UserHubModels user;
        //    Users.TryGetValue(userName, out user);

        //    if (user != null)
        //    {
        //        lock (user.ConnectionIds)
        //        {
        //            user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));
        //            if (!user.ConnectionIds.Any())
        //            {
        //                UserHubModels removedUser;
        //                Users.TryRemove(userName, out removedUser);
        //                Clients.Others.userDisconnected(userName);
        //            }
        //        }
        //    }

        //    return base.OnDisconnected(stopCalled);
        //}
    }
}
