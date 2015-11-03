namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Hubs;
    using PhotoBattles.Models;

    [Authorize]
    public class VotesController : BaseController
    {
        public ActionResult Upvote(int contestId, int photoId)
        {
            if (!this.CanVote(contestId, photoId))
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
            if (!this.CanVote(contestId, photoId))
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
    }
}