using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoBattles.App.Models.ViewModels
{
    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class VoteViewModel : IMapFrom<Vote>
    {
        public string AuthorId { get; set; }

        public int Score { get; set; }
    }
}