using PosWebApplication.DTOs.Post;
using PosWebApplication.Entity;

namespace PosWebApplication.Services.Post
{
    public interface IPostService
    {
        void Create(CreatePostDTO model, int u_id);
        List<post> GetPublicPosts();
        List<post> GetMyPosts(int u_id);
        post?  GetPost (int p_id, int u_id);
        void Update(UpdatePostDTO model, int u_id);
        void Delete(int p_id, int u_id);

    }
}
