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

        private ICollection<Contest> contestsWon;

        private ICollection<Vote> votes;

        public User()
        {
            this.ownContests = new HashSet<Contest>();
            this.contests = new HashSet<Contest>();
            this.contestsWon = new HashSet<Contest>();
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}