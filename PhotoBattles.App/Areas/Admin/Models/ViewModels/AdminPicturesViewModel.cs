namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using System.Collections.Generic;

    public class AdminPicturesViewModel
    {
        public string ContestName { get; set; }

        public ICollection<AdminPictureViewModel> Pictures { get; set; }
    }
}