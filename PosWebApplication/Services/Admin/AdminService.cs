using PosWebApplication.Constraints;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.Services.Admin;

namespace PosWebApplication.Services.Admin
{
    public class AdminService : IAdminService
    {
        private readonly IUserDAO _userDAO;

        public AdminService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public bool MakeAdmin(int u_id)
        {
            var user = _userDAO.GetById(u_id);

            if (user == null)
            {
                return false;
            }

            if (user.role == UserRoles.Admin)
            {
                return false;
            }

            user.role = UserRoles.Admin;

            _userDAO.Update(user);

            return true;
        }

        public bool DeleteUser(int u_id)
        {
            var user = _userDAO.GetById(u_id);

            if (user == null)
            {
                return false;
            }

            if (user.role == UserRoles.Admin)
            {
                return false;
            }

            _userDAO.Delete(u_id);

            return true;
        }
    }
}
