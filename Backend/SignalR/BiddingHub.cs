using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Freelancing.SignalR
{
	public class BiddingHub : Hub
	{
		public async Task JoinBidGroup(int projectId)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, projectId.ToString());
		}
		public override async Task OnConnectedAsync()
		{
			//await Groups.AddToGroupAsync(Context.ConnectionId,projectid);
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			//await Groups.RemoveFromGroupAsync(Context.ConnectionId, "BiddingViewers");
			await base.OnDisconnectedAsync(exception);
		}
	}
}
