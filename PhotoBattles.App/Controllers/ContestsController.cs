namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using Models.BindingModels;
    using Models.ViewModels;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

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
            var model = new ContestBindingModel
            {
                Title = string.Empty,
                Description = string.Empty,
                AvailableParticipants = new List<UserViewModel>(),
                AvailableVoters = new List<UserViewModel>()
            };

            string currentUserName = System.Web.HttpContext.Current.User.Identity.GetUserName();

            var availableParticipants = this.Data.Users
                .GetAll()
                .ProjectTo<UserViewModel>()
                .ToList();
            availableParticipants.ForEach(p => model.AvailableParticipants.Add(p));

            var availableVoters = availableParticipants.Where(u => u.UserName != currentUserName && u.UserName != "Administrator").ToList();
            availableVoters.ForEach(u => model.AvailableVoters.Add(u));

            return this.PartialView("~/Views/Contests/_AddContest.cshtml", model);
        }

        [HttpGet]
        public ActionResult Participate(int id)
        {
            var userId = this.User.Identity.GetUserId();
            var contest = this.Data.Contests.Find(id);
            var user = this.Data.Users.Find(userId);

            if (contest.ParticipationStrategy == ParticipationStrategy.Closed && !contest.RegisteredParticipants.Contains(user))
            {
                return RedirectToAction("Index");
            }

            contest.Participants.Add(user);

            this.Data.SaveChanges();

            return this.RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var contest = this.Data.Contests.GetAll()
                .Where(c => c.Id == id)
                .ProjectTo<ContestDetailsViewModel>()
                .FirstOrDefault();

            return this.View(contest);
        }

        [HttpPost]
        public async Task<ActionResult> AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return null;
            }

            string currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

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
            if (model.VotingStartegy == VotingStrategy.Open)
            {
                newContest.VotingStrategy = VotingStrategy.Open;
            }
            else if (model.VotingStartegy == VotingStrategy.Closed)
            {
                newContest.VotingStrategy = VotingStrategy.Closed;

                var voters = this.Data.Users.GetAll().Where(u => model.Voters.Contains(u.UserName)).ToList();

                voters.ForEach(v => newContest.RegisteredVoters.Add(v));
            }

            // Participation Strategy
            if (model.ParticipationStrategy == ParticipationStrategy.Open)
            {
                newContest.ParticipationStrategy = ParticipationStrategy.Open;
            }
            else if (model.ParticipationStrategy == ParticipationStrategy.Closed)
            {
                newContest.ParticipationStrategy = ParticipationStrategy.Closed;

                var participants = this.Data.Users.GetAll().Where(u => model.Participants.Contains(u.UserName)).ToList();

                participants.ForEach(p => newContest.RegisteredParticipants.Add(p));
                newContest.IsOpen = false;
            }

            // Reward Strategy
            if (model.RewardStrategy == RewardStrategy.SingleWinner)
            {
                newContest.RewardStrategy = RewardStrategy.SingleWinner;
                newContest.NumberOfWinners = 1;
            }
            else if (model.RewardStrategy == RewardStrategy.MultipleWinners)
            {
                newContest.RewardStrategy = RewardStrategy.MultipleWinners;
                newContest.NumberOfWinners = (int)model.NumberOfWinners;
            }

            // Deadline Strategy
            if (model.DeadlineStrategy == DeadlineStrategy.EndDate)
            {
                newContest.DeadlineStrategy = DeadlineStrategy.EndDate;
                newContest.EndDate = (DateTime)model.EndDate;
            }
            else if (model.DeadlineStrategy == DeadlineStrategy.ParticipantsLimit)
            {
                newContest.DeadlineStrategy = DeadlineStrategy.ParticipantsLimit;
                newContest.ParticipantsLimit = (int)model.ParticipantsLimit;
            }

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Contests");
        }
    }
}