namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Areas.Admin.Models.ViewModels;
    using PhotoBattles.App.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class OverviewController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            this.ViewBag.Title = "Admin";

            var adminId = this.User.Identity.GetUserId();

            var users = this.Data
                            .Users
                            .GetAll()
                            .Where(u => u.Id != adminId)
                            .ProjectTo<UserViewModel>()
                            .ToList();

            var contests = this.Data
                               .Contests
                               .GetAll()
                               .ProjectTo<ContestViewModel>()
                               .ToList();

            var model = new AdminOverviewViewModel() { Users = users, Contests = contests };

            return this.View(model);
        }
    }
}