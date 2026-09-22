using PosWebApplication.DTOs.User;
using PosWebApplication.Entity;

namespace PosWebApplication.Services.User
{
    public interface IUserService
    {
        bool Register(RegisterDTO model);
        user? Login(LoginDTO model);
        void UpdateProfile(user user);
    }
}