namespace Freelancing.DTOs
{
    public class BanDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime BanDate { get; set; }
        public DateTime BanEndDate { get; set; }
        public string BannedUserId { get; set; }
        public string BannedUserName { get; set; }
    }
}
