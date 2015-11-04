namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using System.Collections.Generic;

    using PhotoBattles.App.Models.ViewModels;

    public class AdminOverviewViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<ContestViewModel> Contests { get; set; }
    }
}