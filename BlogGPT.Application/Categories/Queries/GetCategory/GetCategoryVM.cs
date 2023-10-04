namespace BlogGPT.Application.Categories.Queries.GetCategory;
public class GetCategoryVM
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public GetCategoryVM? Parent { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, GetCategoryVM>();
        }
    }
}
