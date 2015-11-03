namespace PhotoBattles.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Infrastructure;
    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models.Enumerations;

    public class BaseController : Controller
    {
        public BaseController()
            : this(new PhotoBattlesData(), new AspNetUserIdProvider())
        {
        }

        public BaseController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
        {
            this.Data = data;
            this.UserIdProvider = userIdProvider;
        }

        public IPhotoBattlesData Data { get; set; }

        public IUserIdProvider UserIdProvider { get; set; }

        public bool CanVote(int contestId)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var contest = this.Data.Contests.Find(contestId);

            if (contest.VotingStrategy == VotingStrategy.Closed)
            {
                if (contest.RegisteredVoters.Contains(currentUser)
                    && !contest.Photos.Any(p => p.Votes.Any(v => v.AuthorId == currentUserId)))
                {
                    return true;
                }

            }
            else
            {
                if (!contest.Photos.Any(p => p.Votes.Any(v => v.AuthorId == currentUserId)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}