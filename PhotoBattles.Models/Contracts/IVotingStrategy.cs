namespace PhotoBattles.Models.Contracts
{
    public interface IVotingStrategy
    {
        bool CanVote(string user);
    }
}