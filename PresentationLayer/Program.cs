using BusinessLayer.Middleware;
using BusinessLayer.Repositories.UserRepo;
using BusinessLayer.Services.CommentService;
using BusinessLayer.Services.FriendRequestService;
using BusinessLayer.Services.FriendRequestServices;
using BusinessLayer.Services.LikeServices;
using BusinessLayer.Services.NotificationServices;
using BusinessLayer.Services.UserServices;
using DNTCaptcha.Core;
using DomainLayer.Data;
using DomainLayer.DataAcess;
using DomainLayer.DbSeed;
using DomainLayer.Interfaces.IRepo.IuserRepos;
using DomainLayer.Interfaces.IService.IEmailSender;
using DomainLayer.Interfaces.IService.IFriendRequestServices;
using DomainLayer.Interfaces.IService.IPostServices;
using DomainLayer.Interfaces.IService.IuserServices;
using DomainLayer.ViewModels.FriendRequestViewModels;
using DomainLayer.ViewModels.PasswordResetViewModels;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer.Controllers.MessageControllers;
using PresentationLayer.Models;
using System.Text;
using static EmailSender;

public class Program
{
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<ApexDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHttpContextAccessor();

        //smtp
        builder.Services.Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.AddTransient<IEmailService, EmailSender>();
        builder.Services.AddSingleton<OtpGenerator>();



        // SignalR
        builder.Services.AddSignalR();

        // Custom authentication
        builder.Services.AddSingleton<Authentication>();

        // Unit of work 
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFriendRequestService, FrinedRequestService>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IPostService, PostService>();

        // Repositories
        builder.Services.AddScoped<IUserRepo, UserRepository>();
        builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

        // Hangfire
        builder.Services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseDefaultTypeSerializer()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true
            }));

        // Add Hangfire server
        builder.Services.AddHangfireServer();

        // CAPTCHA
        builder.Services.AddDNTCaptcha(options =>
        {
            options.UseCookieStorageProvider(SameSiteMode.Strict)
                   .AbsoluteExpiration(minutes: 7)
                   .ShowThousandsSeparators(false)
                   .WithEncryptionKey("APEX_KEY_FOR_DNT_CAPTCHA_GENERATION_123!@#")
                   .InputNames(new DNTCaptchaComponent
                   {
                       CaptchaHiddenInputName = "DNT_CaptchaText",
                       CaptchaHiddenTokenName = "DNT_CaptchaToken",
                       CaptchaInputName = "DNT_CaptchaInputText"
                   })
                   .Identifier("dnt_Captcha");
        });

        // JWT Authentication
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

       

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Use Hangfire Dashboard and Server
        app.UseHangfireDashboard();     
        app.UseHangfireServer();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.DbSeed();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}/{id?}");

        // signalR used for sending message like and notifcation for real time 
        app.MapHub<MessageHub>("/messageHub");
        app.MapHub<LikeHub>("/likeHub");
        app.MapHub<NotificationHub>("/notificationHub");
        app.MapHub<SendCommentHub>("/commentHub");
        app.MapHub<FreindRequestHub>("/freindrequestHub");

        //recurring job for hangfire use 
        using (var scope = app.Services.CreateScope())
        {
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            var postService = scope.ServiceProvider.GetRequiredService<IPostService>();

            recurringJobManager.AddOrUpdate("CleanupArchivedPosts",
                () => postService.CleanupArchivedPosts(),
                Cron.Daily);
        }

        app.Run();
    }
}
