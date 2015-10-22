namespace PhotoBattles.Models.Contracts
{
    public interface IDeadlineStrategy
    {
        void Dismiss(Contest contest);

        void End(Contest contest);
    }
}