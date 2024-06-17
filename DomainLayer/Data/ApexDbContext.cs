using DomainLayer.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models;

namespace DomainLayer.Data
{
    public class ApexDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<Branch> Branches { get; set; }

        public ApexDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ApplicationUserConfig());
            builder.ApplyConfiguration(new BranchConfig());
            builder.ApplyConfiguration(new LikeConfig());
            builder.ApplyConfiguration(new PostConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new NotificationConfig());
            builder.ApplyConfiguration(new MessageConfig());
            base.OnModelCreating(builder); 
        }
    }
}
