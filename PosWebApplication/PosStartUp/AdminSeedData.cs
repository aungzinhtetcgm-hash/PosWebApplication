using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PosWebApplication.Entity;
using PosWebApplication.Constraints;
using POS_Retails.Entity;

namespace PosWebApplication.PosStartUp
{
    public static class AdminSeedData
    {
        public static async Task InitializeAsync(
            pos_saas_entities context)
        {
            var adminExists = await context.users
                .AnyAsync(user => user.role == UserRoles.Admin);

            if (adminExists)
            {
                return;
            }

            var admin = new user
            {
                name = "S_Admin",
                email = "admin123@gmail.com",
                role = UserRoles.Admin,
                created_by = 0,
                created_at = DateTime.Now
            };

            var passwordHasher = new PasswordHasher<user>();

            admin.password = passwordHasher.HashPassword(
                admin,
                "Admin@!123");

            context.users.Add(admin);

            await context.SaveChangesAsync();
        }
    }
}