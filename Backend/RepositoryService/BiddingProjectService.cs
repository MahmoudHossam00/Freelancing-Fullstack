using AutoMapper;
using CloudinaryDotNet.Actions;
using Freelancing.DTOs;
using Freelancing.DTOs.AuthDTOs;
using Freelancing.DTOs.BiddingProjectDTOs;
using Freelancing.IRepositoryService;
using Freelancing.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NuGet.Packaging.PackagingConstants;

namespace Freelancing.RepositoryService
{
    public class BiddingProjectService : IBiddingProjectService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISubcategoryService _subcategoryService;
        private readonly UserManager<AppUser> _userManager;

        public BiddingProjectService(ApplicationDbContext context, IMapper mapper, ISubcategoryService subcategoryService, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _subcategoryService = subcategoryService;
            _userManager = userManager;
        }



            //var projects = (await _context.biddingProjects

            //                             .Include(p => p.Proposals)
            //                             .Include(p => p.ProjectSkills)
            //                             .ThenInclude(ps => ps.Skill)
            //                             .Include(b => b.Client).ThenInclude(c => c.Reviewed)
            //                             .Where(p => !p.IsDeleted)
            //                             .ToListAsync())
            //                             .Where(p => p.Status == projectStatus.Pending)
            //                             .ToList();

            //var projectsDTO = _mapper.Map<List<BiddingProjectGetAllDTO>>(projects);
            //return projectsDTO;


        public async Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsync(BiddingProjectFilterDTO filters, int pageNumber, int PageSize)
        {


            var query = _context.biddingProjects.Include(b => b.Subcategory)
                                                .ThenInclude(s => s.Category)
                                                .Include(b => b.ProjectSkills)
                                                .ThenInclude(ps => ps.Skill)
                                                .Include(b => b.Proposals)
                                                .Include(b => b.Client).ThenInclude(c => c.Reviewed)
                                                .Include(b=>b.Client).ThenInclude(c=>c.City).ThenInclude(c=>c.Country)                                             
                                                .Where(b => !b.IsDeleted && b.FreelancerId == null).AsQueryable();

            if (filters.minPrice > 0)
            {
                query = query.Where(q => q.minimumPrice == filters.minPrice);
            }
            if (filters.maxPrice > 0)
            {
                query = query.Where(b => b.maximumprice == filters.maxPrice);
            }
            if (filters.Currency is { Count: > 0 })
            {
                query = query.Where(b => filters.Currency.Contains((int)b.currency));
            }
            if (filters.Category is { Count: > 0 })
            {
                query = query.Where(b => filters.Category.Contains(b.Subcategory.Category.Id));
            }
            if (filters.SubCategory is { Count: > 0 })
            {
                query = query.Where(b => filters.SubCategory.Contains(b.SubcategoryId));
            }
            if (filters.Skills is { Count: > 0 })
            {

                query = query.Where(b => b.ProjectSkills.Any(ps => filters.Skills.Contains(ps.SkillId)));
            }
            if (filters.ExperienceLevel is { Count: > 0 })
            {
                query = query.Where(b => filters.ExperienceLevel.Contains((int)b.experienceLevel));
            }


           

            //if (filters.ProposalsRange != null && filters.ProposalsRange.Any())
            //{
            //    query = query.Where(b => filters.ProposalsRange.Any(range =>
            //        (!range.MinNumOfProposals.HasValue || b.Proposals.Count >= range.MinNumOfProposals) &&
            //        (!range.MaxNumOfProposals.HasValue || b.Proposals.Count <= range.MaxNumOfProposals)
            //    ));
            //}



            if (filters.ClientCountry is { Count: > 0 })
            {
                query = query.Where(b => filters.ClientCountry.Contains(b.Client.City.CountryId));
            }
            if (filters.MinExpectedDuration > 0)
            {
                query = query.Where(b => b.ExpectedDuration >= filters.MinExpectedDuration);
            }
            if (filters.MaxExpectedDuration > 0)
            {
                query = query.Where(b => b.ExpectedDuration <= filters.MaxExpectedDuration);
            }
            var result = await query.ToListAsync();

            if (filters.ProposalRange != null && filters.ProposalRange.Any())
            {
                result = result.Where(b => filters.ProposalRange.Any(range =>
                    (!range.Min.HasValue || b.Proposals.Count >= range.Min) &&
                    (!range.Max.HasValue || b.Proposals.Count <= range.Max)
                )).ToList();
            }

            List<BiddingProjectGetAllDTO> filterdto = new();


			foreach (var project in result) {
                var instance = _mapper.Map<BiddingProjectGetAllDTO>(project);
				if (project.Proposals.Count == 0)
				{
					instance.currentBid = project.maximumprice;

				}
				else
				{
					instance.currentBid = project.Proposals.Min(p => p.Price);
				}
				filterdto.Add(instance);
                

			}
			//var filterdto = _mapper.Map<List<BiddingProjectGetAllDTO>>(result);




            return filterdto.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

        }

           


