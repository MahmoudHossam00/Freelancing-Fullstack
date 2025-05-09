using Microsoft.AspNetCore.Mvc;

namespace Freelancing.IRepositoryService
{
    public interface ICertificatesService
    {
        Task<IEnumerable<Certificate>> GetAllUserCertificatesAsync();
        Task<IEnumerable<Certificate>> GetAllCertificatesByFreelancerUserName(string username);
        Task<IEnumerable<Certificate>> GetAllCertificatesByFreelancerId(string id);
        Task<Certificate> GetCertificateByIDAsync(int id);
        Task<bool> UpdateCertificateAsync(Certificate certificate);
        Task<bool> CreateCertificateAsync(Certificate certificate);
        Task<bool> DeleteCertificateAsync(int id);
    }
}
