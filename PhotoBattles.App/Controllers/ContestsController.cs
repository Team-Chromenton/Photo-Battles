namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Models;

    [Authorize]
    public class ContestsController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            IQueryable<ContestViewModel> contests =
                this.Data.Contests.GetAll()
                    .OrderByDescending(c => c.IsActive)
                    .ThenByDescending(c => c.IsOpen)
                    .ThenByDescending(c => c.CreatedOn)
                    .ProjectTo<ContestViewModel>();

            return this.View(contests);
        }

        public ActionResult AddContest()
        {
            var model = new ContestBindingModel { Title = "", Description = "", AvailableUsers = new List<UserViewModel>()};

            string currentUserName = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var availableUsers = this.Data.Users
                .GetAll()
                .Where(u => u.UserName != currentUserName && u.UserName != "Administrator")
                .ProjectTo<UserViewModel>()
                .ToList();

            availableUsers.ForEach(u => model.AvailableUsers.Add(u));

            return this.PartialView("~/Views/Contests/_AddContest.cshtml", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return null;
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            string currentUserName = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var currentUser =
                this.Data.Users.GetAll().Where(u => u.Id == currentUserId).ProjectTo<UserViewModel>().FirstOrDefault();

            var newContest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsOpen = true,
                OrganizerId = currentUserId
            };

            this.Data.Contests.Add(newContest);

            // Voting Strategy
            if (model.VotingStartegy == "Open")
            {
                newContest.VotingStrategy = "Open";
            }
            else if (model.VotingStartegy == "Closed")
            {
                newContest.VotingStrategy = "Closed";

                var voters = this.Data.Users.GetAll().Where(u => model.VotersNames.Contains(u.UserName)).ToList();

                voters.ForEach(v => newContest.RegisteredVoters.Add(v));
            }

            //// Participation Strategy
            // if (model.ParticipationStrategy == "Closed")
            // {
            // string[] participantsNames = model.Participants.Split(',').Select(p => p.Trim()).ToArray();
            // var participants = this.Data.Users
            // .GetAll()
            // .Where(u => participantsNames.Contains(u.UserName))
            // .ToList();

            // participants.ForEach(p => newContest.Participants.Add(p));
            // newContest.IsOpen = false;
            // }

            //// Reward Strategy
            // if (model.RewardStrategy == "SingleWinner")
            // {
            // newContest.NumberOfWinners = 1;
            // }
            // else if (model.RewardStrategy == "TopNWinners")
            // {
            // newContest.NumberOfWinners = (int) model.NumberOfWinners;
            // }

            //// Deadline Strategy
            // if (model.DeadlineStrategy == "ByTime")
            // {
            // newContest.Deadline = model.Deadline;
            // }
            // else if (model.RewardStrategy == "ByParticipants")
            // {
            // newContest.NumberOfParticipants = model.NumberOfParticipants;
            // }
            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Contests");
        }
    }
}