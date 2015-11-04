namespace PhotoBattles.Models.Contracts
{
    public interface IParticipationStrategy
    {
        bool CanParticipate(string userName);
    }
}