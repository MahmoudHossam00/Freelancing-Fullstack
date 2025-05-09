namespace Freelancing.DTOs.MilestoneDTOs
{
    public class MilestoneGetAllDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int ProjectId { get; set; } 
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        //public string? File { get; set; }

    }
}
