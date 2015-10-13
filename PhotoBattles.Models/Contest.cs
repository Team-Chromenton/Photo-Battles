namespace PhotoBattles.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.Models.Enumerations;

    public class Contest
    {
        private ICollection<User> participants;

        private ICollection<Photo> photos;

        public Contest()
        {
            this.participants = new HashSet<User>();
            this.photos = new HashSet<Photo>();
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public RewardStrategy RewardStrategy { get; set; }

        public VoteStrategy VoteStrategy { get; set; }

        public ParticipationStrategy ParticipationStrategy { get; set; }

        public DeadlineStrategy DeadlineStrategy { get; set; }

        public string OrganizerId { get; set; }

        public virtual User Organizer { get; set; }

        public virtual ICollection<User> Participants
        {
            get
            {
                return this.participants;
            }

            set
            {
                this.participants = value;
            }
        }

        public virtual ICollection<Photo> Photos
        {
            get
            {
                return this.photos;
            }

            set
            {
                this.photos = value;
            }
        }
    }
}