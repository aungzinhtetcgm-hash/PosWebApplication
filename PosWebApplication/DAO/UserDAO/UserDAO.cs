using Microsoft.EntityFrameworkCore;
using POS_Retails.Entity;
using PosWebApplication.Entity;

namespace PosWebApplication.DAO.UserDAO
{
    public class UserDAO : IUserDAO
    {
        private readonly pos_saas_entities _context;

        public UserDAO(pos_saas_entities context)
        {
            _context = context;
        }

        public user? GetById(int u_id)
        {
            return _context.users
                .FirstOrDefault(x => x.u_id == u_id);
        }

        public user? GetByEmail(string email)
        {
            return _context.users
                .FirstOrDefault(x => x.email == email);
        }

        public List<user> GetAll()
        {
            return _context.users
                .AsNoTracking()
                .ToList();
        }

        public void Create(user user)
        {
            _context.users.Add(user);
            _context.SaveChanges();
        }

        public void Update(user user)
        {
            _context.users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int u_id)
        {
            var user = _context.users
                .FirstOrDefault(x => x.u_id == u_id);

            if (user == null)
            {
                return;
            }

            _context.users.Remove(user);
            _context.SaveChanges();
        }

    }
}