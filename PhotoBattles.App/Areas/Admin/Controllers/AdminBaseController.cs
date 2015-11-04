namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;

    [Authorize(Roles = "Admin")]
    public class AdminBaseController : Controller
    {
        public AdminBaseController()
            : this(new PhotoBattlesData())
        {
        }

        public AdminBaseController(IPhotoBattlesData data)
        {
            this.Data = data;
        }

        public IPhotoBattlesData Data { get; set; }
    }
}