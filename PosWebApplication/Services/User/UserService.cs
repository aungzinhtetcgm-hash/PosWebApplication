using Microsoft.AspNetCore.Identity;
using PosWebApplication.Constraints;
using PosWebApplication.DAO.UserDAO;
using PosWebApplication.DTOs.User;
using PosWebApplication.Entity;

namespace PosWebApplication.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        private readonly PasswordHasher<user> _passwordHasher;

        public UserService(IUserDAO userDAO)
        {
            _userDAO = userDAO;
            _passwordHasher = new PasswordHasher<user>();
        }

        public bool Register(RegisterDTO model)
        {
            var existingUser = _userDAO.GetByEmail(model.Email);

            if (existingUser != null)
            {
                return false;
            }

            var newUser = new user
            {
                name = model.Name,
                email = model.Email,
                role = UserRoles.User,
                created_by = 0,
                created_at = DateTime.Now
            };

            newUser.password =
                _passwordHasher.HashPassword(
                    newUser,
                    model.Password);

            _userDAO.Create(newUser);

            return true;
        }

        public user? Login(LoginDTO model)
        {
            var existingUser =
                _userDAO.GetByEmail(model.Email);

            if (existingUser == null)
            {
                return null;
            }

            var result =
                _passwordHasher.VerifyHashedPassword(
                    existingUser,
                    existingUser.password ?? "",
                    model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return existingUser;
        }

        public void UpdateProfile(user user)
        {
            _userDAO.Update(user);
        }
    }
}