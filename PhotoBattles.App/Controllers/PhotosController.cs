﻿namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Google.Apis.Drive.v2;
    using Google.Apis.Drive.v2.Data;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.App.Services;
    using PhotoBattles.Models;

    public class PhotosController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index(int contestId)
        {
            var photos = this.Data.Photos.GetAll()
                             .Where(p => p.ContestId == contestId)
                             .OrderByDescending(p => p.Uploaded)
                             .ProjectTo<PhotoViewModel>();

            return this.View(photos);
        }

        [ChildActionOnly]
        public ActionResult AddPhoto(int? contestId)
        {
            var model = new PhotoBindingModel { ContestId = contestId };
            return this.PartialView("~/Views/Photos/_AddPhoto.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddPhoto(PhotoBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ViewBag.StatusMessage("Error");
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            string[] uploadResults = this.UploadPhotoToGoogleDrive(model.PhotoData);

            if (uploadResults[0] == "success")
            {
                if (model.ContestId != null)
                {
                    var newPhoto = new Photo()
                        {
                            Url = uploadResults[1],
                            Uploaded = DateTime.Now,
                            AuthorId = currentUserId,
                            ContestId = (int)model.ContestId
                        };

                    this.Data.Photos.Add(newPhoto);
                }

                this.Data.SaveChanges();

                return this.RedirectToAction("Details", "Contests", new { id = model.ContestId });
            }
            else
            {
                return this.ViewBag.StatusMessage = "Error";
            }
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
                FilesResource.InsertMediaUpload request = service.Files.Insert(
                    body,
                    fileData.InputStream,
                    photoMimeType);
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