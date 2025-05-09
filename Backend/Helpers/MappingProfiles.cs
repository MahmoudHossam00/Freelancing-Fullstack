using AutoMapper;
using Freelancing.DTOs.AuthDTOs;
using Freelancing.DTOs.MilestoneDTOs;
using Freelancing.DTOs.ProposalDTOS;
using Freelancing.DTOs.BiddingProjectDTOs;
using Freelancing.DTOs;


namespace Freelancing.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Milestone, MilestoneGetAllDTO>();
            CreateMap<MilestoneGetAllDTO, Milestone>();
            #region AuthMappings
            CreateMap<RegisterDTO, Client>()
               .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());


            CreateMap<Client, RegisterDTO>();

            CreateMap<Freelancer, RegisterDTO>();
            CreateMap<RegisterDTO, Freelancer>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<CreateAdminDTO, Admin>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<EditProfileDTO, AppUser>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            
            CreateMap<RegisterDTO, Client>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<Client, RegisterDTO>();
            CreateMap<Freelancer, RegisterDTO>();
            CreateMap<RegisterDTO, Freelancer>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<CreateAdminDTO, Admin>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<EditProfileDTO, AppUser>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<AppUser, UsersRequestingVerificationViewDTO>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name));


            CreateMap<Freelancer, ViewFreelancersDTO>()
                .ForMember(dest => dest.UserSkills, opt =>
                opt.MapFrom(src => src.UserSkills.Select(us => us.Skill.Name).Union(src.NonRecommendedUserSkills.Select(us => us.Name)).ToList()))
				.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name)); 

            //CreateMap<Review, FreelancerReviewDTO>()
            //.ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer != null ? src.Reviewer.UserName : string.Empty));

            CreateMap<Freelancer, ViewFreelancerPageDTO>()
               .ForMember(dest => dest.UserSkills, opt =>
               opt.MapFrom(src => src.UserSkills.Select(us => us.Skill.Name).ToList()))
               .ForMember(dest => dest.Languages, opt =>
               opt.MapFrom(src => src.Languages.Select(us => us.Language.ToString()).ToList()))
               .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name));
            CreateMap<Freelancer, ViewFreelancerPageDTO>()
               .ForMember(dest => dest.UserSkills, opt =>
               opt.MapFrom(src => src.UserSkills.Select(us => us.Skill.Name).ToList()))
               .ForMember(dest => dest.Languages, opt =>
               opt.MapFrom(src => src.Languages.Select(us => us.Language.ToString()).ToList()));

            CreateMap<Client, ViewClientDTO>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => (src.City.Country.Name)))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => (src.City.Name)));

			CreateMap<AppUser, UsersViewDTO>()
            .ForMember(dest=>dest.role,opt=>opt.MapFrom(src=>(src.GetType()==typeof(Admin)?Accountrole.Admin :src.GetType()==typeof(Freelancer)?Accountrole.Freelancer:Accountrole.Client)))
            .ForMember(dest=>dest.Country,opt=>opt.MapFrom(src=>(src.City.Country.Name)))
            .ForMember(dest=>dest.City,opt=>opt.MapFrom(src=>(src.City.Name)))

            .AfterMap((src, dest) =>
			{
                if (src is Freelancer freelancer)
                {
                    dest.isAvailable = freelancer.isAvailable;
                    dest.Balance = freelancer.Balance; // Default to 0 if null
                }
                if (src is Client client)
                {
                    dest.PaymentVerified = client.PaymentVerified;
                    dest.Balance = client.Balance; // Default to 0 if null
                }
            });

            CreateMap<ViewClientDTO, Client>();
            CreateMap<AppUser, UsersViewDTO>()
            .ForMember(dest => dest.role, opt => opt.MapFrom(src => (src.GetType() == typeof(Admin) ? Accountrole.Admin : src.GetType() == typeof(Freelancer) ? Accountrole.Freelancer : Accountrole.Client)))
            .AfterMap((src, dest) =>
            {
                if (src is Freelancer freelancer)
                {
                    dest.isAvailable = freelancer.isAvailable;
                    dest.Balance = freelancer.Balance;
                }
                if (src is Client client)
                {
                    dest.PaymentVerified = client.PaymentVerified;
                    dest.Balance = client.Balance;
                }
            })
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name));

			CreateMap<Freelancer, ViewFreelancerPageDTO>()
               .ForMember(dest => dest.UserSkills, opt =>
               opt.MapFrom(src => src.UserSkills.Select(us => us.Skill.Name).ToList()))
               .ForMember(dest => dest.Languages, opt =>
               opt.MapFrom(src => src.Languages.Select(us => us.Language.ToString()).ToList()))
               .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.City.Country.Name))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name));


            #endregion


            #region Proposal mappings

			CreateMap<CreateProposalSuggestedMilestoneDTO, SuggestedMilestone>();
			CreateMap<UpdateProposalSuggestedMilestoneDTO, SuggestedMilestone>();
            CreateMap<CreateProposalDTO, Proposal>()
            .ForMember(dest => dest.suggestedMilestones, opt => opt.MapFrom(src => src.suggestedMilestones))
            .ForMember(dest => dest.SuggestedDuration, opt => opt.MapFrom(src => src.SuggestedDuration));


            CreateMap<EditProposalDTO, Proposal>()
            .ForMember(dest => dest.suggestedMilestones, opt => opt.MapFrom(src => src.suggestedMilestones))
            .ForMember(dest => dest.SuggestedDuration, opt => opt.MapFrom(src => src.SuggestedDuration));

            CreateMap<SuggestedMilestone, MilestoneViewDTO>();

            CreateMap<Proposal, ProposalViewDTO>()
            .ForMember(dest => dest.FreelancerName, opt => opt.MapFrom(src => src.Freelancer.UserName))
            .ForMember(dest => dest.FreelancerProfilePicture, opt => opt.MapFrom(src => src.Freelancer.ProfilePicture))
            .ForMember(dest => dest.Freelancerskills, opt => opt.MapFrom(src =>
                src.Freelancer.UserSkills
                    .Where(us => !us.IsDelete)
                    .Select(us => us.Skill.Name).ToList()))
            .ForMember(dest => dest.FreelancerLanguages, opt => opt.MapFrom(src =>
                src.Freelancer.Languages
                    .Where(fl => !fl.IsDeleted)
                    .Select(fl => fl.Language.ToString()).ToList()))
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.Freelancer.EmailConfirmed))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Freelancer.City.Country.Name))
            .ForMember(dest => dest.SuggestedDuration, opt => opt.MapFrom(src => src.SuggestedDuration))
            .ForMember(dest => dest.suggestedMilestones, opt => opt.MapFrom(src => src.suggestedMilestones))
            .ForMember(dest => dest.rank, opt => opt.MapFrom(src => src.Freelancer.Rank))
            .ForMember(dest => dest.proposalStatus, opt=>opt.MapFrom((src, dest, destMember, context) => src.Project.FreelancerId==null?proposalstatus.Pending: src.Project.Freelancer.UserName==dest.FreelancerName?proposalstatus.Accepted:proposalstatus.Rejected)
            );

            CreateMap<Proposal, ProposalViewDTO>()
            .ForMember(dest => dest.FreelancerName, opt => opt.MapFrom(src => src.Freelancer.UserName))

            .ForMember(dest => dest.FreelancerProfilePicture, opt => opt.MapFrom(src => src.Freelancer.ProfilePicture))

            .ForMember(dest => dest.Freelancerskills, opt => opt.MapFrom(src =>
                src.Freelancer.UserSkills
                    .Where(us => !us.IsDelete)
                    .Select(us => us.Skill.Name).ToList()))

            .ForMember(dest => dest.FreelancerLanguages, opt => opt.MapFrom(src =>
                src.Freelancer.Languages
                    .Where(fl => !fl.IsDeleted)
                    .Select(fl => fl.Language.ToString()).ToList()))

            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => src.Freelancer.EmailConfirmed))

            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Freelancer.City.Country.Name))

            .ForMember(dest => dest.SuggestedDuration, opt => opt.MapFrom(src => src.SuggestedDuration))

            .ForMember(dest => dest.suggestedMilestones, opt => opt.MapFrom(src => src.suggestedMilestones))

            .ForMember(dest => dest.rank, opt => opt.MapFrom(src => src.Freelancer.Rank))
            .ForMember(dest => dest.proposalStatus, opt => opt.Ignore())
            .ForMember(dest => dest.projecttype, opt => opt.Ignore());
            #endregion
			CreateMap<BiddingProjectDTO, BiddingProject>();
            CreateMap<BiddingProject, BiddingProjectDTO>();

            CreateMap<UserSkill, UserSkillDto>();
            CreateMap<UserSkillDto, UserSkill>();

            //CreateMap<PortofolioProject, PortofolioProjectDTO>();
            CreateMap<PortofolioProjectDTO, PortofolioProject>();

            CreateMap<PortofolioProjectImage, PortofolioProjectImageDTO>();
            CreateMap<PortofolioProjectImageDTO, PortofolioProjectImage>();

            CreateMap<Review, ReviewDto>().ReverseMap();


            CreateMap<PortofolioProject, PortofolioProjectDTO>()
    .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));


            CreateMap<BiddingProjectGetAllDTO, BiddingProject>();


            CreateMap<BiddingProject, BiddingProjectGetAllDTO>()
                .ForMember(dest => dest.ProjectType, opt => opt.MapFrom(src => "Bidding"))
                .ForMember(dest => dest.ProjectSkills, opt => opt.MapFrom(src => src.ProjectSkills.Select(ps => ps.Skill.Name).ToList()))
                .ForMember(dest => dest.BidAveragePrice, opt => opt.MapFrom(src =>
                    src.Proposals != null && src.Proposals.Any()
                    ? (int)src.Proposals.Average(p => p.Price)
                    : 0
                ))
                .ForMember(dest => dest.PostedFrom, opt => opt.MapFrom(src =>
                    (DateTime.Now - src.CreatedAt).TotalMinutes
                ))
                .ForMember(dest => dest.ClientTotalNumberOfReviews, opt => opt.MapFrom(src => src.Client.Reviewed.Count()))

                .ForMember(dest => dest.ClientRating, opt => opt.MapFrom((src, dest) =>
                src.Client != null && src.Client.Reviewed != null && src.Client.Reviewed.Any()
                    ? src.Client.Reviewed.Average(r => r.Rating)
                    : 0))

                .ForMember(dest => dest.ClientCountry, opt => opt.MapFrom(src => src.Client.City.Country.Name))
                .ForMember(dest => dest.NumOfBids, opt => opt.MapFrom(src => src.Proposals.Count()))
                .ForMember(dest=>dest.ClientIsVerified, opt=>opt.MapFrom(src=>src.Client.IsVerified))

                .ForMember(dest=>dest.BiddingStartDate, opt=>opt.MapFrom(src=>src.BiddingStartDate.ToString()))
                .ForMember(dest=>dest.BiddingEndDate, opt=>opt.MapFrom(src=>src.BiddingEndDate.ToString()))

                //.ForMember(dest => dest.ClientRating, opt => opt.MapFrom(src => src.Client.Reviewed.Average(r => r.Rating)))
                ;



            CreateMap<BiddingProjectGetByIdDTO, BiddingProject>();

            //CreateMap<BiddingProject, BiddingProjectGetByIdDTO>()
            //    .ForMember(dest => dest.BidAveragePrice, opt => opt.MapFrom(src =>
            //    src.Proposals != null && src.Proposals.Any()
            //    ? (int)src.Proposals.Average(p => p.Price)
            //    : 0
            //    ))

            //    .ForMember(dest => dest.ProjectSkills, opt => opt.MapFrom(src => src.ProjectSkills.Select(ps => ps.Skill.Name)))

            //    .ForMember(dest => dest.PostedFrom, opt => opt.MapFrom(src =>
            //    (DateTime.Now - src.BiddingStartDate).Days))

            //    .ForMember(dest => dest.ClientTotalNumberOfReviews, opt => opt.MapFrom(src => src.Client.Reviewed.Count()))

            //    .ForMember(dest => dest.ClientRating, opt => opt.MapFrom(src => src.Client.Reviewed.Average(r => r.Rating)))

            //    .ForMember(dest => dest.FreelancersubscriptionPlan, opt => opt.MapFrom(src => src.Freelancer.subscriptionPlan.name))

            //    .ForMember(dest => dest.FreelancerTotalNumber, opt => opt.MapFrom(src => src.Freelancer.subscriptionPlan.TotalNumber))

            //    .ForMember(dest => dest.FreelancerRemainingNumberOfBids, opt => opt.MapFrom(src => src.Freelancer.RemainingNumberOfBids))
            //    ;

            CreateMap<BiddingProject, BiddingProjectGetByIdDTO>()

            .ForMember(dest => dest.BidAveragePrice, opt => opt.MapFrom(src =>
                src.Proposals != null && src.Proposals.Any()
                    ? (int)src.Proposals.Average(p => p.Price)
                    : 0
            ))

            .ForMember(dest => dest.ProjectSkills, opt => opt.MapFrom(src =>
                src.ProjectSkills != null
                    ? src.ProjectSkills.Select(ps => ps.Skill.Name)
                    : new List<string>()
            ))

                .ForMember(dest => dest.NumOfBids, opt => opt.MapFrom(src => src.Proposals.Count()))


            .ForMember(dest => dest.PostedFrom, opt => opt.MapFrom(src =>
                (DateTime.Now - src.CreatedAt).TotalMinutes
            ))

            .ForMember(dest => dest.ClientTotalNumberOfReviews, opt => opt.MapFrom(src =>
                src.Client.Reviewed != null
                    ? src.Client.Reviewed.Count()
                    : 0
            ))

            .ForMember(dest => dest.ClientOtherProjectsIdsNotAssigned, opt => opt.Ignore())
            .ForMember(dest => dest.ClientProjectsTotalCount, opt => opt.Ignore())


                .ForMember(dest => dest.ClientRating, opt => opt.MapFrom((src, dest) =>
                src.Client != null && src.Client.Reviewed != null && src.Client.Reviewed.Any()
                    ? src.Client.Reviewed.Average(r => r.Rating)
                    : 0
                ))

                .ForMember(dest => dest.ClinetAccCreationDate, opt => opt.MapFrom(src => src.Client.AccountCreationDate.ToString()))

                //.ForMember(dest => dest.ClientRating, opt => opt.MapFrom((src, dest) =>
                //{
                //    var reviews = src.Client?.Reviewed?.ToList();
                //    return (reviews != null && reviews.Any())
                //        ? reviews.Average(r => r.Rating)
                //        : 0;
                //}))

                //.ForMember(dest => dest.ClientRating, opt => opt.Ignore())

                .ForMember(dest => dest.ClientCity, opt => opt.MapFrom(src => src.Client.City.Name))

                .ForMember(dest => dest.ClientCountry, opt => opt.MapFrom(src => src.Client.City.Country.Name))

                //    .ForMember(dest => dest.FreelancersubscriptionPlan, opt => opt.MapFrom(src =>
                //        src.Freelancer!=null
                //        ?src.Freelancer.subscriptionPlan.name
                //        : "Freelancer not found"
                //    ))


                //    .ForMember(dest => dest.FreelancerTotalNumber, opt => opt.MapFrom((src, dest) =>
                //    src.Freelancer != null && src.Freelancer.subscriptionPlan != null
                //        ? src.Freelancer.subscriptionPlan.TotalNumber
                //        : 0
                //))

                //    .ForMember(dest => dest.FreelancerRemainingNumberOfBids, opt => opt.MapFrom(src =>
                //        src.Freelancer!=null
                //        ?src.Freelancer.RemainingNumberOfBids
                //        :0
                //    ));

                .ForMember(dest => dest.FreelancersubscriptionPlan, opt => opt.Ignore())
                .ForMember(dest => dest.FreelancerTotalNumber, opt => opt.Ignore())
                .ForMember(dest => dest.FreelancerRemainingNumberOfBids, opt => opt.Ignore())
                .ForMember(dest => dest.FreelancerId, opt => opt.MapFrom(src => src.FreelancerId))
                .ForMember(dest => dest.currentBid, opt => opt.Ignore());
                


            //.ForMember(dest=>dest.projectSkills, opt=>opt.Ignore())
            //.ForMember(dest=>dest.Subcategory, opt=>opt.MapFrom(src=>src.Subcategory.Name.ToString()))

            CreateMap<BiddingProjectCreateUpdateDTO, BiddingProject>()
