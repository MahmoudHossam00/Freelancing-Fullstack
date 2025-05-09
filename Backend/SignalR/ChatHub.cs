using System.Security.Claims;
using Freelancing.DTOs;
using Freelancing.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.SignalR
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public ChatHub(ApplicationDbContext context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task SendMessage(ChatDto chatDto)
        {
            //var senderId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            //  ?? Context.User.FindFirst("sub")?.Value;
            var senderId = Context.UserIdentifier;

            if (chatDto == null || string.IsNullOrEmpty(chatDto.ReceiverId))
            {
                throw new HubException("Invalid message data");
            }

            await Clients.Users(senderId, chatDto.ReceiverId)
                .SendAsync("ReceiveMessage", chatDto);
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            // Add new connection without removing existing ones
            var userConnection = new UserConnection
            {
                UserId = userId,
                ConnectionId = connectionId,
                ConnectedAt = DateTime.UtcNow,
                IsConnected = true
            };

            _context.UserConnections.Add(userConnection);
            await _context.SaveChangesAsync();

            // Get all active connections for this user
            var connections = await _context.UserConnections
                .Where(uc => uc.UserId == userId && uc.IsConnected)
                .ToListAsync();

            // Only notify if this is the first connection
            if (connections.Count == 1)
            {
                await Clients.All.SendAsync("UserStatusChanged", userId, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            var connection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.ConnectionId == connectionId);

            if (connection != null)
            {
                connection.IsConnected = false;
                _context.UserConnections.Update(connection);
                await _context.SaveChangesAsync();
            }

            // Check remaining active connections
            var activeConnections = await _context.UserConnections
                .CountAsync(uc => uc.UserId == userId && uc.IsConnected);

            if (activeConnections == 0)
            {
                await Clients.All.SendAsync("UserStatusChanged", userId, false);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendTypingNotification(string receiverId)
        {
            var senderId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(receiverId) && !string.IsNullOrEmpty(senderId))
            {
                await Clients.User(receiverId).SendAsync("UserTyping", senderId);
            }
        }


        public async Task SendConversationDeleted(string userId1, string userId2)
        {
            await Clients.Users(userId1, userId2)
                .SendAsync("ConversationDeleted", userId1, userId2);
        }

        public class CustomUserIdProvider : IUserIdProvider
        {
            public string GetUserId(HubConnectionContext connection)
            {
                // Assuming your JWT uses the NameIdentifier (sub or userId) claim
                return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
        }


        public async Task SendReadReceipt(string receiverId)
        {
            var senderId = Context.UserIdentifier;
            await Clients.User(receiverId).SendAsync("MessageRead", senderId);
        }

        public async Task MessageUpdated(ChatDto chatDto)
        {
            await Clients.Users(chatDto.SenderId, chatDto.ReceiverId)
                .SendAsync("MessageUpdated", chatDto);
        }


        public async Task SendDeleteNotification(int messageId, string senderId, string receiverId)
        {
            await Clients.Users(senderId, receiverId)
                .SendAsync("MessageDeleted", messageId);
        }


        public async Task InitiateVideoCall(string receiverId)
        {
            var senderId = Context.UserIdentifier;
            if (string.IsNullOrEmpty(receiverId) || string.IsNullOrEmpty(senderId))
            {
                throw new HubException("Invalid user IDs");
            }

            await Clients.User(receiverId).SendAsync("ReceiveVideoCall", senderId);
        }

        public async Task SendOffer(string receiverId, object offer)
        {
            var senderId = Context.UserIdentifier;
            await Clients.User(receiverId).SendAsync("ReceiveOffer", senderId, offer);
        }

        public async Task SendAnswer(string receiverId, object answer)
        {
            var senderId = Context.UserIdentifier;
            await Clients.User(receiverId).SendAsync("ReceiveAnswer", senderId, answer);
        }

        public async Task SendIceCandidate(string receiverId, object candidate)
        {
            var senderId = Context.UserIdentifier;
            await Clients.User(receiverId).SendAsync("ReceiveIceCandidate", senderId, candidate);
        }

        public async Task EndVideoCall(string receiverId)
        {
            var senderId = Context.UserIdentifier;
            await Clients.Users(senderId, receiverId).SendAsync("VideoCallEnded");
        }

    }
}