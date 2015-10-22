namespace PhotoBattles.Models.Contracts
{
    public interface IParticipationStrategy
    {
        bool CanParticipate(User user, Contest contest);
    }
}