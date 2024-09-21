using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Security.Claims;

namespace Infrastructure.SignalR;

public class ShopHub : Hub
{
    public async Task SendOrderUpdate(string message)
    {
        // Gửi thông báo cập nhật đơn hàng tới tất cả các client đang kết nối
        await Clients.All.SendAsync("ReceiveOrderUpdate", message);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier; // Đảm bảo UserIdentifier được gán giá trị userId
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}
public class CustomUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        // Lấy userId từ Claims hoặc nơi bạn lưu trữ userId
        return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
