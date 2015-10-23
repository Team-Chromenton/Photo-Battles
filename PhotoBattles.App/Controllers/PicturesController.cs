namespace PhotoBattles.App.Controllers
{
    using System.Web.Mvc;

    public class PicturesController : BaseController
    {
        public ActionResult Create()
        {
            return this.View();
        }
    }
}