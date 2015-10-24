namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;

    public class BaseController : Controller
    {
        public BaseController()
            : this(new PhotoBattlesData())
        {
        }

        public BaseController(IPhotoBattlesData data)
        {
            this.Data = data;
        }

        public IPhotoBattlesData Data { get; set; }
    }
}