namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    public class PicturesController : Controller
    {
        public ActionResult Create()
        {
            return this.View();
        }
    }
}