using BusinessLayer.Middleware;
using BusinessLayer.Repositories.UserRepo;
using BusinessLayer.Services.FriendRequestServices;
using BusinessLayer.Services.UserServices;
using DNTCaptcha.Core;
using DomainLayer.Data;
using DomainLayer.DataAcess;
using DomainLayer.DbSeed;
using DomainLayer.Interfaces.IRepo.IuserRepos;
using DomainLayer.Interfaces.IService.IFriendRequestServices;
using DomainLayer.Interfaces.IService.IPostServices;
using DomainLayer.Interfaces.IService.IuserServices;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApexDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<Authentication>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        //services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFriendRequestService, FrinedRequestService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IPostService, PostService>();

        //repository
        builder.Services.AddScoped<IUserRepo, UserRepository>();
        builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

        //hangfire
        builder.Services.AddHangfire(config =>
              config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Hangfire server
        builder.Services.AddHangfireServer();



        //capthca generator
        builder.Services.AddDNTCaptcha(options =>
        {
            options.UseCookieStorageProvider(SameSiteMode.Strict)
             .AbsoluteExpiration(minutes: 7)
            .ShowThousandsSeparators(false)
            .WithEncryptionKey("IXSIGHT_KEY_FOR_DNT_CAPTCHA_GENERATION_123!@#")
            .InputNames(// This is optional. Change it if you don't like the default names.
                new DNTCaptchaComponent
                {
                    CaptchaHiddenInputName = "DNT_CaptchaText",
                    CaptchaHiddenTokenName = "DNT_CaptchaToken",
                    CaptchaInputName = "DNT_CaptchaInputText"
                })
            .Identifier("dnt_Captcha")// This is optional. Change it if you don't like its default name.
            ;
        });


        // Configure JWT authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
            };
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        RecurringJob.AddOrUpdate<PostService>("CleanupArchivedPosts",
            service => service.CleanupArchivedPosts(),
            Cron.Daily);

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        //hangfire
        app.UseHangfireDashboard();

        app.UseHangfireServer();


        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.DbSeed();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}/{id?}");

        app.Run();
    }
}