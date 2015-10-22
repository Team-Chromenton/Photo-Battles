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

        private ICollection<Contest> participatingContests;

        private ICollection<Contest> votingContests;

        private ICollection<Contest> winContests;

        public User()
        {
            this.ownContests = new HashSet<Contest>();
            this.participatingContests = new HashSet<Contest>();
            this.votingContests = new HashSet<Contest>();
            this.winContests = new HashSet<Contest>();
        }

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

        public virtual ICollection<Contest> ParticipatingContests
        {
            get
            {
                return this.participatingContests;
            }

            set
            {
                this.participatingContests = value;
            }
        }

        public virtual ICollection<Contest> VotingContests
        {
            get
            {
                return this.votingContests;
            }

            set
            {
                this.votingContests = value;
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}