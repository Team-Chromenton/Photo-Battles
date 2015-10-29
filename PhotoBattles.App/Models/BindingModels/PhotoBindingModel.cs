namespace PhotoBattles.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class PhotoBindingModel
    {
        [Required]
        public int? ContestId { get; set; }

        [Required]
        public HttpPostedFileBase PhotoData { get; set; }
    }
}