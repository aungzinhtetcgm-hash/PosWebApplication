using PosWebApplication.DAO.CommentDAO;
using PosWebApplication.DTOs.Comment;
using PosWebApplication.Entity;

namespace PosWebApplication.Services.Comment
{
    public class CommentService : ICommentService
    {
        private readonly ICommentDAO _commentDAO;

        public CommentService(ICommentDAO commentDAO)
        {
            _commentDAO = commentDAO;
        }

        public List<comment> GetByPostId(int postId)
        {
            return _commentDAO.GetByPostId(postId);
        }

        public void Create(CreateCommentDTO model, int userId)
        {
            var comment = new comment
            {
                u_id = userId,
                p_id = model.p_id,
                comments = model.comments,
                created_at = DateTime.Now,
                updated_at = DateTime.Now
            };

            _commentDAO.Create(comment);
        }
    }
}