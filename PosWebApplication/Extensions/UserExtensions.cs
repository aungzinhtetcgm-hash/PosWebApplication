using PosWebApplication.Constraints;
using PosWebApplication.Entity;

namespace PosWebApplication.Extensions
{
    public static class UserExtensions
    {
        public static bool IsAdmin(
            this user currentUser)
        {
            return currentUser.role == UserRoles.Admin;
        }

        public static bool IsUser(
            this user currentUser)
        {
            return currentUser.role == UserRoles.User;
        }
    }
}