using System.ComponentModel.DataAnnotations.Schema;

namespace Freelancing.Models
{
    public class UserConnection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }
        public DateTime ConnectedAt { get; set; }
        public bool IsConnected { get; set; }

        public AppUser User { get; set; }

     
    }
}
