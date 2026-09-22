namespace PosWebApplication.Services.Admin
{
    public interface IAdminService
    {
        bool MakeAdmin(int u_id);
        bool DeleteUser(int u_id);
    }
}
