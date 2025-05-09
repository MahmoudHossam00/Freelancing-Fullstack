namespace Freelancing.DTOs
{
    public class CreateCertificateDTO
    {
        public string Name { get; set; }       
        public string Issuer { set; get; }
        public DateTime IssueDate { get; set; }
        
    }
}
