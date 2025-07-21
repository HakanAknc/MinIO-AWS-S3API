using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace S3AdvancedV2.Models
{
    public class UploadFileRequest
    {
        [Required]  // Indicates that this field is required
        [FromForm(Name = "file")]  // The name of the form field that contains the file
        public IFormFile File { get; set; }  // The file to be uploaded, required field
    }
}
