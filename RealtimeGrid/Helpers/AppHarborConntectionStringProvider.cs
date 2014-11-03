using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RealtimeGrid.Helpers
{
    public static class AppHarborConntectionStringProvider
    {
        public static string Get()
        {
            var uriString = ConfigurationManager.AppSettings["SQLSERVER_URI"];
            var uri = new Uri(uriString);
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = uri.Host,
                InitialCatalog = uri.AbsolutePath.Trim('/'),
                UserID = uri.UserInfo.Split(':').First(),
                Password = uri.UserInfo.Split(':').Last(),
            }.ConnectionString;

            return connectionString;
        }
    }
}