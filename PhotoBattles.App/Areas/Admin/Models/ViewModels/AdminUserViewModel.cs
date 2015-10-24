namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class AdminUserViewModel : IMapFrom<User>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}