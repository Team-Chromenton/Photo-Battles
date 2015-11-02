namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    [Authorize]
    public class ContestsController : BaseController
    {
        public ContestsController()
        {
        }

        public ContestsController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

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

            var availableVoters =
                availableParticipants.Where(u => u.UserName != currentUserName && u.UserName != "Administrator")
                                     .ToList();
            availableVoters.ForEach(u => model.AvailableVoters.Add(u));

            return this.PartialView("~/Views/Contests/_AddContest.cshtml", model);
        }

        [HttpGet]
        public ActionResult Participate(int id)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var contest = this.Data.Contests.Find(id);
            var user = this.Data.Users.Find(userId);

            if (contest.ParticipationStrategy == ParticipationStrategy.Closed
                && !contest.RegisteredParticipants.Contains(user))
            {
                return this.RedirectToAction("Index");
            }

            contest.Participants.Add(user);

            this.Data.SaveChanges();

            return this.RedirectToAction("ParticipateContests");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var contest = this.Data.Contests.GetAll()
                              .Where(c => c.Id == id)
                              .OrderByDescending(c => c.CreatedOn)
                              .ProjectTo<ContestDetailsViewModel>()
                              .FirstOrDefault();

            return this.View(contest);
        }

        [HttpPost]
        public ActionResult AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("Index");
            }

            string currentUserId = this.User.Identity.GetUserId();

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

        [HttpGet]
        public ActionResult ParticipateContests()
        {
            var userId = this.User.Identity.GetUserId();

            IQueryable<ContestViewModel> partContests = this.Data.Contests.GetAll()
                                                            .Where(p => p.Participants.Any(u => u.Id == userId))
                                                            .OrderByDescending(c => c.CreatedOn)
                                                            .ProjectTo<ContestViewModel>();

            return this.View("~/Views/Contests/_ParticipateContests.cshtml", partContests);
        }

        [HttpGet]
        public ActionResult Winners(int id)
        {
            var userId = this.User.Identity.GetUserId();

            var winnContests = this.Data.Contests.GetAll()
                                   .Where(p => p.Participants.Any(u => u.Id == userId) && p.Id == id)
                                   .ProjectTo<ContestViewModel>().FirstOrDefault();

            return this.View("~/Views/Contests/_Winners.cshtml", winnContests);
        }

        [HttpGet]
        public ActionResult OwnContests()
        {
            var userId = this.User.Identity.GetUserId();

            //IQueryable<ContestViewModel> ownContests = this.Data.Contests.GetAll()
            //    .Where(p => p.Participants.Any(u => u.Id == user.Id))
            //    .ProjectTo<ContestViewModel>();

            //This if we want to show only the from user created  contests should be discuss
            IQueryable<ContestViewModel> ownContests = this.Data.Contests.GetAll()
                                                           .Where(p => p.Organizer.Id == userId)
                                                           .ProjectTo<ContestViewModel>();

            return this.View("~/Views/Contests/_OwnContests.cshtml", ownContests);
        }

        [HttpGet]
        public ActionResult EditContest(int id)
        {
            var contest = this.Data.Contests.GetAll().Where(c => c.Id == id).FirstOrDefault();

            this.ViewBag.Limit = contest.ParticipantsLimit;
            this.ViewBag.Id = contest.Id;
            return this.View("~/Views/Contests/_EditContest.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> EditContest(ContestEditBindigModel model, int id)
        {
            var contest = this.Data.Contests.GetAll().FirstOrDefault(c => c.Id == id);

            contest.Title = model.Title;
            contest.Description = model.Description;
            contest.EndDate = model.EndDate;
            contest.ParticipantsLimit = model.ParticipantsLimit;
            this.Data.SaveChanges();

            return this.RedirectToAction("OwnContests", "Contests");
        }

        [HttpPost]
        public ActionResult DismissContest(int id)
        {
            var contest = this.Data.Contests.Find(id);
            contest.Dismiss();
            this.Data.SaveChanges();
            return this.RedirectToAction("OwnContests", "Contests");
        }

        [HttpPost]
        public ActionResult Finalize(int id)
        {
            var contextFinalize = this.Data.Contests.Find(id);
            contextFinalize.End();
            if (contextFinalize.NumberOfWinners > 1)
            {
                var winners =
                    contextFinalize.Participants.Where(p => p.Votes.Count > 0)
                                   .OrderByDescending(p => p.Votes.Count)
                                   .Take(contextFinalize.NumberOfWinners);
                contextFinalize.Winners = winners.ToList();
            }
            else
            {
                var winner =
                    contextFinalize.Participants.Where(p => p.Votes.Count > 0)
                                   .OrderByDescending(p => p.Votes.Count)
                                   .FirstOrDefault();
                contextFinalize.Winners = new List<User>()
                    {
                        winner
                    };
            }

            this.Data.SaveChanges();
            return this.RedirectToAction("OwnContests", "Contests");
        }
    }
}