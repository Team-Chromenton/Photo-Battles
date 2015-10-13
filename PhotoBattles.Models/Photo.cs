namespace PhotoBattles.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        private ICollection<Vote> votes;

        public Photo()
        {
            this.votes = new HashSet<Vote>();
        }

        [Key]
        public int Id { get; set; }

        public string PhotoUrl { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int ContestId { get; set; }

        public Contest Contest { get; set; }

        public virtual ICollection<Vote> Votes
        {
            get
            {
                return this.votes;
            }

            set
            {
                this.votes = value;
            }
        }
    }
}