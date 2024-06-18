using DomainLayer.Data;
using DomainLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DomainLayer.DbSeed
{
    public static class DbInstallizing
    {
        public static IApplicationBuilder DbSeed(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApexDbContext>();
            Seed(context);
            return app;
        }

        private static void Seed(ApexDbContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            context.Database.EnsureCreated();

            if (context.Users.Any()) return;


            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FullName="SoloMon",
                    Email = "admin@example.com",
                    Password = "Solomon@123",
                    PhoneNumber = "9840883321",
                    IsActive = true,
                    Gender = Gender.Male,
                    Roles = Roles.Admin,
                    DateOfBirth = new DateTime(1980,2,2),
                    CreatedBy = "System"    ,
                    TenantId = 1
                }
            };
            var branch = new List<Branch>
            {
                new Branch
                {
                    Name = "Swarswati",
                    DateCreated = DateTime.Now,
                    ModifiedDate = null,
                    CreatedBy= "System" ,
                    IsActive = true,

                }
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}
