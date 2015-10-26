namespace PhotoBattles.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContestBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }


    }
}