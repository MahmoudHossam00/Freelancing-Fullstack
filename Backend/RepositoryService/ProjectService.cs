using AutoMapper;
using Freelancing.DTOs;
using Freelancing.IRepositoryService;

namespace Freelancing.RepositoryService
{
	public class ProjectService(ApplicationDbContext _context, IMapper mapper) : IProjectService
	{
		public async Task<Project> CreateProjectAsync(Project project)
		{
			//await _context.project.AddAsync(project);
			await _context.SaveChangesAsync();
			return project;
		}

		public async Task<bool> DeleteProjectAsync(int id)
		{
			var project = await GetProjectByIdAsync(id);
			if (project is not null)
			{
				project.IsDeleted = true;
				_context.Update(project);
			}
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<List<Project>> GetAllProjectsAsync()
		{
			var biddingProjects = await _context.Set<BiddingProject>()
									.Include(m => m.Milestones)
									.Where(p => !p.IsDeleted)
									.ToListAsync();

			var fixedProjects = await _context.Set<FixedPriceProject>()
				.Include(m => m.Milestones)
				.Where(p => !p.IsDeleted)
				.ToListAsync();

			var projects = biddingProjects.Cast<Project>()
				.Concat(fixedProjects.Cast<Project>())
				.ToList();
			return projects;
		}

		public async Task<Project> GetProjectByIdAsync(int id)
		{
			return await _context.Set<Project>().FindAsync(id);
		}

		public async Task<Project> UpdateProjectAsync(Project project)
		{
			var proj = await GetProjectByIdAsync(project.Id);
			if (proj is not null)
			{
				_context.Update(project);
			}
			return proj;
		}


		public async Task<List<ProjectDTO>> GetAllProjectsDtoAsync()
		{
			//var x = _context;
			//var y = _context.Set<Project>();
			//var z = _context.project;
			//var h = _context.project.ToList();
			var biddingProjects = await _context.Set<BiddingProject>()
									.Include(m=>m.Milestones)
									.Where(p => !p.IsDeleted)
									.ToListAsync();

			var fixedProjects = await _context.Set<FixedPriceProject>()
				.Include(m => m.Milestones)
				.Where(p => !p.IsDeleted)
				.ToListAsync();

			var projects = biddingProjects.Cast<Project>()
				.Concat(fixedProjects.Cast<Project>())
				.ToList();

			var prjcts = new List<ProjectDTO>();
			foreach(var project in projects)
			{
				var prjct = mapper.Map<ProjectDTO>(project);
				prjct.milestones = new();
				foreach(var milestone in project.Milestones)
				{

					prjct.milestones.Add(new()
					{
						Amount=milestone.Amount,
						Description=milestone.Description,
						startdate=milestone.StartDate,
						enddate=milestone.EndDate,
						Status=milestone.Status,
						Title = milestone.Title
					});
				}
				prjcts.Add(prjct);

			}
			return prjcts;
		}

		public async Task<ProjectDTO> GetProjectDtoByIdAsync(int id)
		{
			var project = await _context.Set<Project>()
								 .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
			return mapper.Map<ProjectDTO>(project);
		}
	}
}