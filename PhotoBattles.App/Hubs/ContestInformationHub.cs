namespace PhotoBattles.App.Hubs
{
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [HubName("contestInfoHub")]
    public class ContestInformationHub : Hub
    {
        public void InfoExpiredContest(string contestTitle, int contestId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ContestInformationHub>();
            hubContext.Clients.All.infoExpiredContest(contestTitle, contestId);
        }
    }
}