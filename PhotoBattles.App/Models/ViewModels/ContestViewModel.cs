namespace PhotoBattles.App.Models.ViewModels
{
    using System;

    using AutoMapper;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class ContestViewModel : IMapFrom<Contest>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserViewModel Organizer { get; set; }
    }
}