namespace Freelancing.DTOs.MilestoneDTOs
{
    public class MilestoneGetByIdOrProjectIdDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } 
        public List<string>? File { get; set; }

    }
}
