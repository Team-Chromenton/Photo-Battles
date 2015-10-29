namespace PhotoBattles.App.Models.ViewModels
{
    using System.Linq;

    using AutoMapper;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class UserViewModel : IMapFrom<User>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}