//.ForMember(dest => dest.SubcategoryId, opt => opt.Ignore()) 
//.ForMember(dest => dest.Subcategory, opt => opt.Ignore())
//.ForMember(dest => dest.ProjectSkills, opt => opt.Ignore())
.ForMember(dest => dest.experienceLevel, opt => opt.MapFrom(src => src.ExperienceLevel))
              //.ForMember(dest => dest.SubcategoryId, opt => opt.Ignore()) 
              //.ForMember(dest => dest.Subcategory, opt => opt.Ignore())
              //.ForMember(dest => dest.ProjectSkills, opt => opt.Ignore())
              //.ForMember(dest => dest.experienceLevel, opt => opt.MapFrom(src => (ExperienceLevel)src.ExperienceLevel))
              .ForMember(dest => dest.currency, opt => opt.MapFrom(src => (Currency)src.currency))
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
              .ForMember(dest => dest.ProjectSkills, opt => opt.MapFrom(src => src.ProjectSkillsIds.Select(ps => new ProjectSkill() { SkillId = ps })))
                .ForMember(dest => dest.SubcategoryId, opt => opt.MapFrom(src => src.SubcategoryId));



            //CreateMap<BiddingProject, BiddingProjectCreateDTO>()
            //    .ForMember(dest => dest.ExperienceLevel, opt => opt.MapFrom(src => src.experienceLevel.ToString()))
            //    //.ForMember(dest => dest.SubcategoryName, opt => opt.Ignore())
            //    //.ForMember(dest => dest.ProjectSkills, opt => opt.Ignore())

            //;


            CreateMap<MilestoneGetAllDTO, Milestone>();

            CreateMap<Milestone, MilestoneGetByIdOrProjectIdDTO>(); //source, destination
            CreateMap<MilestoneGetByIdOrProjectIdDTO, Milestone>();
            //.ForMember(dest=>dest.Status, opt=>opt.MapFrom(src=> Enum.Parse<MilestoneStatus>(src.Status)));


            CreateMap<MilestoneCreateDTO, Milestone>();

            CreateMap<Review, GetReviewByRevieweeIdDto>()
                .ForMember(dest => dest.ProjectTitle, opt => opt.MapFrom(src => src.Project != null ? src.Project.Title : string.Empty))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString()))
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer != null ? src.Reviewer.UserName : string.Empty))
                .ForMember(dest => dest.ProjectType,
                opt => opt.MapFrom(src =>
                    src.Project != null
                        ? (src.Project is BiddingProject ? "Bidding" : "Fixed")
                        : string.Empty));




            CreateMap<Project, ProjectDTO>()
            .ForMember(dest => dest.projectType, opt => opt.MapFrom(src =>
            src.GetType() == typeof(FixedPriceProject) ? projectType.fixedprice : projectType.bidding))
            .ForMember(dest=>dest.milestones, opt=>opt.Ignore());

			CreateMap<ProjectDTO, Project>();
        }
    }
}
