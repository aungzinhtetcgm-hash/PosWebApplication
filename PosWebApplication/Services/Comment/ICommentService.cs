using PosWebApplication.DTOs.Comment;
using PosWebApplication.Entity;

namespace PosWebApplication.Services.Comment
{
    public interface ICommentService
    {
        List<comment> GetByPostId(int p_id);
        void Create(CreateCommentDTO model, int u_id);
    }
}
