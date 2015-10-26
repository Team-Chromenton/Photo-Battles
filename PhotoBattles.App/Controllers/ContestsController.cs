namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models;

    [Authorize]
    public class ContestsController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            IQueryable<ContestViewModel> contests = this.Data.Contests
                    .GetAll()
                    .OrderByDescending(c => c.IsActive)
                    .ThenByDescending(c => c.IsOpen)
                    .ThenByDescending(c => c.CreatedOn)
                    .ProjectTo<ContestViewModel>();

            return this.View(contests);
        }

        public ActionResult AddContest()
        {
            return this.PartialView("~/Views/Contests/_AddContest.cshtml");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("~/Views/Contests/_AddContest.cshtml", model);
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var currentUser = this.Data.Users.GetAll().Where(u => u.Id == currentUserId).ProjectTo<UserViewModel>().FirstOrDefault();

            var newContest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsOpen = true,
                OrganizerId = currentUserId
            };

            this.Data.Contests.Add(newContest);
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Contests");
        }
    }
}