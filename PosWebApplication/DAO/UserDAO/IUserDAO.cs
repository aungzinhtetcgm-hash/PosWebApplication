using PosWebApplication.Entity;

namespace PosWebApplication.DAO.UserDAO
{
    public interface IUserDAO
    {
        user? GetById(int u_id);
        user? GetByEmail(string email);
        List<user> GetAll();
        void Create(user user);
        void Update(user user);
        void Delete(int u_id);
    }
}
