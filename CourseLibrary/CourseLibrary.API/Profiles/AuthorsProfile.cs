using AutoMapper;
using CourseLibrary.API.Helpers;

namespace CourseLibrary.API.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            /* We have to tell AutoMapper how it should map between our entities
             * and DTOs. We are passing Entities.Author as the source and Models.AuthorDto
             * as the destination. AutoMapper is convention-based. It will map property
             * names on the source object to the same property names on the destination
             * object, and by default it will ignore null references exception from
             * source to targe, i.e. if the property doesn't exist, it'll be ignored. But
             * we have some additional work. We need to ensure that first name and last
             * name are concatenated into the name property and we need to calculate the age.
             * This means we need projection. To do that, we call into ForMember.
             * Projection transforms the source to the destination beyond flattening the
             * object model. Without extra configuration, AutoMapper requires flattened destination
             * to match source's type. But, if you want to project sources values into a
             * destination that doesn't exactly match the source structure, we must specify
             * custom member mapping definitions. And that is what we are doing here
             * with ForMember.
             */
            CreateMap<Entities.Author, Models.AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
                )
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge())
                );
        }
    }
}
