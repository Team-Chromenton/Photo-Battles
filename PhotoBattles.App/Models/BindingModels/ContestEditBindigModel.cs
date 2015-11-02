namespace PhotoBattles.App.Models.BindingModels
{
    using System;
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