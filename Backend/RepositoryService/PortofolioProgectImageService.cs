using Freelancing.DTOs;
using Freelancing.IRepositoryService;
//using Freelancing.Migrations;
using Freelancing.Models;

namespace Freelancing.RepositoryService
{
    public class PortofolioProgectImageService(ApplicationDbContext context) : IPortofolioProjectImage
    {

        public async Task<List<PortofolioProjectImage>> GetByPortfolioProjectIdAsync(int id)
        {
            var project = context.PortofolioProjects.SingleOrDefault(p => p.Id == id);
            if (project == null)
            {
                return null;
            }
                var images =await context.PortofolioProjectImages.Where(i => i.PreviousProjectId == project.Id && !i.IsDeleted).ToListAsync();
            return images;
        }



        public async Task<PortofolioProjectImage> AddAsync(PortofolioProjectImageDTO portofolioProjectImage)
        {
            if (portofolioProjectImage == null)
            {
                throw new ArgumentNullException(nameof(portofolioProjectImage));
            }

            var p = new PortofolioProjectImage()
            {
                Image = portofolioProjectImage.Image,
                PreviousProjectId = portofolioProjectImage.PreviousProjectId,
                IsDeleted=false
            };
            context.PortofolioProjectImages.Add(p);
            await context.SaveChangesAsync();
            return p;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var image = context.PortofolioProjectImages.SingleOrDefault(p => p.Id == id );
            if(image is not null)
            {
                image.IsDeleted = true;
                //context.Update(image);
                context.Remove(image);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
