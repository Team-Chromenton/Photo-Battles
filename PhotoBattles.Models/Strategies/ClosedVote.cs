namespace PhotoBattles.Models.Strategies
{
    using PhotoBattles.Models.Contracts;

    public class ClosedVote : IVotingStrategy
    {
        public bool CanVote(User user, Contest contest)
        {
            if (contest.VotingUsers.Contains(user))
            {
                return true;
            }

            return false;
        }
    }
}