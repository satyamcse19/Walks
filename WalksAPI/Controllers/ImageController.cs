using Microsoft.AspNetCore.Mvc;
using WalksAPI.Models.DTO;
using WalksAPI.Models.Domain;
using WalksAPI.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
namespace WalksAPI.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImageController(IImageRepository imageRepository )
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles = "Writer")]
        public async Task< IActionResult> Uplaod([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUplaod(request);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension=Path.GetExtension(request.File.FileName),
                    FileSizeInBytes=request.FileName.Length,
                    FileName=request.FileName,
                    FileDescription=request.FileDescription,
                };
                //user respository upload image 
                await imageRepository.Upload(imageDomainModel);
               return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        private void ValidateFileUplaod(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "unsuppoeted file extension");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size more then 10 mb");
            }

        }
    }
}
