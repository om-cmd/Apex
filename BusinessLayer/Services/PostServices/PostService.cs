using DomainLayer.Data;
using DomainLayer.Interfaces.IService.IPostServices;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

public class PostService : IPostService
{
    private readonly IServiceProvider _serviceProvider;

    public PostService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task CleanupArchivedPosts()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApexDbContext>();
            var thresholdDate = DateTime.UtcNow.AddDays(-7);
            var postsToDelete = context.Posts.Where(p => p.IsArchived && p.DateArchived <= thresholdDate).ToList();

            context.Posts.RemoveRange(postsToDelete);
            await context.SaveChangesAsync();
        }
    }

    public async Task ArchivePost(int postId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApexDbContext>();
            var post = await context.Posts.FindAsync(postId);
            if (post != null)
            {
                post.IsArchived = true;
                post.DateArchived = DateTime.UtcNow;
                context.Posts.Update(post);
                await context.SaveChangesAsync();

                BackgroundJob.Schedule(() => DeleteArchivedPost(postId), TimeSpan.FromDays(7));
            }
        }
    }

    [AutomaticRetry(Attempts = 0)]
    public async Task DeleteArchivedPost(int postId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApexDbContext>();
            var post = await context.Posts.FindAsync(postId);
            if (post != null && post.IsArchived)
            {
                context.Posts.Remove(post);
                await context.SaveChangesAsync();
            }
        }
    }
}
