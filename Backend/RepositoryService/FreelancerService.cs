using AutoMapper;
using Freelancing.DTOs.AuthDTOs;
using Freelancing.IRepositoryService;
////using Freelancing.Migrations;
using Freelancing.Models;
using Microsoft.EntityFrameworkCore;

namespace Freelancing.RepositoryService
{
    public class FreelancerService(ApplicationDbContext context,IMapper _mapper) : IFreelancerService
    {

		/*
         public string name { set; get; } = default;
		public DateOnly AccountCreationDate { set; get; } = default;
		public string Country { get; set; } = default;
		public bool IsVerified { get; set; } = default;
		public bool Paymentverified { get; set; } = default;
		public bool isAvailable { get; set; } = default;
		public List<Language> Languages { get; set; } = new List<Language>();
		public List<Rank> ranks { get; set; } = new List<Rank>();
         */
       
		public async Task<List<ViewFreelancersDTO>> GetAllFiltered(FreelancerFilterationDTO dto)
		{
			var freelancers =  context.freelancers.Include(f=>f.NonRecommendedUserSkills).Include(f => f.UserSkills).ThenInclude(S => S.Skill)
				.Include(f => f.Languages).Include(c=>c.City).ThenInclude(c=>c.Country).Include(f=>f.Reviewed).Include(f=>f.subscriptionPlan).Where(f => !f.isDeleted);
			if (dto.CountryIDs is { Count: > 0 })
			{
				freelancers = freelancers
					.Where(f => dto.CountryIDs.Contains(f.City.CountryId));
			}
			if (dto.AccountCreationDate != default)
			{
				freelancers = freelancers.Where(f => f.AccountCreationDate <= dto.AccountCreationDate);
			}
            if (!string.IsNullOrEmpty(dto.name))
            {
                freelancers = freelancers.Where(f => 
                f.firstname.ToLower().Contains(dto.name.ToLower()) ||
                f.lastname.ToLower().Contains(dto.name.ToLower()) ||
                f.UserName.ToLower().Contains(dto.name.ToLower())
                );
			}
			if (dto.IsVerified==true)
			{
                freelancers = freelancers.Where(f => f.IsVerified); // id verification
			}
			//if (dto.Paymentverified)
			//{
			//	freelancers = freelancers.Where(f => f.Paymentverified); // id verification
			//}
			if (dto.isAvailable==true)
			{
                freelancers = freelancers.Where(f => f.isAvailable == true);
			}
			if (dto.Languages != null && dto.Languages.Count > 0)
			{
                freelancers = freelancers.Where(f => f.Languages.Any(fl => dto.Languages.Contains(fl.Language)));

			}
            if(freelancers.Count()==0)
            {
                return new List<ViewFreelancersDTO>();

			}
			var freelancerslist = await freelancers.ToListAsync();

			if (dto.ranks != null && dto.ranks.Count > 0)
			{
				freelancerslist = freelancerslist.AsEnumerable().Where(f => dto.ranks.Contains(f.Rank)).ToList();
			}
			dto.numofpages = dto.pagesize > 0
	                            ? (int)Math.Ceiling((double)freelancerslist.Count() / dto.pagesize)
	                            : 0;
			List<ViewFreelancersDTO> freelancerdtos = _mapper.Map<List<ViewFreelancersDTO>>((freelancerslist.Skip((dto.pageNum-1)*dto.pagesize).Take(dto.pagesize)));
			
			return freelancerdtos;
		}
		public async Task<Freelancer> CreateFreelancerAsync(Freelancer freelancer)
        {
            Freelancer f = new Freelancer()
            {
                Certificates = freelancer.Certificates,
                UserName = freelancer.UserName,
                isAvailable = freelancer.isAvailable,
                Balance = freelancer.Balance,
                Education = freelancer.Education,
                Experiences = freelancer.Experiences,
                Languages = freelancer.Languages,
                Portofolio = freelancer.Portofolio,
                //IsDeleted = false
            };

            context.Add(f);
            context.SaveChanges();
            return f;
        }

        public async Task<bool> DeleteFreelancerAsync(string id)
        {
           var freelancer= await context.freelancers.FirstOrDefaultAsync(f => f.Id == id && !f.isDeleted);
            if (freelancer == null)
                return false;

            freelancer.isDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ViewFreelancersDTO>> GetAllAsync()
        {

            var freelancers= await context.freelancers.Include(f=>f.NonRecommendedUserSkills).Include(c => c.City).ThenInclude(c => c.Country).Include(f=>f.UserSkills).ThenInclude(S=>S.Skill).Where(f=>!f.isDeleted).ToListAsync();
            List<ViewFreelancersDTO> freelancerdtos = new();
            foreach(var freelancer in freelancers)
			{
                freelancerdtos.Add(_mapper.Map<ViewFreelancersDTO>(freelancer));
			}
            return freelancerdtos;
		}

        public async Task<ViewFreelancerPageDTO> GetByIDAsync(string id)
        {
            var freelancer=await context.freelancers.Include(c => c.City).ThenInclude(c => c.Country).Include(f => f.UserSkills).ThenInclude(S => S.Skill)
                .Include(f=>f.Languages).Where(f => !f.isDeleted).FirstOrDefaultAsync(F=>F.Id==id);

			return _mapper.Map<ViewFreelancerPageDTO>(freelancer);

			
        }

        public async Task<Freelancer> UpdateFreelancerAsync(Freelancer freelancer)
        {
           var fl =  await context.freelancers.Include(c => c.City).ThenInclude(c => c.Country).FirstOrDefaultAsync(f=>f.Id==freelancer.Id);

            if (fl == null)
            { 
                return null;

            }
              
            fl.firstname = freelancer.firstname;
            fl.lastname = freelancer.lastname;
            fl.City=freelancer.City;
            fl.CityId=freelancer.CityId;
            fl.ProfilePicture = freelancer.ProfilePicture;

            fl.isAvailable = freelancer.isAvailable;
            fl.Balance = freelancer.Balance;
            fl.Education = freelancer.Education;
            fl.Experiences = freelancer.Experiences;
            fl.Certificates = freelancer.Certificates;
            fl.UserSkills = freelancer.UserSkills;
            fl.Languages = freelancer.Languages;
            fl.Portofolio = freelancer.Portofolio;

            await context.SaveChangesAsync();
            return fl;


        }

        //Task<List<Freelancer>> IFreelancerService.GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<Freelancer> IFreelancerService.GetByIDAsync(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
