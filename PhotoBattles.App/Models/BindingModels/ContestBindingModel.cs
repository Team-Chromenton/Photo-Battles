namespace PhotoBattles.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContestBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string VotingStartegy { get; set; }

        public string CommiteeMembers { get; set; }

        [Required]
        public string ParticipationStrategy { get; set; }

        public string Participants { get; set; }

        [Required]
        public string RewardStrategy { get; set; }

        public int? NumberOfWinners { get; set; }
    }
}