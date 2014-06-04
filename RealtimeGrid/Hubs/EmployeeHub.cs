using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RealtimeGrid.Models;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace RealtimeGrid.Hubs
{
    [HubName("employee")]
    public class EmployeeHub : Hub
    {
        private RealtimeGridContext db = new RealtimeGridContext();

        private static ConcurrentDictionary<string, List<int>> mapping = new ConcurrentDictionary<string, List<int>>();

        public override Task OnConnected()
        {
            mapping.TryAdd(Context.ConnectionId, new List<int>());
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            foreach (var id in mapping[Context.ConnectionId])
            {
                var employeeToPatch = db.Employees.Find(id);
                employeeToPatch.Locked = false;
                db.Entry(employeeToPatch).State = EntityState.Modified;
                db.SaveChanges();
                Clients.Others.unlockEmployee(id);
                //mapping[Context.ConnectionId].Remove(id);
            }

            var list = new List<int>();
            mapping.TryRemove(Context.ConnectionId, out list);
            return base.OnDisconnected();
        }



        public void Lock(int id, string connectionId)
        {
            var employeeToPatch = db.Employees.Find(id);
            employeeToPatch.Locked = true;
            db.Entry(employeeToPatch).State = EntityState.Modified;
            db.SaveChanges();
            Clients.Others.lockEmployee(id,connectionId);
            mapping[Context.ConnectionId].Add(id);
        }

        public void Unlock(int id)
        {
            var employeeToPatch = db.Employees.Find(id);
            employeeToPatch.Locked = false;
            db.Entry(employeeToPatch).State = EntityState.Modified;
            db.SaveChanges();
            Clients.Others.unlockEmployee(id);
            mapping[Context.ConnectionId].Remove(id);
        }
    }
}