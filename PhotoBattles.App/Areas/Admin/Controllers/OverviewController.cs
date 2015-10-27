namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Areas.Admin.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class OverviewController : BaseController
    {
        // GET: Admin/Overview
        public ActionResult Index()
        {
            this.ViewBag.Title = "Admin";

            var adminId = this.User.Identity.GetUserId();

            var users = this.Data
                            .Users
                            .GetAll()
                            .Where(u => u.Id != adminId)
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