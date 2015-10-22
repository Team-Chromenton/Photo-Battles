namespace PhotoBattles.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        [Required]
        public int PhotoId { get; set; }

        public virtual Photo Photo { get; set; }
    }
}