            //if (filters.ProposalsRange != null && filters.ProposalsRange.Any())
            //{
            //    result = result.Where(b => filters.ProposalsRange.Any(range =>
            //        (!range.MinNumOfProposals.HasValue || b.Proposals.Count >= range.Min) &&
            //        (!range.Max.HasValue || b.Proposals.Count <= range.Max)
            //    ));
            //}



      

        //if(filters.ProposalRange is { Count: > 0 })
        //{
        //    query = query.Where(b => filters.ProposalRange.Any(range =>
        //         (!range.Min.HasValue || b.Proposals.Count >= range.Min) &&
        //        (!range.Max.HasValue || b.Proposals.Count <= range.Max)
        //    ));
        //}

        //if (filters.MinNumOfProposals > 0)
        //{
        //    query = query.Where(b => b.Proposals.Count() >= filters.MinNumOfProposals);
        //}
        //if (filters.MaxNumOfProposals > 0)
        //{
        //    query = query.Where(b => b.Proposals.Count() <= filters.MaxNumOfProposals);
        //}


        //foreach (var (dto, entity) in filterdto.Zip(result, (dto, entity) => (dto, entity)))
        //{
        //    var reviews = entity.Client?.Reviewed?.ToList();
        //    dto.ClientRating = (reviews != null && reviews.Any()) ? reviews.Average(r => r.Rating) : 0;
        //}


        //foreach (var dto in filterdto)
        //{
        //    var original = query.FirstOrDefault(p => p.Id == dto.Id);

        //    var clientReviews = await _context.Reviews.Where(r => r.RevieweeId == original.ClientId).ToListAsync();

        //    dto.ClientRating = clientReviews.Any()
        //        ? (clientReviews.Average(r => r.Rating))
        //        : 0;

        //    dto.ClientTotalNumberOfReviews = clientReviews.Count;

        //}

        //----------------------------------------------------------------------------------------------------

        public async Task<BiddingProjectGetByIdDTO> GetBiddingProjectByIdAsync(int id, string userId)
        {
            var project = await _context.biddingProjects
                                        .Include(b => b.Proposals)
                                        .Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
                                        .Include(b => b.Client).ThenInclude(c => c.Reviewed)
                                        .Include(b=>b.Freelancer).ThenInclude(f=>f.subscriptionPlan)

                                        .Include(b=>b.Client.City).ThenInclude(c=>c.Country)
                                        .FirstOrDefaultAsync(b => b.Id == id);
            if (project == null)
            {
                return null;
            }

            




            var projectDto = _mapper.Map<BiddingProjectGetByIdDTO>(project);

            projectDto.ClientOtherProjectsIdsNotAssigned = _context.project.Where(p=>!p.IsDeleted && p.ClientId == project.ClientId && p.FreelancerId==null && p.Id!=id).Select(p=>p.Id).ToList();
            projectDto.ClientProjectsTotalCount = _context.project.Where(p => !p.IsDeleted && p.ClientId == project.ClientId).Count();

            var freelancer = await _context.freelancers
            .Include(f => f.subscriptionPlan)
            .FirstOrDefaultAsync(f => f.Id == userId);
            if (freelancer is not null)
            {
                projectDto.FreelancersubscriptionPlan = freelancer?.subscriptionPlan?.name ?? string.Empty;
                projectDto.FreelancerTotalNumber = freelancer?.subscriptionPlan?.TotalNumber ?? 0;
                projectDto.FreelancerRemainingNumberOfBids = freelancer?.RemainingNumberOfBids ?? 0;
            }
            if(project.Proposals.Count==0)
            {
                projectDto.currentBid = project.maximumprice;

			}
            else
            {
                projectDto.currentBid = project.Proposals.Min(p => p.Price);
            }


			return projectDto;

        }


        //----------------------------------------------------------------------------------------------------


        //var projectskills= await _context.ProjectSkills.Include(ps=>ps.Skill.Name).Where(p=>p.Skill.Name.)

        // List<string> ps = new List<string>();
        //foreach(var skill in project.projectSkills)
        //{
        //    ps.Add(skill);
        //}


        //var subcategory = await _context.Subcategories.FirstOrDefaultAsync(s => s.Name.ToLower() == project.SubcategoryName.ToLower());

        //if (subcategory == null)
        //    throw new Exception($"Subcategory not found");


        //var skillEntities = await _context.Skills.Where(s => project.ProjectSkills.Select(ps => ps.ToLower()).Contains(s.Name.ToLower())).ToListAsync();

        //if (skillEntities == null)
        //    throw new Exception($"projectskills not found");


