namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using System.Collections.Generic;

    public class AdminOverviewViewModel
    {
        public IEnumerable<AdminUserViewModel> Users { get; set; }

        public IEnumerable<AdminContestViewModel> Contests { get; set; }
    }
}