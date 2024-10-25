using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WalksAPI.Data;
using WalksAPI.Interfaces.Repositories;
using WalksAPI.Models.Domain;

namespace WalksAPI.Repositories
{
    public class LocalImageRepository:IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly WalkDbContext walkDbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor, WalkDbContext walkDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.walkDbContext = walkDbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            // Correct the file path combination
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Image", image.FileName + image.FileExtension);

            // Upload image to local path
            using (var stream = new FileStream(localFilePath, FileMode.Create))
            {
                await image.File.CopyToAsync(stream);
            }

            // Construct the URL for the uploaded image
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/Image/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add image to the image table
            await walkDbContext.Images.AddAsync(image);
            await walkDbContext.SaveChangesAsync();

            return image;
        }
    }
}
