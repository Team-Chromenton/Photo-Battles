namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web.Mvc;
    using App.Models.ViewModels;
    using AutoMapper.QueryableExtensions;
    using Models.ViewModels;

    [Authorize(Roles = "Admin")]
    public class PicturesController : BaseController
    {
        [HttpGet]
        public ActionResult EditContestPictures(int id)
        {
            var contest = this.Data.Contests
                .GetAll()
                .Where(c => c.Id == id)
                .Select(c =>
                    new AdminEditPicturesViewModel
                    {
                        ContestName = c.Title,
                        Pictures = c.Photos
                                    .Where(p => p.ContestId == id)
                                    .Select(p => 
                                        new AdminContestPicturesViewModel
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
        public ActionResult DeletePictureConfirmation(int id)
        {
            var contestId = this.Data.Photos.Find(id).ContestId;
            ViewBag.PictureId = id;
            ViewBag.ContestId = contestId;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePicture(int id)
        {
            var picture = this.Data.Photos.Find(id);
            var contestId = picture.ContestId;
            this.Data.Photos.Delete(picture);
            this.Data.SaveChanges();
            return this.RedirectToAction("EditContestPictures", new { id = contestId } );
        }
    }
}