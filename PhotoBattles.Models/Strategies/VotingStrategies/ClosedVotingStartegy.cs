namespace PhotoBattles.Models.Strategies.VotingStrategies
{
    using System.Collections.Generic;

    using PhotoBattles.Models.Contracts;

    public class ClosedVotingStartegy : IVotingStrategy
    {
        private IList<string> commiteeMembers = null;

        public ClosedVotingStartegy(string[] committeeMembers)
        {
            this.commiteeMembers = committeeMembers;
        }

        public bool CanVote(string user)
        {
            if (this.commiteeMembers.Contains(user))
            {
                return true;
            }

            return false;
        }
    }
}