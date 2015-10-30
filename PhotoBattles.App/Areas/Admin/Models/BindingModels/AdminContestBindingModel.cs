namespace PhotoBattles.App.Areas.Admin.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.Models.Enumerations;

    public class AdminContestBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public VotingStrategy VotingStrategy { get; set; }

        [Required]
        public ParticipationStrategy ParticipationStrategy { get; set; }

        [Required]
        public RewardStrategy RewardStrategy { get; set; }

        [Required]
        public DeadlineStrategy DeadlineStrategy { get; set; }
    }
}