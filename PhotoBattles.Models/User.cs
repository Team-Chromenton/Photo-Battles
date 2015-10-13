namespace PhotoBattles.Models
{
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Contest> contests;

        public User()
        {
            this.contests = new HashSet<Contest>();
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
    }
}