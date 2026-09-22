using PosWebApplication.Entity;

namespace PosWebApplication.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public List<user> Users { get; set; } = new();
    }
}
