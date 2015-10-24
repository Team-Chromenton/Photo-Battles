namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Areas.Admin.Models.BindingModels;
    using PhotoBattles.App.Areas.Admin.Models.ViewModels;

    public class UsersController : BaseController
    {
        // GET: Admin/Users
        [HttpGet]
        public ActionResult EditUser(string username)
        {
            this.ViewBag.Title = username;

            var user = this.Data
                           .Users
                           .GetAll()
                           .Where(u => u.UserName == username)
                           .ProjectTo<AdminUserViewModel>()
                           .FirstOrDefault();

            if (user == null)
            {
                return this.HttpNotFound();
            }

            return this.View(user);
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(AdminUserBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
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

            return this.RedirectToAction("Index", "Overview");
        }
    }
}