namespace PhotoBattles.App.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Hubs;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models;

    [Authorize]
    public class VotesController : BaseController
    {
        public ActionResult Upvote(int contestId, int photoId)
        {
            bool currentUserCanVote = this.CurrentUserCanVote(contestId, photoId);

            if (!currentUserCanVote)
            {
                return this.RedirectToAction("Details", "Contests", new { id = contestId });
            }

            string currentUserId = this.User.Identity.GetUserId();

            var newVote = new Vote { Score = 1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            var hub = new VotingHub();
            hub.IncreaseScore(photoId);

            return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Downvote(int contestId, int photoId)
        {
            bool currentUserCanVote = this.CurrentUserCanVote(contestId, photoId);

            if (!currentUserCanVote)
            {
                return this.RedirectToAction("Details", "Contests", new { id = contestId });
            }

            string currentUserId = this.User.Identity.GetUserId();

            var newVote = new Vote { Score = -1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            var hub = new VotingHub();
            hub.DecreaseScore(photoId);

            return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        private bool CurrentUserCanVote(int contestId, int photoId)
        {
            var currentUsername = this.User.Identity.GetUserName();
            var contest = this.Data.Contests.GetAll().Where(c => c.Id == contestId).ProjectTo<ContestViewModel>().FirstOrDefault();

            var votingStrategy = contest.GetVotingStrategy(this.Data.Contests.Find(contest.Id));

            if (votingStrategy.CanVote(photoId, currentUsername))
            {
                return true;
            }

            return false;
        }
    }
}