        //createdProject.Subcategory = subcategory;
        //createdProject.CreatedAt = DateTime.Now;
        //createdProject.ProjectSkills = skillEntities.Select(skill => new ProjectSkill
        //{
        //    SkillId = skill.Id,
        //    Skill = skill
        //}).ToList();

        public async Task<BiddingProject> CreateBiddingProjectAsync(BiddingProjectCreateUpdateDTO project, string ClinetId)
        {



            var createdProject = _mapper.Map<BiddingProject>(project);
            createdProject.ClientId = ClinetId;
            //createdProject.ClientId = "63d89bb1-7a13-4e02-bf19-14701398e3a1";
         

            await _context.biddingProjects.AddAsync(createdProject);
			(await _context.clients.FirstOrDefaultAsync(c => c.Id == (createdProject.ClientId))).RemainingNumberOfProjects--;
			await _context.SaveChangesAsync();

            _context.Entry(createdProject).Reference(p => p.Subcategory).Load();
            await _context.Entry(createdProject).Collection(p => p.ProjectSkills).LoadAsync();
            foreach (var projectSkill in createdProject.ProjectSkills)
            {
                _context.Entry(projectSkill).Reference(ps => ps.Skill).Load();
            }
            return createdProject;
        }


        //----------------------------------------------------------------------------------------------------



        public async Task<BiddingProject> UpdateBiddingProjectAsync(BiddingProjectCreateUpdateDTO project, int id)
        {
            var p = _context.biddingProjects.Include(b=>b.ProjectSkills).ThenInclude(ps=>ps.Skill).Include(p=>p.Subcategory).SingleOrDefault(p => p.Id == id);
            if (p is not null)
            {
                foreach(var skill in p.ProjectSkills)
                {
                    _context.ProjectSkills.Remove(skill);
                }
                _context.SaveChanges();
                _mapper.Map(project, p);
                //_context.biddingProjects.Update(p);
                await _context.SaveChangesAsync();

            }
            _context.Entry(p).Reference(p => p.Subcategory).Load();
            await _context.Entry(p).Collection(ps => ps.ProjectSkills).LoadAsync();
            foreach (var projectSkill in p.ProjectSkills)
            {
                _context.Entry(projectSkill).Reference(ps => ps.Skill).Load();
            }
            return p;
        }



        //----------------------------------------------------------------------------------------------------


