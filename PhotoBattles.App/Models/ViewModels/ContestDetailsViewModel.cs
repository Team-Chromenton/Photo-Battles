namespace PhotoBattles.App.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class ContestDetailsViewModel : ContestViewModel, ICustomMappings
    {
        public ICollection<PhotoViewModel> Photos { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Contest, ContestDetailsViewModel>()
                         .ForMember(c => c.Photos, opt => opt.MapFrom(c => c.Photos.OrderByDescending(p => p.Uploaded)));
        }
    }
}