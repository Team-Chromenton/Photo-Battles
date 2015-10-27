namespace PhotoBattles.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Contest> ownContests;

        private ICollection<Contest> contests;

        private ICollection<Contest> allowedForPartisipation;

        private ICollection<Contest> allowedForVoting;

        private ICollection<Contest> winContests;

        private ICollection<Vote> votes;

        public User()
        {
            this.ownContests = new HashSet<Contest>();
            this.contests = new HashSet<Contest>();
            this.allowedForPartisipation = new HashSet<Contest>();
            this.allowedForVoting = new HashSet<Contest>();
            this.winContests = new HashSet<Contest>();
            this.votes = new HashSet<Vote>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Contest> OwnContests
        {
            get
            {
                return this.ownContests;
            }

            set
            {
                this.ownContests = value;
            }
        }

        public virtual ICollection<Contest> Contests
        {
            get
            {
                return this.contests;
            }

            set
            {
                this.contests = value;
            }
        }

        public virtual ICollection<Contest> AllowedForParticipation
        {
            get
            {
                return this.allowedForPartisipation;
            }

            set
            {
                this.allowedForPartisipation = value;
            }
        }

        public virtual ICollection<Contest> AllowedForVoting
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

        public virtual ICollection<Contest> WinContests
        {
            get
            {
                return this.winContests;
            }

            set
            {
                this.winContests = value;
            }
        }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}