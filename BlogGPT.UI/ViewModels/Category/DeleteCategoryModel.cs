using BlogGPT.Application.Categories.Queries;

public class DeleteCategoryModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }


    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetCategoryVM, DeleteCategoryModel>();
        }
    }
}
