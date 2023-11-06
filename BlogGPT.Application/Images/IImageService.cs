namespace BlogGPT.Application.Images
{
    public interface IImageService
    {
        Task UploadImageAsync(string name, string url, CancellationToken cancellationToken);
    }
}