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

        private ICollection<User> votingUsers;

        private ICollection<User> winners;

        public Contest()
        {
            this.photos = new HashSet<Photo>();
            this.participants = new HashSet<User>();
            this.votingUsers = new HashSet<User>();
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
        public IRewardStrategy RewardStrategy { get; set; }

        [Required]
        public IVotingStrategy VotingStrategy { get; set; }

        [Required]
        public IParticipationStrategy ParticipationStrategy { get; set; }

        [Required]
        public IDeadlineStrategy DeadlineStrategy { get; set; }

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

        public virtual ICollection<User> VotingUsers
        {
            get
            {
                return this.votingUsers;
            }

            set
            {
                this.votingUsers = value;
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