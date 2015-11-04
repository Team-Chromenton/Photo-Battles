namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Areas.Admin.Models.BindingModels;
    using PhotoBattles.App.Extensions;
    using PhotoBattles.App.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class AdminUsersController : AdminBaseController
    {
        [HttpGet]
        public ActionResult EditUser(string username)
        {
            this.ViewBag.Title = username;

            var user = this.Data
                           .Users
                           .GetAll()
                           .Where(u => u.UserName == username)
                           .ProjectTo<UserViewModel>()
                           .FirstOrDefault();

            if (user == null)
            {
                return this.HttpNotFound();
            }

            return this.View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(AdminUserBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.AddNotification("Invalid input data", NotificationType.ERROR);
                return this.RedirectToAction("EditUser", new {username = model.UserName});
            }

            var user = this.Data
                           .Users
                           .GetAll()
                           .FirstOrDefault(u => u.UserName == model.UserName);

            if (user == null)
            {
                return this.HttpNotFound();
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            this.Data.SaveChanges();

            this.AddNotification("User edited", NotificationType.SUCCESS);
            return this.RedirectToAction("Index", "Overview");
        }
    }
}