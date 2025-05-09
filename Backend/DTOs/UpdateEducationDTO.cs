namespace Freelancing.DTOs
{
    public class UpdateEducationDTO
    {
        public string Degree { get; set; } // e.g., "Bachelor of Science"
        public string FieldOfStudy { get; set; } // e.g., "Computer Science"
        public string Institution { get; set; } // e.g., "MIT"

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Nullable for ongoing education

        public string Grade { get; set; } // Optional, e.g., "3.8 GPA"
        public string Description { get; set; } // Optional summary of coursework or honors
        
    }
}
