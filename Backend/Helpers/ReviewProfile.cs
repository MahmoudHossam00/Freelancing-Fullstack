using AutoMapper;
using Freelancing.DTOs;

namespace Freelancing.Helpers
{
    public class ReviewProfile : Profile
    {

        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                        .ForMember(dest => dest.RevieweeName, opt => opt.MapFrom(src => src.Reviewee.UserName)) 
                        .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer.UserName)); 

          
            CreateMap<ReviewDto, Review>()
                .ForMember(dest => dest.Reviewee, opt => opt.Ignore()) 
                .ForMember(dest => dest.Reviewer, opt => opt.Ignore());
        }
    }
}
