using AutoMapper;
using Freelancing.DTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class PortofolioProjectService(ApplicationDbContext context,IPortofolioProjectImage _images, IMapper _mapper) : IPortofolioProject
    {

        public async Task<List<PortofolioProject>> GetAllAync()
        {
            return await context.PortofolioProjects.Include(p => p.Images).Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<PortofolioProject> GetByIdAsync(int id)
        {
            return await context.PortofolioProjects.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PortofolioProject>> GetByFreelancerId(string id)
        {
            var freelancer = context.freelancers.SingleOrDefault(f => f.Id == id);
            if(freelancer is not null)
            {
                var PortfolioProjects= await context.PortofolioProjects.Include(p=>p.Images).Where(p => p.FreelancerId == id&&!p.IsDeleted).ToListAsync();
                return PortfolioProjects;
            }
            else
            {
                return null;
            }
                throw new KeyNotFoundException("Invalid Freelancer Id");

        }




        public async Task<PortofolioProject> AddAsync(CreatePortfolioProjectDTO portofolioProject, string freelancerid)
        {
            PortofolioProject p = new PortofolioProject()
            {
                Title = portofolioProject.Title,
                Description = portofolioProject.Description,
                CreatedAt = portofolioProject.CreatedAt,
                FreelancerId = freelancerid
            };
            await context.PortofolioProjects.AddAsync(p);
            await context.SaveChangesAsync();
            if (portofolioProject.Images is not null)
            {
                foreach (var item in portofolioProject.Images)
                {
                    await _images.AddAsync(new PortofolioProjectImageDTO()

                    {
                        PreviousProjectId = p.Id,
                        Image = item.Save2()
                    });
                }
            }
			return p;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var portfolioPrject = context.PortofolioProjects.SingleOrDefault(p => p.Id == id);
            if(portfolioPrject is not null)
            {
                portfolioPrject.IsDeleted = true;
                context.PortofolioProjects.Update(portfolioPrject);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

   

        public async Task<PortofolioProject> UpdateAsync(PortofolioProjectDTO portofolioProject)
        {
            var p = context.PortofolioProjects.SingleOrDefault(p => p.Id == portofolioProject.Id);
            if (p is not null)
            {
                p.Title = portofolioProject.Title;
                p.Description = portofolioProject.Description;
                p.CreatedAt = portofolioProject.CreatedAt;
                //p.Images = portofolioProject.Images;
                //_mapper.Map(portofolioProject, p);

                await context.SaveChangesAsync();
                return p;
            }

            throw new KeyNotFoundException("Invalid Portfolio Project");
           
        }

        public Task<PortofolioProject> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
