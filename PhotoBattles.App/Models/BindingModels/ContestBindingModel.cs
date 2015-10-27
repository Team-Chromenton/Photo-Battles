namespace PhotoBattles.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.Models.Enumerations;

    public class ContestBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public VotingStrategy VotingStartegy { get; set; }

        public string CommiteeMembers { get; set; }
    }
}