namespace PhotoBattles.App.Areas.Admin.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class AdminContestBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}