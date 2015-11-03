namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Areas.Admin.Models.BindingModels;
    using PhotoBattles.App.Areas.Admin.Models.ViewModels;
    using PhotoBattles.App.Extensions;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

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

            contest.Users = new List<AdminUserViewModel>(
                this.Data
                    .Users
                    .GetAll()
                    .ProjectTo<AdminUserViewModel>()
                    .ToList());

            this.ViewBag.Title = contest.Title;
            return this.View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContests(AdminContestBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                this.AddNotification("Invalid input data", NotificationType.ERROR);
                return this.RedirectToAction("EditContests");
            }

            var contest = this.Data
                              .Contests
                              .GetAll()
                              .FirstOrDefault(c => c.Id == model.Id);

            if (contest == null)
            {
                this.AddNotification("Cannot find contest", NotificationType.ERROR);
                return this.RedirectToAction("EditContests");
            }

            contest.Title = model.Title;
            contest.Description = model.Description;

            if (!(this.EditAvailableVoters(model, contest) &&
                  this.EditAvailableParticipants(model, contest)))
            {
                this.RedirectToAction("EditContests");
            }

            contest.RewardStrategy = model.RewardStrategy;
            contest.DeadlineStrategy = model.DeadlineStrategy;

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Overview");
        }

        private bool EditAvailableParticipants(AdminContestBindingModel model, Contest contest)
        {
            contest.RegisteredParticipants = new List<User>();

            if (model.ParticipationStrategy == ParticipationStrategy.Closed)
            {
                if (model.Participants == null || !model.Participants.Any())
                {
                    this.AddNotification("Please select participants.", NotificationType.ERROR);
                    return false;
                }

                var participants =
                    this.Data.Users.GetAll().Where(u => model.Participants.Contains(u.UserName)).ToList();

                contest.RegisteredParticipants = new List<User>(participants);
                contest.IsOpen = false;
            }
            else
            {
                contest.IsOpen = true;
            }

            contest.ParticipationStrategy = model.ParticipationStrategy;
            return true;
        }

        private bool EditAvailableVoters(AdminContestBindingModel model, Contest contest)
        {
            contest.RegisteredVoters = null;

            if (model.VotingStrategy == VotingStrategy.Closed)
            {
                if (model.Voters == null || !model.Voters.Any())
                {
                    this.AddNotification("Please select voters.", NotificationType.ERROR);
                    return false;
                }

                var voters = this.Data.Users.GetAll().Where(u => model.Voters.Contains(u.UserName)).ToList();

                contest.RegisteredVoters = new List<User>(voters);
            }

            contest.VotingStrategy = model.VotingStrategy;
            return true;
        }
    }
}