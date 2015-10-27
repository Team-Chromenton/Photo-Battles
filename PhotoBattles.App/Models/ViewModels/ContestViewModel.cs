namespace PhotoBattles.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Contracts;
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

        public ICollection<string> InvitedUsers { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestViewModel>()
                .ForMember(c => c.InvitedUsers,
                    opt => opt.MapFrom(c => c.RegisteredParticipants.Select(u => u.UserName)));
        }
    }
}