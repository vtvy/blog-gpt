using BlogGPT.Application.Common.Interfaces.Data;

namespace BlogGPT.Application.Images
{
    public class ImageService : IImageService
    {
        private readonly IApplicationDbContext _context;
        public ImageService(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task UploadImageAsync(string name, string url, CancellationToken cancellationToken)
        {
            _context.Images.Add(new Image
            {
                Name = name,
                Url = url
            });

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}