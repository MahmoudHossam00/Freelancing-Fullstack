namespace Freelancing.DTOs.MilestoneDTOs
{
    public class MilestoneCreateDTO
    {
        public int? Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int? ProjectId { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; } //duration in days

        //public string? File { get; set; }

    }
}
