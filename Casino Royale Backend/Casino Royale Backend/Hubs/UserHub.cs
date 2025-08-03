using Microsoft.AspNetCore.SignalR;

namespace Casino_Royale_Backend.Hubs;

public class UserHub : Hub
{
    public static int TotalViews { get; set; } = 0;
    public static int TotalUsers { get; set; } = 0;


    
    public async Task NewWindowOpened()
    {
        TotalViews++;
        await Clients.All.SendAsync("updatedTotalViews", TotalViews);
    }

    public override Task OnConnectedAsync()
    {
        TotalUsers++;
        Clients.All.SendAsync("updatedTotalUsers", TotalUsers).GetAwaiter().GetResult();
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        TotalUsers--;
        Clients.All.SendAsync("updatedTotalUsers", TotalUsers).GetAwaiter().GetResult();
        return base.OnDisconnectedAsync(exception);
    }
}
