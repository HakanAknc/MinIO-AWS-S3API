using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using S3AdvancedV2.Models;
using S3AdvancedV2.Services;
using S3AdvancedV2.Services.Interfaces;

namespace S3AdvancedV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly IS3Service _s3Service;

        public S3Controller(IS3Service s3Service)
        {
            _s3Service = s3Service;
        }

        // This method uploads a file to S3 and returns the key and pre-signed URL for accessing the file.
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("Dosya gönderilmedi.");

            var key = await _s3Service.UploadAsync(request.File);
            var url = _s3Service.GetPreSignedUrl(key);
            return Ok(new { key, url });
        }

        // This method lists all files in the S3 bucket with an optional prefix.
        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] string? prefix)
        {
            var files = await _s3Service.ListAsync(prefix ?? "uploads/");
            return Ok(files);
        }
        

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string key)
        {
            await _s3Service.DeleteAsync(key);
            return Ok("Silindi");
        }

        // This method downloads a file from S3 using its key and returns it as a file result.
        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string key)
        {
            var (stream, contentType) = await _s3Service.DownloadAsync(key);
            return File(stream, contentType, Path.GetFileName(key));
        }

        // This method generates a pre-signed URL for accessing a file in S3.
        [HttpGet("list-details")]
        public async Task<IActionResult> ListDetails([FromQuery] string? prefix)
        {
            var files = await _s3Service.ListDetailedAsync(prefix ?? "uploads/");
            return Ok(files);
        }

        // This method lists files in S3 by their extension.
        [HttpGet("list-by-extension")]
        public async Task<IActionResult> ListByExtension([FromQuery] string ext)
        {
            if (string.IsNullOrWhiteSpace(ext))
                return BadRequest("Dosya uzantısı boş olamaz. Örn: .txt");

            var files = await _s3Service.ListByExtensionAsync(ext);
            return Ok(files);
        }

        // This method lists files in S3 by their upload date.  
        [HttpGet("list-by-date")]
        public async Task<IActionResult> ListByDate([FromQuery] DateTime date)
        {
            var files = await _s3Service.ListByDateAsync(date);
            return Ok(files);
        }

        // This method renames a file in S3 by changing its key.
        [HttpPost("rename")]
        public async Task<IActionResult> Rename([FromBody] RenameFileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.OldKey) || string.IsNullOrWhiteSpace(request.NewFileName))
                return BadRequest("Geçerli eski dosya ve yeni dosya adı girilmelidir.");

            var newKey = await _s3Service.RenameAsync(request.OldKey, request.NewFileName);
            return Ok(new { oldKey = request.OldKey, newKey });
        }

        // This method lists all S3 buckets.
        [HttpGet("buckets")]
        public async Task<IActionResult> ListBuckets()
        {
            var buckets = await _s3Service.ListBucketsAsync();
            return Ok(buckets);
        }

        // This method creates a new S3 bucket with the specified name.
        [HttpPost("buckets/create")]
        public async Task<IActionResult> CreateBucket([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Bucket adı girilmelidir.");

            await _s3Service.CreateBucketAsync(name);
            return Ok($"Bucket '{name}' oluşturuldu.");
        }

        // This method deletes an S3 bucket with the specified name.
        [HttpDelete("buckets/delete")]
        public async Task<IActionResult> DeleteBucket([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Bucket adı girilmelidir.");

            await _s3Service.DeleteBucketAsync(name);
            return Ok($"Bucket '{name}' silindi.");
        }
    }
}
