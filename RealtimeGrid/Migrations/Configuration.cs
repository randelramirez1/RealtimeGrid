namespace RealtimeGrid.Migrations
{
    using RealtimeGrid.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RealtimeGrid.Models.RealtimeGridContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(RealtimeGrid.Models.RealtimeGridContext context)
        {
            context.Employees.AddOrUpdate(e => e.Name,
                    new Employee { Name = "Randel Ramirez", Email = "me@company.com", Salary = 1 },
                    new Employee { Name = "Alain Franchimon", Email = "me@company.com", Salary = 1 },
                    new Employee { Name = "Crispin Muyrong", Email = "doc@company.com", Salary = 1 },
                    new Employee { Name = "Gene Adina", Email = "me@company.com", Salary = 1 },
                    new Employee { Name = "Benj Medina", Email = "me@company.com", Salary = 1 },
                    new Employee { Name = "Pam Vinluan", Email = "me@company.com", Salary = 1 },
                     new Employee { Name = "Wilfredo Vallente", Email = "me@company.com", Salary = 1 }
                );
        }
    }
}
