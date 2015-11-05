namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Infrastructure;
    using PhotoBattles.Data;
    using PhotoBattles.Data.Contracts;

    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class AdminBaseController : Controller
    {
        public AdminBaseController()
            : this(new PhotoBattlesData(), new AspNetUserIdProvider())
        {
        }

        public AdminBaseController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
        {
            this.Data = data;
            this.UserIdProvider = userIdProvider;
        }

        public IPhotoBattlesData Data { get; set; }
        
        public IUserIdProvider UserIdProvider { get; set; }
    }
}