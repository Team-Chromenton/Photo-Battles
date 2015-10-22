namespace PhotoBattles.Models.Contracts
{
    public interface IDeadlineStrategy
    {
        void Dismiss();

        void End();
    }
}