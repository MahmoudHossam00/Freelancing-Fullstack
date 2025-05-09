using AutoMapper;
using Freelancing.DTOs.MilestoneDTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class MilestoneService(ApplicationDbContext context, IMapper mapper) : IMilestoneService
    {

        public async Task<List<MilestoneGetAllDTO>> GetAllAsync()
        {
            var milestones = await context.Milestones.Where(m => !m.IsDeleted).ToListAsync();
            var milestoneDTOs = mapper.Map<List<MilestoneGetAllDTO>>(milestones);
            return milestoneDTOs;
        }

        public async Task<MilestoneGetAllDTO> GetByIdAsync(int id)
        {
            var milestone = await context.Milestones.SingleOrDefaultAsync(m => m.Id == id);
            var milestoneDTO = mapper.Map<MilestoneGetAllDTO>(milestone);
            return milestoneDTO;
        }


        public async Task<MilestoneCreateDTO> CreateAsync(MilestoneCreateDTO milestone)
        {

            Milestone ms = mapper.Map<Milestone>(milestone); //<destination>
            await context.Milestones.AddAsync(ms);
            await context.SaveChangesAsync();
            return milestone;

        }

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    var milestone = context.Milestones.SingleOrDefault(m => m.Id == id && !m.IsDeleted);
        //    if (milestone != null)
        //    {
        //        milestone.IsDeleted = true;
        //        context.Milestones.Update(milestone);
        //        await context.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;

        //}



        public async Task<MilestoneGetAllDTO> UpdateStatusAsync(int MilestoneId, int StatusId)
        {
            var milestone = context.Milestones.Include(m=>m.Project).ThenInclude(p=>p.Freelancer).FirstOrDefault(m => m.Id == MilestoneId);
            var freelancer = milestone.Project.Freelancer;

            if(milestone is not null)
            {
                milestone.Status = (MilestoneStatus)StatusId;
                if(milestone.Status== MilestoneStatus.Completed){
                    milestone.EndDate = DateTime.Now;
                    var project = context.project.Include(p=>p.Milestones).FirstOrDefault(P=>P.Id==milestone.ProjectId);
                    var nextmilestone = project.Milestones.OrderBy(m=>m.Id).SkipWhile(m => m.Id != milestone.Id).Skip(1).FirstOrDefault();


                    if (nextmilestone is not null) {
                        nextmilestone.StartDate = DateTime.Now;
                    }
                    freelancer.Balance += milestone.Amount;
                }

                await context.SaveChangesAsync();
                if(milestone.Project.Status==projectStatus.Completed)
                {
                    milestone.Project.EndDate = DateTime.Now;
                    context.Update(milestone.Project);
                    context.SaveChanges();
                }
                var milestoneDto = mapper.Map<MilestoneGetAllDTO>(milestone);
                return milestoneDto;
            }
            throw new Exception("Milestone is not found");



            //var ms = context.Milestones.SingleOrDefault(m => m.Id == milestone.Id);
            //if(ms is not null)
            //{
            //    mapper.Map(milestone, ms);
            //    await context.SaveChangesAsync();

            //}
                //ms.Title = milestone.Title;
                //ms.Description = milestone.Description;
                //ms.Amount = milestone.Amount;
                //ms.ProjectId = milestone.ProjectId;
                //ms.Status = milestone.Status;
                //ms.StartDate = milestone.StartDate;
                //ms.EndDate = milestone.EndDate;

                //await context.SaveChangesAsync();
                //return ms;
        }

        public async Task<List<MilestoneGetByIdOrProjectIdDTO>> GetByProjectId(int id)
        {
            Project project = context.fixedPriceProjects.SingleOrDefault(p => p.Id == id) == null ?
             context.biddingProjects.SingleOrDefault(p => p.Id == id) : context.fixedPriceProjects.SingleOrDefault(p => p.Id == id);



            if (project is not null)
            {
                if (project.Milestones is not null)
                {
                    var milestones = await context.Milestones.Where(m => m.ProjectId == id).ToListAsync();
                    var milestonerDTO = mapper.Map<List<MilestoneGetByIdOrProjectIdDTO>>(milestones);
                    return milestonerDTO;
                }
                throw new InvalidOperationException("This project doesn't have any milestones.");
            }
            throw new KeyNotFoundException("Project not found.");


        }

        public async Task<bool> DeleteAsync(int id)
        {
            var milestone = await context.Milestones.FirstOrDefaultAsync(m => m.Id == id);
            if(milestone is not null)
            {
                milestone.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }

            throw new Exception("milestone is not found");
            
        }

        public async Task<MilestoneGetAllDTO> UpdateAsync(MilestoneCreateDTO milestone)
        {
            var ms = context.Milestones.FirstOrDefault(m => m.Id == milestone.Id);
            if (ms is not null)
            {
                mapper.Map(milestone, ms);
                await context.SaveChangesAsync();
                return mapper.Map<MilestoneGetAllDTO>(ms);
            }
            throw new Exception("Milestone is not found");
        }


        //[HttpPost]
        public async Task<List<string>> UploadFile(List<IFormFile> files, int MilestoneId)
        {
            List<string> FilesNames = new List<string>();
            foreach (var file in files)
            {
                MilestoneFile mf = new MilestoneFile() { MilestoneId = MilestoneId, fileName = file.Save() };
                context.MilestoneFiles.Add(mf);
                FilesNames.Add(mf.fileName);
            }
            await context.SaveChangesAsync();
            return FilesNames;
        }


        public async Task<bool> RemoveFile(int FileId)
        {
            var file = context.MilestoneFiles.FirstOrDefault(f => f.id == FileId);
            if(file is not null)
            {
                context.MilestoneFiles.Remove(file);
                await context.SaveChangesAsync();
                SaveImage.Delete(file.fileName);
                return true;
            }
            return false;
        }

		public async Task<bool> RemoveFilebyName(string name)
		{
			var file = context.MilestoneFiles.FirstOrDefault(f => f.fileName.EndsWith(name));
			if (file is not null)
			{
				context.MilestoneFiles.Remove(file);
				await context.SaveChangesAsync();
				SaveImage.Delete(file.fileName);
				return true;
			}
			return false;
		}

		public async Task<List<MilestoneFile>> GetFilesByMilestoneId(int MilsestoneId)
        {
            var milestone =await context.Milestones.Include(m=>m.MilestoneFiles).FirstOrDefaultAsync(m => m.Id == MilsestoneId);
            if(milestone is not null)
            {
                return milestone.MilestoneFiles.ToList();
            }
            throw new Exception("Milestone not found");
        }
    }
}
