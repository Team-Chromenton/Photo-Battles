namespace PhotoBattles.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Vote
    {
        [Key]
        public int Id { get; set; }

        public int Score { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}