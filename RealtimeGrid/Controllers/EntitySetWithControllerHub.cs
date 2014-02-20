using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RealtimeGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.OData;

namespace RealtimeGrid.Controllers
{
    public class EntitySetWithControllerHub<THub> : EntitySetController<Employee, int> where THub: IHub
    {
        // initialize a hub
        Lazy<IHubContext> hub = new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<THub>());

        // return as a protected member
        protected IHubContext Hub { get { return hub.Value; } }
    }
}