using PosWebApplication.Entity;
namespace PosWebApplication.DAO.CommentDAO
{
    public interface ICommentDAO
    {
        List<comment> GetByPostId(int p_id);
        void Create(comment comment);
    }
}
