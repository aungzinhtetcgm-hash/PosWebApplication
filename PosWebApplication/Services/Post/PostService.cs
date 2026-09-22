using PosWebApplication.DAO.PostDAO;
using PosWebApplication.DTOs.Post;
using PosWebApplication.Entity;
using PosWebApplication.Exceptions;

namespace PosWebApplication.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IPostDAO _postDAO;

        public PostService(IPostDAO postDAO)
        {
            _postDAO = postDAO;
        }

        public void Create(CreatePostDTO model, int u_id)
        {
            var post = new post
            {
                title = model.title,
                description = model.description,
                public_flag = model.public_flag,
                created_by = u_id,
                updated_by = u_id,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _postDAO.Create(post);
        }

        public List<post> GetPublicPosts()
        {
            return _postDAO
                .GetAll()
                .Where(x => x.public_flag == "public")
                .ToList();
        }

        public List<post> GetMyPosts(int u_id)
        {
            return _postDAO
                .GetByu_id(u_id);
        }

        public post? GetPost(int p_id, int u_id)
        {
            var post = _postDAO.GetById(p_id);

            if (post == null)
            {
                return null;
            }

            if (post.public_flag == "public")
            {
                return post;
            }

            if (post.public_flag == "private" &&
                post.created_by == u_id)
            {
                return post;
            }

            return null;
        }

        public void Update(UpdatePostDTO model, int u_id)
        {
            var post = _postDAO.GetById(model.p_id);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            if (post.created_by != u_id)
            {
                throw new UnauthorizedAdminException(
                    "You can only edit your own post."
                );
            }

            post.title = model.title;
            post.description = model.description;
            post.public_flag = model.public_flag;
            post.updated_by = u_id;
            post.updated_at = DateTime.Now;

            _postDAO.Update(post);
        }

        public void Delete(int p_id, int u_id)
        {
            var post = _postDAO.GetById(p_id);

            if (post == null)
            {
                throw new NotFoundException("Post not found.");
            }

            if (post.created_by != u_id)
            {
                throw new UnauthorizedAdminException(
                    "You can only delete your own post."
                );
            }

            _postDAO.Delete(post);
        }
    }
}