        public async Task<bool> DeleteBiddingProjectAsync(int id)
        {
            var project = await _context.biddingProjects.FindAsync(id);
            if (project == null) return false;

            //_context.biddingProjects.Remove(project);
            project.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        //----------------------------------------------------------------------------------------------------

        public async Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsyncByfreelancerId(string id, userRole role)
        {
            List<BiddingProject> projects;

            switch (role)
            {
                case userRole.Client:
                    {
                        projects = await _context.biddingProjects
                .Include(b => b.Proposals)
                .Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
                .Include(b => b.Client).ThenInclude(c => c.Reviewed)
                .Include(b => b.Freelancer).ThenInclude(f => f.subscriptionPlan)
                .Include(b => b.Client.City).ThenInclude(c => c.Country)
                 .Where(u => u.ClientId == id && !u.IsDeleted).ToListAsync();
                    }
                    break;
                case userRole.Freelancer:
                    {
                        projects = await _context.biddingProjects
                .Include(b => b.Proposals)
                .Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
                .Include(b => b.Client).ThenInclude(c => c.Reviewed)
                .Include(b => b.Freelancer).ThenInclude(f => f.subscriptionPlan)
                .Include(b => b.Client.City).ThenInclude(c => c.Country)
                 .Where(u => u.FreelancerId == id && !u.IsDeleted).ToListAsync();
                    }
                    break;
                default:
                    projects = new();
                    break;
            }

            var projectss = _mapper.Map<List<BiddingProjectGetAllDTO>>(projects);
            return projectss;
        }

        //----------------------------------------------------------------------------------------------------

        public async Task<List<BiddingProjectGetAllDTO>> GetAllBiddingProjectsAsyncByclientId(string id,BiddingProjectFilterDTO filters)
        {
            var projects = await _context.biddingProjects
                .Include(b => b.Proposals)
                .Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
                .Include(b => b.Client).ThenInclude(c => c.Reviewed)
                .Include(b => b.Freelancer).ThenInclude(f => f.subscriptionPlan)
                .Include(b => b.Client.City).ThenInclude(c => c.Country)
                .Where(u => u.ClientId == id && !u.IsDeleted).ToListAsync();
            var projectss = _mapper.Map<List<BiddingProjectGetAllDTO>>(projects);
            return projectss;
        }

        //----------------------------------------------------------------------------------------------------

        public async Task<List<BiddingProjectGetAllDTO>> GetmyBiddingProjectsAsync(string userId,userRole role,int pageNumber, int PageSize)
        {
            List<BiddingProject> projects;

			switch (role)
            {
                case userRole.Client:
                    {
						projects = await _context.biddingProjects
				.Include(b => b.Proposals)
				.Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
				.Include(b => b.Client).ThenInclude(c => c.Reviewed)
				.Include(b => b.Freelancer).ThenInclude(f => f.subscriptionPlan)
				.Include(b => b.Client.City).ThenInclude(c => c.Country)
				 .Where(u => u.ClientId == userId&& !u.IsDeleted).Skip((pageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
					}
                    break;
				case userRole.Freelancer:
					{
						projects = await _context.biddingProjects
				.Include(b => b.Proposals)
				.Include(b => b.ProjectSkills).ThenInclude(ps => ps.Skill)
				.Include(b => b.Client).ThenInclude(c => c.Reviewed)
				.Include(b => b.Freelancer).ThenInclude(f => f.subscriptionPlan)
				.Include(b => b.Client.City).ThenInclude(c => c.Country)
				 .Where(u => u.FreelancerId == userId && !u.IsDeleted).Skip((pageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
					}
					break;
                default:
                    projects = new();
					break;
			}
			
			var projectss = _mapper.Map<List<BiddingProjectGetAllDTO>>(projects);
            return projectss;
		}

		//----------------------------------------------------------------------------------------------------

		//public async Task<List<BiddingProjectGetAllDTO>> Filter(BiddingProjectFilterDTO filters, int pageNumber, int PageSize)
		//{


        //    var query = _context.biddingProjects.Include(b => b.Subcategory)
        //                                        .ThenInclude(s => s.Category)
        //                                        .Include(b => b.ProjectSkills)
        //                                        .ThenInclude(ps => ps.Skill)
        //                                        .Include(b => b.Proposals)
        //                                        .Include(b => b.Client).ThenInclude(c => c.Reviewed)
        //                                        .Where(b => !b.IsDeleted).AsQueryable();

		//    if (filters.minPrice > 0)
		//    {
		//        query = query.Where(q => q.minimumPrice == filters.minPrice);
		//    }

		//    if (filters.maxPrice > 0)
		//    {
		//        query = query.Where(b => b.maximumprice == filters.maxPrice);
		//    }

        //    if (filters.Currency is { Count: > 0 })
        //    {
        //        query = query.Where(b => filters.Currency.Contains((int)b.currency));
        //    }

        //    if (filters.Category is { Count: > 0 })
        //    {
        //        query = query.Where(b => filters.Category.Contains(b.Subcategory.Category.Id));
        //    }

        //    if (filters.SubCategory is { Count: > 0 })
        //    {
        //        query = query.Where(b => filters.SubCategory.Contains(b.SubcategoryId));
        //    }

        //    if (filters.Skills is { Count: > 0 })
        //    {
        //        query = query.Where(b => b.ProjectSkills.Any(ps => filters.Skills.Contains(ps.SkillId)));
        //    }

        //    if (filters.ExperienceLevel is { Count: > 0 })
        //    {
        //        query = query.Where(b => filters.ExperienceLevel.Contains((int)b.experienceLevel));
        //    }

		//    if (filters.MinNumOfProposals > 0)
		//    {
		//        query = query.Where(b => b.Proposals.Count() >= filters.MinNumOfProposals);
		//    }

        //    if (filters.MaxNumOfProposals > 0)
        //    {
        //        query = query.Where(b => b.Proposals.Count() <= filters.MaxNumOfProposals);

		//    }

        //    if (filters.ClientCountry is { Count: > 0 })
        //    {
        //        query = query.Where(b => filters.ClientCountry.Contains(b.Client.City.CountryId));
        //    }

		//    if (filters.MinExpectedDuration > 0)
		//    {
		//        query = query.Where(b => b.ExpectedDuration >= filters.MinExpectedDuration);
		//    }

		//    if (filters.MaxExpectedDuration > 0)
		//    {
		//        query = query.Where(b => b.ExpectedDuration <= filters.MaxExpectedDuration);
		//    }

		//    var result = await query.ToListAsync();

		//    var filterdto = _mapper.Map<List<BiddingProjectGetAllDTO>>(result);




        //    return filterdto.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToList();

		//}
		//foreach (var dto in filterdto)
		//{
		//    var original = query.FirstOrDefault(p => p.Id == dto.Id);

		//    var clientReviews = await _context.Reviews.Where(r => r.RevieweeId == original.ClientId).ToListAsync();

		//    dto.ClientRating = clientReviews.Any()
		//        ? (clientReviews.Average(r => r.Rating))
		//        : 0;

		//    dto.ClientTotalNumberOfReviews = clientReviews.Count;

		//}
	}
}
