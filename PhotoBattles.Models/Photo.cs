namespace PhotoBattles.Models
{
    using System;
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

        [Required]
        public string Url { get; set; }

        [Required]
        public DateTime Uploaded { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        [Required]
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