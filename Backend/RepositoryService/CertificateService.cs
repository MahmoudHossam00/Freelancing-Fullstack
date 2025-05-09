using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class CertificateService(ApplicationDbContext context) : ICertificatesService
    {
        public async Task<bool> CreateCertificateAsync(Certificate certificate)
        {            
            await context.certificates.AddAsync(certificate);
            return await context.SaveChangesAsync()>0;            
        }

        public async Task<bool> DeleteCertificateAsync(int id)
        {
            var certificate = await GetCertificateByIDAsync(id);
            if (certificate == null)
            {
                return false;
            }
            certificate.IsDeleted = true;
            context.Update(certificate);
            return await context.SaveChangesAsync()>0;                       
        }

        public async Task<IEnumerable<Certificate>> GetAllCertificatesByFreelancerUserName(string username)
        {
           var certificates = await context.certificates
                .Include(c=>c.Freelancer)
                .Where(c=>c.Freelancer.UserName == username && !c.IsDeleted)
                .ToListAsync();
            return certificates;
        }
        public async Task<IEnumerable<Certificate>> GetAllCertificatesByFreelancerId(string id)
        {
           var certificates = await context.certificates
                .Include(c=>c.Freelancer)
                .Where(c=>c.Freelancer.Id == id && !c.IsDeleted)
                .ToListAsync();
            return certificates;
        }

        public async Task<IEnumerable<Certificate>> GetAllUserCertificatesAsync()
        {
            return await context.certificates
                .Include(c=>c.Freelancer)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }


        public async Task<Certificate> GetCertificateByIDAsync(int id)
        {
            var cert= await context.certificates
                .Include(c=>c.Freelancer)
                .FirstOrDefaultAsync(c => c.Id==id && !c.IsDeleted);
            return cert;
        }
        
        public async Task<bool> UpdateCertificateAsync(Certificate certificate)
        {
            var existingCertificate = await GetCertificateByIDAsync(certificate.Id);
            if (existingCertificate == null)
            {
                return false;
            }
            context.certificates.Update(certificate);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
