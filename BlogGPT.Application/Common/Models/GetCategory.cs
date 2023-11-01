namespace BlogGPT.Application.Common.Models
{
    public class GetCategory
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public string? Description { get; set; }

        public int? ParentId { get; set; }

        public string? Parent { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Category, GetCategory>()
                    .ForMember(destination => destination.Parent, opt => opt
                    .MapFrom(src => src.Parent != null ? src.Parent.Name : null));
            }
        }
    }
}