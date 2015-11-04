namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Hubs;
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

        public void CheckActive()
        {
            var hub = new ContestInformationHub();

            var contests = this.Data.Contests.GetAll().Where(c => c.IsActive);

            foreach (var contest in contests)
            {
                if (contest.DeadlineStrategy == DeadlineStrategy.EndDate)
                {
                    if (DateTime.Now > contest.EndDate)
                    {
                        contest.IsActive = false;
                        hub.InfoExpiredContest(contest.Title, contest.Id);
                    }
                }
                else if (contest.DeadlineStrategy == DeadlineStrategy.ParticipantsLimit)
                {
                    if (contest.RegisteredParticipants.Count > contest.ParticipantsLimit)
                    {
                        contest.IsActive = false;
                        hub.InfoExpiredContest(contest.Title, contest.Id);
                    }
                }
            }

            this.Data.SaveChanges();
        }

        public bool CanVote(int contestId, int photoId)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var currentUser = this.Data.Users.Find(currentUserId);

            var contest = this.Data.Contests.Find(contestId);

            if (contest.VotingStrategy == VotingStrategy.Closed)
            {
                if (contest.RegisteredVoters.Contains(currentUser)
                    && !contest.Photos.FirstOrDefault(p => p.Id == photoId).Votes.Any(v => v.AuthorId == currentUserId))
                {
                    return true;
                }
            }
            else
            {
                if (!contest.Photos.First(p => p.Id == photoId).Votes.Any(v => v.AuthorId == currentUserId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}