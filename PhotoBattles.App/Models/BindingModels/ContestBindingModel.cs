namespace PhotoBattles.App.Models.BindingModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using PhotoBattles.App.Models.ViewModels;

    public class ContestBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string VotingStartegy { get; set; }

        public IList<UserViewModel> AvailableUsers { get; set; }

        public string[] VotersNames { get; set; }

        //[Required]
        //public string ParticipationStrategy { get; set; }

        //public string Participants { get; set; }

        //[Required]
        //public string RewardStrategy { get; set; }

        //public int? NumberOfWinners { get; set; }

        //[Required]
        //public string DeadlineStrategy { get; set; }

        //public DateTime? Deadline { get; set; }

        //public int? NumberOfParticipants { get; set; }

    }
}