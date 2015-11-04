namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using PhotoBattles.App.Areas.Admin.Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class AdminPhotosController : AdminBaseController
    {
        [HttpGet]
        public ActionResult EditContestPhotos(int id)
        {
            var contest = this.Data.Contests
                              .GetAll()
                              .Where(c => c.Id == id)
                              .Select(
                                  c =>
                                  new AdminPicturesViewModel
                                      {
                                          ContestName = c.Title,
                                          Pictures = c.Photos
                                                      .Where(p => p.ContestId == id)
                                                      .Select(
                                                          p =>
                                                          new AdminPictureViewModel
                                                              {
                                                                  Id = p.Id,
                                                                  Url = p.Url
                                                              })
                                                      .ToList()
                                      })
                              .FirstOrDefault();

            return this.View(contest);
        }

        [HttpGet]
        public ActionResult DeletePhotoConfirmation(int id)
        {
            var contestId = this.Data.Photos.Find(id).ContestId;
            this.ViewBag.PictureId = id;
            this.ViewBag.ContestId = contestId;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePhoto(int id)
        {
            var picture = this.Data.Photos.Find(id);
            var contestId = picture.ContestId;
            this.Data.Photos.Delete(picture);
            this.Data.SaveChanges();
            return this.RedirectToAction("EditContestPhotos", new { id = contestId });
        }
    }
}