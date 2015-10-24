namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Areas.Admin.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class OverviewController : BaseController
    {
        // GET: Admin/Overview
        public ActionResult Index()
        {
            this.ViewBag.Title = "Admin";

            var users = this.Data
                            .Users
                            .GetAll()
                            .ProjectTo<AdminUserViewModel>()
                            .ToList();

            var contests = this.Data
                               .Contests
                               .GetAll()
                               .ProjectTo<AdminContestViewModel>()
                               .ToList();

            var model = new AdminOverviewViewModel() { Users = users, Contests = contests };

            return this.View(model);
        }
    }
}