namespace PhotoBattles.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Contest
    {
        private ICollection<Photo> photos;

        private ICollection<User> registeredParticipants;

        private ICollection<User> participants;

        private ICollection<User> winners;

        private ICollection<User> registeredVoters;

        public Contest()
        {
            this.photos = new HashSet<Photo>();
            this.registeredParticipants = new HashSet<User>();
            this.participants = new HashSet<User>();
            this.winners = new HashSet<User>();
            this.registeredVoters = new HashSet<User>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public bool IsOpen { get; set; }

        [Required]
        public string OrganizerId { get; set; }

        public virtual User Organizer { get; set; }

        //[Required]
        public virtual string VotingStrategy { get; set; }

        //[Required]
        //public virtual IParticipationStrategy ParticipationStrategy { get; set; }

        //[Required]
        //public virtual IRewardStrategy RewardStrategy { get; set; }

        //[Required]
        //public virtual IDeadlineStrategy DeadlineStrategy { get; set; }

        [Required]
        public int NumberOfWinners { get; set; }

        public DateTime? Deadline { get; set; }

        public int? NumberOfParticipants { get; set; }

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

        public virtual ICollection<User> RegisteredParticipants
        {
            get
            {
                return this.registeredParticipants;
            }

            set
            {
                this.registeredParticipants = value;
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

        public virtual ICollection<User> RegisteredVoters
        {
            get
            {
                return this.registeredVoters;
            }

            set
            {
                this.registeredVoters = value;
            }
        }

        public void Dismiss()
        {
            this.IsActive = false;
            this.IsOpen = false;
            this.NumberOfWinners = 0;
        }

        public void End()
        {
            this.IsActive = false;
            this.IsOpen = false;
            //this.RewardStrategy.GetWiners();
        }
    }
}