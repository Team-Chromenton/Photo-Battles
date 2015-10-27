namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Areas.Admin.Models.BindingModels;
    using PhotoBattles.App.Areas.Admin.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class ContestsController : BaseController
    {
        // GET: Admin/Contests
        [HttpGet]
        public ActionResult EditContests(int id)
        {
            var contest = this.Data
                              .Contests
                              .GetAll()
                              .Where(c => c.Id == id)
                              .ProjectTo<AdminContestViewModel>()
                              .FirstOrDefault();

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.Title = contest.Title;
            return this.View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContests(AdminContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var contest = this.Data
                              .Contests
                              .GetAll()
                              .FirstOrDefault(c => c.Id == model.Id);

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            contest.Title = model.Title;
            contest.Description = model.Description;

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Overview");
        }
    }
}