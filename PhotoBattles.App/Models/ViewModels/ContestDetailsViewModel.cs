namespace PhotoBattles.App.Models.ViewModels
{
    using System.Collections.Generic;

    public class ContestDetailsViewModel : ContestViewModel
    {
        public ICollection<PhotoViewModel> Photos { get; set; }
    }
}