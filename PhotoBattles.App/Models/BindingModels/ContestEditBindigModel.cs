using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoBattles.App.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ContestEditBindigModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ParticipantsLimit { get; set; }
    }
}