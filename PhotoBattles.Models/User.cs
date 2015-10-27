namespace PhotoBattles.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Contest> organizedContests;

        private ICollection<Contest> contests;

        private ICollection<Contest> contestsWon;

        private ICollection<Vote> votes;

        private ICollection<Contest> contestsAllowedToVote;

        private ICollection<Contest> contestsAllowedToParticipate;

        public User()
        {
            this.organizedContests = new HashSet<Contest>();
            this.contests = new HashSet<Contest>();
            this.contestsWon = new HashSet<Contest>();
            this.votes = new HashSet<Vote>();
            this.contestsAllowedToVote = new HashSet<Contest>();
            this.contestsAllowedToParticipate = new HashSet<Contest>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public virtual ICollection<Contest> OrganizedContests
        {
            get
            {
                return this.organizedContests;
            }

            set
            {
                this.organizedContests = value;
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

        public virtual ICollection<Contest> ContestsWon
        {
            get
            {
                return this.contestsWon;
            }

            set
            {
                this.contestsWon = value;
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

        public virtual ICollection<Contest> ContestsAllowedToVote
        {
            get
            {
                return this.contestsAllowedToVote;
            }

            set
            {
                this.contestsAllowedToVote = value;
            }
        }

        public virtual ICollection<Contest> ContestsAllowedToParticipate
        {
            get
            {
                return this.contestsAllowedToParticipate;
            }

            set
            {
                this.contestsAllowedToParticipate = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}