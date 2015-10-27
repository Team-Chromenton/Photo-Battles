namespace PhotoBattles.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Commitee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public string Members { get; set; }
    }
}