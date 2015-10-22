namespace PhotoBattles.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using PhotoBattles.Models.Contracts;

    public class Contest
    {
        private ICollection<Photo> photos;

        private ICollection<User> participants;

        private ICollection<User> allowedForParticipation;

        private ICollection<User> allowedForVoting;

        private ICollection<User> winners;

        public Contest()
        {
            this.photos = new HashSet<Photo>();
            this.participants = new HashSet<User>();
            this.allowedForParticipation = new HashSet<User>();
            this.allowedForVoting = new HashSet<User>();
            this.winners = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string OrganizerId { get; set; }

        public virtual User Organizer { get; set; }

        [Required]
        public virtual IRewardStrategy RewardStrategy { get; set; }

        [Required]
        public virtual IVotingStrategy VotingStrategy { get; set; }

        [Required]
        public virtual IParticipationStrategy ParticipationStrategy { get; set; }

        [Required]
        public virtual IDeadlineStrategy DeadlineStrategy { get; set; }

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

        public virtual ICollection<User> AllowedForParticipation
        {
            get
            {
                return this.allowedForParticipation;
            }

            set
            {
                this.allowedForParticipation = value;
            }
        }

        public virtual ICollection<User> AllowedForVoting
        {
            get
            {
                return this.allowedForVoting;
            }

            set
            {
                this.allowedForVoting = value;
            }
        }

        public virtual ICollection<User> Winners
        {
            get
            {
                return this.winners;
            }

            set
            {
                this.winners = value;
            }
        }
    }
}