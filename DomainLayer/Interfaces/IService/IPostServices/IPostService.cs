using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces.IService.IPostServices
{
    public interface IPostService
    {
        public Task CleanupArchivedPosts();
        public Task ArchivePost(int postId);
        public Task DeleteArchivedPost(int postId);

    }
}
