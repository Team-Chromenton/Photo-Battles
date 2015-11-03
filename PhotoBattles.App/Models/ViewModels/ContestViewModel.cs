namespace PhotoBattles.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    public class ContestViewModel : IMapFrom<Contest>, ICustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public UserViewModel Organizer { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public int? ParticipantsLimit { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<UserViewModel> Winners { get; set; }

        public ICollection<UserViewModel> InvitedUsers { get; set; }

        public ICollection<UserViewModel> Participants { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                         .ForMember(c => c.InvitedUsers, opt => opt.MapFrom(c => c.RegisteredParticipants))
                         .ForMember(c => c.Participants, opt => opt.MapFrom(c => c.Participants));
        }
    }
}