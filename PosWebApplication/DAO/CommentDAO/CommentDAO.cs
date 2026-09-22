using Microsoft.EntityFrameworkCore;
using POS_Retails.Entity;
using PosWebApplication.Entity;

namespace PosWebApplication.DAO.CommentDAO
{
    public class CommentDAO : ICommentDAO
    {
        private readonly pos_saas_entities _context;

        public CommentDAO(pos_saas_entities context)
        {
            _context = context;
        }

        public List<comment> GetByPostId(int postId)
        {
            return _context.comments
                .Include(x => x.user)
                .Where(x => x.p_id == postId)
                .OrderBy(x => x.created_at)
                .ToList();
        }

        public void Create(comment comment)
        {
            _context.comments.Add(comment);
            _context.SaveChanges();
        }
    }
}