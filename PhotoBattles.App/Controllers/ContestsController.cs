namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.Identity;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.App.Extensions;
    using PhotoBattles.App.Models.BindingModels;
    using PhotoBattles.App.Models.ViewModels;
    using PhotoBattles.Data.Contracts;
    using PhotoBattles.Models;
    using PhotoBattles.Models.Enumerations;

    [Authorize]
    public class ContestsController : BaseController
    {
        public const int RecordsPerPage = 5;

        public ContestsController()
        {
            this.ViewBag.RecordsPerPage = RecordsPerPage;
        }

        public ContestsController(IPhotoBattlesData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.RedirectToAction("GetContests");
        }

        [AllowAnonymous]
        public ActionResult GetContests(int? pageNum)
        {
            pageNum = pageNum ?? 0;
            this.ViewBag.IsEndOfRecords = false;

            if (this.Request.IsAjaxRequest())
            {
                var contests = this.GetRecordsForPage(pageNum.Value);

                this.ViewBag.IsEndOfRecords = contests.Any()
                                              && ((pageNum.Value * RecordsPerPage) >= contests.Last().Key);
                return this.PartialView("_ContestRow", contests);
            }
            else
            {
                this.LoadAllContestsToSession();
                this.ViewBag.Contests = this.GetRecordsForPage(pageNum.Value);
                return this.View("Index");
            }
        }

        public void LoadAllContestsToSession()
        {
            IEnumerable<ContestViewModel> contests =
                this.Data.Contests.GetAll()
                    .OrderByDescending(c => c.IsActive)
                    .ThenByDescending(c => c.IsOpen)
                    .ThenByDescending(c => c.CreatedOn)
                    .ProjectTo<ContestViewModel>();

            var currentUserName = this.User.Identity.GetUserName();

            foreach (var contest in contests)
            {
                var deadlineStrategy = contest.GetDeadlineStrategy(this.Data.Contests.Find(contest.Id));
                bool hasExpired = deadlineStrategy.Expire();

                if (hasExpired)
                {
                    this.hub.InfoExpiredContest(contest.Title, contest.Id);
                }
            }

            this.Data.SaveChanges();

            int contestIndex = 1;

            this.Session["Contests"] = contests.ToDictionary(c => contestIndex++, c => c);
            this.ViewBag.TotalNumberOfContests = contests.Count();
        }

        public Dictionary<int, ContestViewModel> GetRecordsForPage(int pageNum)
        {
            Dictionary<int, ContestViewModel> contests = this.Session["Contests"] as Dictionary<int, ContestViewModel>;

            int from = pageNum * RecordsPerPage;
            int to = from + RecordsPerPage;

            return contests
                .Where(c => c.Key > from && c.Key <= to)
                .OrderBy(c => c.Key)
                .ToDictionary(c => c.Key, c => c.Value);
        }

        public ActionResult AddContest()
        {
            var model = new ContestViewModel
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

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                this.AddNotification("Incorrect title or description", NotificationType.ERROR);
                return this.RedirectToAction("AddContest");
            }

            string currentUserId = this.UserIdProvider.GetUserId();

            var newContest = new Contest
                {
                    Title = model.Title,
                    Description = model.Description,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    IsOpen = true,
                    OrganizerId = currentUserId
                };

            if (!(this.SetVotingStrategy(model, newContest) &&
                  this.SetParticipatingStrategy(model, newContest) &&
                  this.SetRewardStrategy(model, newContest) &&
                  this.SetDeadlineStrategy(model, newContest)))
            {
                return this.RedirectToAction("AddContest");
            }

            this.Data.Contests.Add(newContest);
            this.Data.SaveChanges();

            this.AddNotification("Contest created!", NotificationType.SUCCESS);
            return this.RedirectToAction("Index", "Contests");
        }

        [HttpGet]
        public ActionResult Participate(int id)
        {
            var contest =
                this.Data.Contests.GetAll().Where(c => c.Id == id).ProjectTo<ContestViewModel>().FirstOrDefault();

            var deadlineStrategy = contest.GetDeadlineStrategy(this.Data.Contests.Find(contest.Id));
            bool hasExpired = deadlineStrategy.Expire();

            if (hasExpired)
            {
                this.Data.SaveChanges();
                this.hub.InfoExpiredContest(contest.Title, contest.Id);
                return this.RedirectToAction("Index");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var currentUserName = this.User.Identity.GetUserName();

            var currentUser = this.Data.Users.Find(currentUserId);
            var participationStrategy = contest.GetParticipationStrategy(this.Data.Contests.Find(contest.Id));

            if (!participationStrategy.CanParticipate(currentUserName))
            {
                this.AddNotification(
                    "You are currently participating in contest " + contest.Title,
                    NotificationType.ERROR);
                return this.RedirectToAction("Index");
            }

            this.Data.Contests.Find(id).Participants.Add(currentUser);

            this.Data.SaveChanges();

            return this.RedirectToAction("ParticipateContests");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int id)
        {
            var currentUserName = this.User.Identity.GetUserName();

            var contest = this.Data.Contests.GetAll()
                              .Where(c => c.Id == id)
                              .OrderByDescending(c => c.CreatedOn)
                              .ProjectTo<ContestDetailsViewModel>()
                              .FirstOrDefault();

            var votingStrategy = contest.GetVotingStrategy(this.Data.Contests.Find(contest.Id));

            contest.Photos.ForEach(p => p.UserCanVote = votingStrategy.CanVote(p.Id, currentUserName));

            return this.View(contest);
        }

        [HttpGet]
        public ActionResult OwnContests()
        {
            var userId = this.User.Identity.GetUserId();

            IQueryable<ContestViewModel> ownContests = this.Data.Contests.GetAll()
                                                           .Where(p => p.Organizer.Id == userId)
                                                           .ProjectTo<ContestViewModel>();

            return this.View("OwnContests", ownContests);
        }

        [HttpGet]
        public ActionResult ParticipateContests()
        {
            var userId = this.User.Identity.GetUserId();

            IQueryable<ContestViewModel> partContests = this.Data.Contests.GetAll()
                                                            .Where(p => p.Participants.Any(u => u.Id == userId))
                                                            .OrderByDescending(c => c.CreatedOn)
                                                            .ProjectTo<ContestViewModel>();

            return this.View("ParticipateContests", partContests);
        }

        [HttpGet]
        public ActionResult Winners(int id)
        {
            var userId = this.User.Identity.GetUserId();

            var winnContests = this.Data.Contests.GetAll()
                                   .Where(p => p.Participants.Any(u => u.Id == userId) && p.Id == id)
                                   .ProjectTo<ContestViewModel>().FirstOrDefault();

            return this.View("Winners", winnContests);
        }

        [HttpGet]
        public ActionResult EditContest(int id)
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

            string currentUserName = this.User.Identity.GetUserName();

            var availableParticipants = this.Data.Users
                                            .GetAll()
                                            .ProjectTo<UserViewModel>()
                                            .ToList();

            contest.AvailableParticipants = new List<UserViewModel>(availableParticipants);

            var availableVoters = availableParticipants
                .Where(u => u.UserName != currentUserName && u.UserName != "Administrator")
                .ToList();

            contest.AvailableVoters = new List<UserViewModel>(availableVoters);

            this.ViewBag.Title = contest.Title;
            return this.View("EditContest", contest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContest(ContestBindingModel model, int id)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                this.AddNotification("Invalid input data", NotificationType.ERROR);
                return this.RedirectToAction("EditContest");
            }

            var contest = this.Data
                              .Contests
                              .GetAll()
                              .FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                this.AddNotification("Cannot find contest", NotificationType.ERROR);
                return this.RedirectToAction("EditContest");
            }

            contest.Title = model.Title;
            contest.Description = model.Description;

            if (!(this.EditAvailableVoters(model, contest) &&
                  this.EditAvailableParticipants(model, contest) &&
                  this.EditRewardStrategy(model, contest) &&
                  this.EditDeadlineStartegy(model, contest)))
            {
                this.RedirectToAction("EditContest");
            }

            this.Data.SaveChanges();

            return this.RedirectToAction("Index", "Contests");
        }

        [HttpPost]
        public ActionResult DismissContest(int id)
        {
            var contest = this.Data.Contests.Find(id);

            contest.IsActive = false;
            contest.IsOpen = false;

            this.Data.SaveChanges();

            this.AddNotification("The contest was dismised", NotificationType.SUCCESS);
            return this.RedirectToAction("OwnContests", "Contests");
        }

        [HttpPost]
        public ActionResult Finalize(int id)
        {
            var contest =
                this.Data.Contests.GetAll().Where(c => c.Id == id).ProjectTo<ContestViewModel>().FirstOrDefault();

            var rewardStrategy = contest.GetRewardStrategy(this.Data.Contests.Find(contest.Id));
            rewardStrategy.SetWinners();

            this.Data.Contests.Find(contest.Id).IsActive = false;
            this.Data.Contests.Find(contest.Id).IsOpen = false;

            this.Data.SaveChanges();

            this.AddNotification("The contest was finalized", NotificationType.SUCCESS);
            return this.RedirectToAction("OwnContests", "Contests");
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

        private bool SetDeadlineStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.DeadlineStrategyEnum == DeadlineStrategyEnum.EndDate)
            {
                if (model.EndDate == null)
                {
                    this.AddNotification("Incorect end date", NotificationType.ERROR);
                    return false;
                }

                newContest.DeadlineStrategyEnum = DeadlineStrategyEnum.EndDate;
                newContest.EndDate = (DateTime)model.EndDate;
            }
            else if (model.DeadlineStrategyEnum == DeadlineStrategyEnum.ParticipantsLimit)
            {
                if (model.ParticipantsLimit == null || model.ParticipantsLimit <= 0)
                {
                    this.AddNotification("Incorect number of participants", NotificationType.ERROR);
                    return false;
                }

                newContest.DeadlineStrategyEnum = DeadlineStrategyEnum.ParticipantsLimit;
                newContest.ParticipantsLimit = (int)model.ParticipantsLimit;
            }

            return true;
        }

        private bool SetRewardStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.RewardStrategyEnum == RewardStrategyEnum.SingleWinner)
            {
                newContest.RewardStrategyEnum = RewardStrategyEnum.SingleWinner;
                newContest.NumberOfWinners = 1;
            }
            else if (model.RewardStrategyEnum == RewardStrategyEnum.MultipleWinners)
            {
                if (model.NumberOfWinners == null || model.NumberOfWinners <= 0)
                {
                    this.AddNotification("Incorect number of winners", NotificationType.ERROR);
                    return false;
                }

                newContest.RewardStrategyEnum = RewardStrategyEnum.MultipleWinners;
                newContest.NumberOfWinners = (int)model.NumberOfWinners;
            }

            return true;
        }

        private bool SetParticipatingStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.ParticipationStrategyEnum == ParticipationStrategyEnum.Open)
            {
                newContest.ParticipationStrategyEnum = ParticipationStrategyEnum.Open;
            }
            else if (model.ParticipationStrategyEnum == ParticipationStrategyEnum.Closed)
            {
                if (model.Participants == null || !model.Participants.Any())
                {
                    this.AddNotification("Please select participants.", NotificationType.ERROR);
                    return false;
                }

                newContest.ParticipationStrategyEnum = ParticipationStrategyEnum.Closed;

                var participants = this.Data.Users.GetAll().Where(u => model.Participants.Contains(u.UserName)).ToList();

                participants.ForEach(p => newContest.RegisteredParticipants.Add(p));
                newContest.IsOpen = false;
            }

            return true;
        }

        private bool SetVotingStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.VotingStrategyEnum == VotingStrategyEnum.Open)
            {
                newContest.VotingStrategyEnum = VotingStrategyEnum.Open;
            }
            else if (model.VotingStrategyEnum == VotingStrategyEnum.Closed)
            {
                if (model.Voters == null || !model.Voters.Any())
                {
                    this.AddNotification("Please select voters.", NotificationType.ERROR);
                    return false;
                }

                var voters = this.Data.Users.GetAll().Where(u => model.Voters.Contains(u.UserName)).ToList();

                voters.ForEach(v => newContest.RegisteredVoters.Add(v));

                newContest.VotingStrategyEnum = VotingStrategyEnum.Closed;
            }

            return true;
        }
    }
}