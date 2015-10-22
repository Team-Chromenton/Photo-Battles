namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class ContestsController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Participate(int id)
        {
            return View();
        }

        public ActionResult GetResultsOfContest(int id)
        {
            return View();
        }

        public ActionResult GetContestsByUser(string username)
        {
            return View();
        }

        public ActionResult UpdateContestUser(string username)
        {
            return View();
        }

        public ActionResult DismissContestUser(string username)
        {
            return View();
        }

        public ActionResult FinalizeContestUser(string username)
        {
            return View();
        }
    }
}