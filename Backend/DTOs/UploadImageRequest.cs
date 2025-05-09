namespace Freelancing.DTOs
{
    public class UploadImageRequest
    {
        public IFormFile ImageFile { get; set; }
        public int ProjectId { get; set; }
    }
}
