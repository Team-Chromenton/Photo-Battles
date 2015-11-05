namespace PhotoBattles.App.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Extensions;
    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    [Authorize(Roles = "Admin")]
    public class AdminContestsController : AdminBaseController
    {
        public AdminContestsController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [HttpGet]
        public ActionResult AdminEditContest(int id)
        {
            var contest = this.Data
                              .Contests
                              .GetAll()
                              .Where(c => c.Id == id)
                              .ProjectTo<ContestViewModel>()
                              .FirstOrDefault();

            if (contest == null)
            {
                return this.HttpNotFound();
            }

            string currentUserName = this.UserIdProvider.GetUserName();

            var availableParticipants = this.Data.Users
                                            .GetAll()
                                            .ProjectTo<UserViewModel>()
                                            .ToList();
            contest.AvailableParticipants = new List<UserViewModel>(availableParticipants);

            var availableVoters = availableParticipants.Where(u => u.UserName != currentUserName)
                                                       .ToList();
            contest.AvailableVoters = new List<UserViewModel>(availableVoters);

            this.ViewBag.Title = contest.Title;
            return this.View(contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEditContest(ContestBindingModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                this.AddNotification("Invalid input data", NotificationType.ERROR);
                return this.RedirectToAction("AdminEditContest");
            }

            var contest = this.Data
                              .Contests
                              .GetAll()
                              .FirstOrDefault(c => c.Id == model.Id);

            if (contest == null)
            {
                this.AddNotification("Cannot find contest", NotificationType.ERROR);
                return this.RedirectToAction("AdminEditContest");
            }

            contest.Title = model.Title;
            contest.Description = model.Description;

            if (!(this.EditAvailableVoters(model, contest) &&
                  this.EditAvailableParticipants(model, contest) &&
                  this.EditRewardStrategy(model, contest) &&
                  this.EditDeadlineStartegy(model, contest)))
            {
                this.RedirectToAction("AdminEditContest");
            }

            this.Data.SaveChanges();

            this.AddNotification("Contest edited", NotificationType.SUCCESS);
            return this.RedirectToAction("Index", "AdminOverview");
        }

        private bool EditDeadlineStartegy(ContestBindingModel model, Contest contest)
        {
            if (model.DeadlineStrategyEnum == DeadlineStrategyEnum.EndDate)
            {
                contest.EndDate = model.EndDate;
                contest.ParticipantsLimit = null;
            }
            else
            {
                contest.ParticipantsLimit = model.ParticipantsLimit;
                contest.EndDate = null;
            }

            contest.DeadlineStrategyEnum = model.DeadlineStrategyEnum;

            return true;
        }

        private bool EditRewardStrategy(ContestBindingModel model, Contest contest)
        {
            if (model.RewardStrategyEnum == RewardStrategyEnum.MultipleWinners)
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

            contest.RewardStrategyEnum = model.RewardStrategyEnum;

            return true;
        }

        private bool EditAvailableParticipants(ContestBindingModel model, Contest contest)
        {
            contest.RegisteredParticipants.Clear();

            if (model.ParticipationStrategyEnum == ParticipationStrategyEnum.Closed)
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

            contest.ParticipationStrategyEnum = model.ParticipationStrategyEnum;
            return true;
        }

        private bool EditAvailableVoters(ContestBindingModel model, Contest contest)
        {
            contest.RegisteredVoters.Clear();

            if (model.VotingStrategyEnum == VotingStrategyEnum.Closed)
            {
                if (model.Voters == null || !model.Voters.Any())
                {
                    this.AddNotification("Please select voters.", NotificationType.ERROR);
                    return false;
                }

                var voters = this.Data.Users.GetAll().Where(u => model.Voters.Contains(u.UserName)).ToList();

                contest.RegisteredVoters = new List<User>(voters);
            }

            contest.VotingStrategyEnum = model.VotingStrategyEnum;
            return true;
        }
    }
}