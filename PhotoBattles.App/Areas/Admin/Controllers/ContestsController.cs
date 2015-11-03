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
                  this.EditAvailableParticipants(model, contest) &&
                  this.EditRewardStrategy(model, contest) &&
                  this.EditDeadlineStartegy(model, contest)))
            {
                this.RedirectToAction("EditContests");
            }

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Overview");
        }

        private bool EditDeadlineStartegy(AdminContestBindingModel model, Contest contest)
        {
            if (model.DeadlineStrategy == DeadlineStrategy.EndDate)
            {
                contest.EndDate = model.EndDate;
                contest.ParticipantsLimit = null;
            }
            else
            {
                contest.ParticipantsLimit = model.ParticipantsLimit;
                contest.EndDate = null;
            }

            contest.DeadlineStrategy = model.DeadlineStrategy;

            return true;
        }

        private bool EditRewardStrategy(AdminContestBindingModel model, Contest contest)
        {
            if (model.RewardStrategy == RewardStrategy.MultipleWinners)
            {
                if (model.NumberOfWinners != null)
                {
                    contest.NumberOfWinners = (int)model.NumberOfWinners;
                }
            }
            else
            {
                contest.NumberOfWinners = 1;
            }

            contest.RewardStrategy = model.RewardStrategy;

            return true;
        }

        private bool EditAvailableParticipants(AdminContestBindingModel model, Contest contest)
        {
            contest.RegisteredParticipants.Clear();

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
            contest.RegisteredVoters.Clear();

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