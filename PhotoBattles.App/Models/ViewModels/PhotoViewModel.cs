﻿namespace PhotoBattles.App.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using PhotoBattles.App.Contracts;
    using PhotoBattles.Models;

    public class PhotoViewModel : IMapFrom<Photo>
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public DateTime Uploaded { get; set; }

        public UserViewModel Author { get; set; }

        public ContestViewModel Contest { get; set; }

        public ICollection<VoteViewModel> Votes { get; set; }

        public bool UserCanVote { get; set; }
    }
}