namespace BlogGPT.Application.Categories.Queries.GetAllCategory
{
    public class GetAllCategoryVM
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public GetAllCategoryVM? Parent { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Category, GetAllCategoryVM>();
            }
        }
    }
}
