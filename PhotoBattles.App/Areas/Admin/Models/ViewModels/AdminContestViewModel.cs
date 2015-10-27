namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using System;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class AdminContestViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpen { get; set; }

        public string OrganizerId { get; set; }
    }
}