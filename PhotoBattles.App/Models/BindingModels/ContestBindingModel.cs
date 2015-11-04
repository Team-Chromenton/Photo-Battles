namespace PhotoBattles.App.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models.Enumerations;

    public class ContestBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        //// Strategies - Enum
        public VotingStrategyEnum VotingStrategyEnum { get; set; }

        public ParticipationStrategyEnum ParticipationStrategyEnum { get; set; }

        public RewardStrategyEnum RewardStrategyEnum { get; set; }

        public DeadlineStrategyEnum DeadlineStrategyEnum { get; set; }
        //// Strategies - Enum

        public ICollection<UserViewModel> AvailableVoters { get; set; }

        public ICollection<UserViewModel> AvailableParticipants { get; set; }

        public string[] Voters { get; set; }

        public string[] Participants { get; set; }

        public int? NumberOfWinners { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ParticipantsLimit { get; set; }
    }
}