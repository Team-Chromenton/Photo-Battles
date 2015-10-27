namespace PhotoBattles.App.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models.Enumerations;

    public class ContestBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public VotingStrategy VotingStartegy { get; set; }

        public IList<UserViewModel> AvailableVoters { get; set; }

        public string[] Voters { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public IList<UserViewModel> AvailableParticipants { get; set; }

        public string[] Participants { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public int? NumberOfWinners { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ParticipantsLimit { get; set; }
    }
}