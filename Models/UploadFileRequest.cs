using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace S3AdvancedV2.Models
{
    public class UploadFileRequest
    {
        [Required]
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }
}
