namespace PhotoBattles.App.Areas.Admin.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models.Enumerations;

    public class AdminContestBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public VotingStrategy VotingStrategy { get; set; }

        public ICollection<UserViewModel> AvailableVoters { get; set; }

        public string[] Voters { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public ICollection<UserViewModel> AvailableParticipants { get; set; }

        public string[] Participants { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public int? NumberOfWinners { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ParticipantsLimit { get; set; }
    }
}