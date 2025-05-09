namespace Freelancing.IRepositoryService
{
    public interface IChatRepositoryService
    {
        Task<Chat> GetChatByIdAsync(int id);
        Task<List<Chat>> GetChatsBySenderIdAsync(string senderId);
        Task<List<Chat>> GetChatsByReceiverIdAsync(string receiverId);
        Task<List<Chat>> GetConversationAsync(string userId1, string userId2);
        Task<Chat> CreateChatAsync(Chat chat);
        Task UpdateChatAsync(Chat chat);
        Task DeleteChatAsync(int id);
        Task MarkAsReadAsync(int id);
    }
}
