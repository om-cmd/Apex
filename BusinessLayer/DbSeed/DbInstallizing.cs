using BusinessLayer.Helper;
using DomainLayer.Data;
using DomainLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.Models;

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

            // Seed branches if they don't exist
            if (!context.Branches.Any())
            {

                var branches = new List<Branch>
                {
                    new Branch
                    {
                        Name = "Swarswati",
                        DateCreated = DateTime.Now,
                        CreatedBy = "System",
                        IsActive = true,
                        ModifiedDate= DateTime.Now,
                    }
                };

                context.Branches.AddRange(branches);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {

                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        FullName = "SoloMon",
                        Email = "admin@example.com",
                        Password = PasswordHash.Hashing("Solomon@123"),
                        PhoneNumber = "9840883321",
                        IsActive = true,
                        Gender = Gender.Male,
                        Roles = Roles.Admin,
                        DateOfBirth = new DateTime(1980, 2, 2),
                        CreatedBy = "System",
                        TenantId = context.Branches.First().TenantId
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
