namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.App.Services;

    using File = Google.Apis.Drive.v2.Data.File;

    public class PhotosController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index(int contestId = 0)
        {
            var photos = this.Data.Photos.GetAll()
                    .Where(p => p.ContestId == contestId)
                    .OrderByDescending(p => p.Uploaded)
                    .ProjectTo<PhotoViewModel>();

            return this.View(photos);
        }

        public ActionResult AddPhoto()
        {
            return this.PartialView("~/Views/Photos/_AddPhoto.cshtml");
        }

        // POST /contests/{id}/photos
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddPhoto(PhotoBindingModel model, int contestId = 1)
        {
            if (!this.ModelState.IsValid)
            {
                return this.PartialView("~/Views/Photos/_AddPhoto.cshtml", model);
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string username = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var currentUser = this.Data.Users.GetAll()
                    .Where(u => u.Id == currentUserId)
                    .ProjectTo<UserViewModel>()
                    .FirstOrDefault();

            string[] uploadResults = this.UploadPhotoToGoogleDrive(model.PhotoData);

            if (uploadResults[0] == "success")
            {



                this.ViewBag.StatusMessage = "Success";
            }
            else
            {
                this.ViewBag.StatusMessage = "Error";
            }

            return this.RedirectToAction("Index", "Photos");
        }

        private string[] UploadPhotoToGoogleDrive(HttpPostedFileBase fileData)
        {
            string photoMimeType = fileData.ContentType;
            string fileName = fileData.FileName;

            var service = GoogleDriveService.Get();

            File body = new File
            {
                Title = fileName,
                MimeType = photoMimeType,
                Parents =
                        new List<ParentReference>
                            {
                                new ParentReference { Id = "0ByDHCWWSmvcLRlNrcEh2QVF3cnM" }
                            }
            };

            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, fileData.InputStream, photoMimeType);
                request.Upload();

                return new[] { "success", "http://docs.google.com/uc?export=open&id=" + request.ResponseBody.Id };
            }
            catch (Exception exception)
            {
                return new[] { "error", string.Format("Something happened.\r\n" + exception.Message) };
            }
        }
    }
}