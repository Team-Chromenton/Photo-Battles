namespace PhotoBattles.App.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.Models;

    public class VotesController : BaseController
    {
        [Authorize]
        public async Task<ActionResult> Upvote(int photoId, int contestId)
        {
            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var newVote = new Vote { Score = 1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            return this.RedirectToAction("Details", "Contests", new { id = contestId });
        }

        [Authorize]
        public async Task<ActionResult> Downvote(int photoId, int contestId)
        {
            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var newVote = new Vote { Score = -1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            return this.RedirectToAction("Details", "Contests", new { id = contestId });
        }
    }
}