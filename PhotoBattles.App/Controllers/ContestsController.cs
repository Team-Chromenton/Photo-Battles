namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ContestsController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View();
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return this.View();
        }

        public ActionResult Create()
        {
            return this.View();
        }

        public ActionResult Participate(int id)
        {
            return this.View();
        }

        public ActionResult GetResultsOfContest(int id)
        {
            return this.View();
        }

        public ActionResult GetContestsByUser(string username)
        {
            return this.View();
        }

        public ActionResult UpdateContestUser(string username)
        {
            return this.View();
        }

        public ActionResult DismissContestUser(string username)
        {
            return this.View();
        }

        public ActionResult FinalizeContestUser(string username)
        {
            return this.View();
        }
    }
}