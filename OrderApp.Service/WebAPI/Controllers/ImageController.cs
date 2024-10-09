using Core.Utilities.Security.Hashing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace ReactlyMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Geçersiz dosya veya dosya adı");
                }

                var fileExtension = Path.GetExtension(file.FileName);
                string imageName = HashingHelper.CreateImageHash(
                    file.FileName + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")) + fileExtension;

                string webRootPath = _webHostEnvironment.WebRootPath;
                var filePath = Path.Combine(webRootPath, "Images", imageName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return Ok(new { success = true, message = "Başarıyla yüklendi.", path = imageName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("getimage")]
        public IActionResult GetImage([FromQuery] string imageName)
        {
            try
            {
                if (string.IsNullOrEmpty(imageName))
                {
                    return BadRequest("Geçersiz dosya adı");
                }

                string webRootPath = _webHostEnvironment.WebRootPath;
                var filePath = Path.Combine(webRootPath, "Images", imageName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("Dosya bulunamadı");
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                string contentType = "";

                // Determine content type based on file extension
                switch (Path.GetExtension(imageName).ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    case ".webp":
                        contentType = "image/webp";
                        break;
                    case ".svg":
                        contentType = "image/svg+xml";
                        break;
                    case ".ico":
                        contentType = "image/x-icon";
                        break;
                    default:
                        return BadRequest("Desteklenmeyen dosya türü");
                }


                // Return image file
                return File(fileBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
