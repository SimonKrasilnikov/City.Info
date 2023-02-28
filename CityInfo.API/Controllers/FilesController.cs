using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtentensionContentProvider;
        public FilesController(
            FileExtensionContentTypeProvider fileExtentensionContentProvider) 
        {
            _fileExtentensionContentProvider = fileExtentensionContentProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtentensionContentProvider));

        }
        
        [HttpGet("{fileId}")]
        public IActionResult GetFile(string fileId)
        {
            var pathToFile = "dev.pdf";
            
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtentensionContentProvider.TryGetContentType(
                pathToFile, out var contentType)) 
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
