namespace PhotoBattles.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

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
        public const int RecordsPerPage = 3;

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

            //IQueryable<ContestViewModel> contests =
            //    this.Data.Contests.GetAll()
            //        .OrderByDescending(c => c.IsActive)
            //        .ThenByDescending(c => c.IsOpen)
            //        .ThenByDescending(c => c.CreatedOn)
            //        .ProjectTo<ContestViewModel>();

            //return this.View(contests);
        }

        public ActionResult GetContests(int? pageNum)
        {
            pageNum = pageNum ?? 0;
            this.ViewBag.IsEndOfRecords = false;

            if (this.Request.IsAjaxRequest())
            {
                var contests = this.GetRecordsForPage(pageNum.Value);

                this.ViewBag.IsEndOfRecords = contests.Any() && ((pageNum.Value * RecordsPerPage) >= contests.Last().Key);
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
            IQueryable<ContestViewModel> contests =
                this.Data.Contests.GetAll()
                    .OrderByDescending(c => c.IsActive)
                    .ThenByDescending(c => c.IsOpen)
                    .ThenByDescending(c => c.CreatedOn)
                    .ProjectTo<ContestViewModel>();

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
        [ValidateAntiForgeryToken]
        public ActionResult AddContest(ContestBindingModel model)
        {
            if (!this.ModelState.IsValid || model == null)
            {
                this.AddNotification("Incorrect title or description", NotificationType.ERROR);
                return this.RedirectToAction("Index");
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
                return this.RedirectToAction("Index");
            }

            this.Data.Contests.Add(newContest);
            this.Data.SaveChanges();

            this.AddNotification("Contest created!", NotificationType.SUCCESS);
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
            var contest = this.Data.Contests.GetAll().FirstOrDefault(c => c.Id == id);
            if (contest == null)
            {
                this.AddNotification("Cannot edit contest #" + id, NotificationType.ERROR);
                return this.View("~/Views/Contests/_EditContest.cshtml");
            }

            this.ViewBag.Limit = contest.ParticipantsLimit;
            this.ViewBag.Id = contest.Id;
            return this.View("~/Views/Contests/_EditContest.cshtml");
        }

        [HttpPost]
        public ActionResult EditContest(ContestEditBindigModel model, int id)
        {
            var contest = this.Data.Contests.GetAll().FirstOrDefault(c => c.Id == id);

            if (contest == null)
            {
                this.AddNotification("Cannot edit contest #" + id, NotificationType.ERROR);
                return this.View("~/Views/Contests/_EditContest.cshtml");
            }

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

        private bool SetDeadlineStrategy(ContestBindingModel model, Contest newContest)
        {
            // Deadline Strategy
            if (model.DeadlineStrategy == DeadlineStrategy.EndDate)
            {
                if (model.EndDate == null)
                {
                    this.AddNotification("Incorect end date", NotificationType.ERROR);
                    return false;
                }

                newContest.DeadlineStrategy = DeadlineStrategy.EndDate;
                newContest.EndDate = (DateTime)model.EndDate;
            }
            else if (model.DeadlineStrategy == DeadlineStrategy.ParticipantsLimit)
            {
                if (model.ParticipantsLimit == null || model.ParticipantsLimit <= 0)
                {
                    this.AddNotification("Incorect number of participants", NotificationType.ERROR);
                    return false;
                }

                newContest.DeadlineStrategy = DeadlineStrategy.ParticipantsLimit;
                newContest.ParticipantsLimit = (int)model.ParticipantsLimit;
            }

            return true;
        }

        private bool SetRewardStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.RewardStrategy == RewardStrategy.SingleWinner)
            {
                newContest.RewardStrategy = RewardStrategy.SingleWinner;
                newContest.NumberOfWinners = 1;
            }
            else if (model.RewardStrategy == RewardStrategy.MultipleWinners)
            {
                if (model.NumberOfWinners == null || model.NumberOfWinners <= 0)
                {
                    this.AddNotification("Incorect number of winners", NotificationType.ERROR);
                    return false;
                }

                newContest.RewardStrategy = RewardStrategy.MultipleWinners;
                newContest.NumberOfWinners = (int)model.NumberOfWinners;
            }

            return true;
        }

        private bool SetParticipatingStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.ParticipationStrategy == ParticipationStrategy.Open)
            {
                newContest.ParticipationStrategy = ParticipationStrategy.Open;
            }
            else if (model.ParticipationStrategy == ParticipationStrategy.Closed)
            {
                if (model.Participants == null || !model.Participants.Any())
                {
                    this.AddNotification("Please select participants.", NotificationType.ERROR);
                    return false;
                }

                newContest.ParticipationStrategy = ParticipationStrategy.Closed;

                var participants = this.Data.Users.GetAll().Where(u => model.Participants.Contains(u.UserName)).ToList();

                participants.ForEach(p => newContest.RegisteredParticipants.Add(p));
                newContest.IsOpen = false;
            }

            return true;
        }

        private bool SetVotingStrategy(ContestBindingModel model, Contest newContest)
        {
            if (model.VotingStartegy == VotingStrategy.Open)
            {
                newContest.VotingStrategy = VotingStrategy.Open;
            }
            else if (model.VotingStartegy == VotingStrategy.Closed)
            {
                if (model.Voters == null || !model.Voters.Any())
                {
                    this.AddNotification("Please select voters.", NotificationType.ERROR);
                    return false;
                }

                newContest.VotingStrategy = VotingStrategy.Closed;

                var voters = this.Data.Users.GetAll().Where(u => model.Voters.Contains(u.UserName)).ToList();

                voters.ForEach(v => newContest.RegisteredVoters.Add(v));
            }

            return true;
        }
    }
}