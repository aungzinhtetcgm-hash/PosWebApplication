using PosWebApplication.Entity;

namespace PosWebApplication.DAO.PostDAO
{
    public interface IPostDAO
    {
        List<post> GetAll();
        List<post> GetByu_id(int u_id);
        post?   GetById(int id);
        void Create (post post);
        void Update (post post);    
        void Delete (post post);
    }
}
