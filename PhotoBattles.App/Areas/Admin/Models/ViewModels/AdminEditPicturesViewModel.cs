namespace PhotoBattles.App.Areas.Admin.Models.ViewModels
{
    using System.Collections.Generic;

    public class AdminEditPicturesViewModel
    {
        public string ContestName { get; set; }
        public ICollection<AdminContestPicturesViewModel> Pictures { get; set; } 
    }
}