namespace PhotoBattles.App.Areas.Admin.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class AdminUserBindingModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}