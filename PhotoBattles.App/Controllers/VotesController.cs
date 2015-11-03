namespace PhotoBattles.App.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.SignalR;

    using PhotoBattles.App.Hubs;
    using PhotoBattles.Models;

    public class VotesController : BaseController
    {
        [System.Web.Mvc.Authorize]
        public ActionResult Upvote(int photoId)
        {
            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var newVote = new Vote { Score = 1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            var hub = new VotingHub();
            hub.IncreaseScore(photoId);

            return this.Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.Authorize]
        public ActionResult Downvote(int photoId)
        {
            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            var newVote = new Vote { Score = -1, AuthorId = currentUserId, PhotoId = photoId };

            this.Data.Votes.Add(newVote);
            this.Data.SaveChanges();

            var hub = new VotingHub();
            hub.DecreaseScore(photoId);

            return this.Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}