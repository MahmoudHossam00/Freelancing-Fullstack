using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class ChatRepositoryService : IChatRepositoryService
    {
        private readonly ApplicationDbContext _context;

        public ChatRepositoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Chat?> GetChatByIdAsync(int id)
        {
            return await _context.Chats
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Chat>> GetChatsBySenderIdAsync(string senderId)
        {
            return await _context.Chats
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .Where(c => c.SenderId == senderId)
                .OrderBy(c => c.SentAt)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetChatsByReceiverIdAsync(string receiverId)
        {
            return await _context.Chats
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .Where(c => c.ReceiverId == receiverId)
                .OrderBy(c => c.SentAt)
                .ToListAsync();
        }

        public async Task<List<Chat>> GetConversationAsync(string userId1, string userId2)
        {

            var chats = _context.Chats
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .Where(c => (c.SenderId == userId1 && c.ReceiverId == userId2) ||
                           (c.SenderId == userId2 && c.ReceiverId == userId1));
            var x = chats.OrderBy(c => c.SentAt);
            if(x.Count()==0)
            {
                return new List<Chat>();
            }
            var y = await x.ToListAsync();

            return y;
        }
        //public async Task<Chat> CreateChatAsync(Chat chat)
        //{
        //    var senderExists = await _context.Users.AnyAsync(u => u.Id == chat.SenderId);
        //    var receiverExists = await _context.Users.AnyAsync(u => u.Id == chat.ReceiverId);
        //    if (!senderExists || !receiverExists)
        //    {
        //        throw new ArgumentException("Sender or Receiver does not exist.");
        //    }

        //    chat.SentAt = DateTime.UtcNow;
        //    chat.isRead = false;
        //    _context.Chats.Add(chat);
        //    await _context.SaveChangesAsync();
        //    return chat;
        //}

        public async Task<Chat> CreateChatAsync(Chat chat)
        {
            chat.SentAt = DateTime.UtcNow;
            chat.isRead = false;
            _context.Chats.Add(chat);
            await _context.SaveChangesAsync();

            return await _context.Chats
                .Include(c => c.Sender)
                .Include(c => c.Receiver)
                .FirstOrDefaultAsync(c => c.Id == chat.Id);
        }
        public async Task UpdateChatAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteChatAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                if(chat.ImageUrl!=null)
                {
                    SaveImage.Delete(chat.ImageUrl);

				}
                _context.Chats.Remove(chat);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(int id)
        {
            var chat = await _context.Chats.FindAsync(id);
            if (chat != null)
            {
                chat.isRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
