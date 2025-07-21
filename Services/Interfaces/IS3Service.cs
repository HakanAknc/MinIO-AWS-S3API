using S3AdvancedV2.Models;

namespace S3AdvancedV2.Services.Interfaces
{
    public interface IS3Service
    {
        public interface IS3Service
        {
            Task<string> UploadAsync(IFormFile file);
            Task<List<string>> ListAsync(string prefix = "uploads/");
            Task DeleteAsync(string key);
            string GetPreSignedUrl(string key, int expireInSeconds = 3600);
            Task<(Stream stream, string contentType)> DownloadAsync(string key);
            Task<List<S3ObjectInfo>> ListDetailedAsync(string prefix = "uploads/");
            Task<List<string>> ListByExtensionAsync(string extension, string prefix = "uploads/");
            Task<List<string>> ListByDateAsync(DateTime date);
            Task<string> RenameAsync(string oldKey, string newFileName);

            Task<List<string>> ListBucketsAsync();
            Task CreateBucketAsync(string bucketName);
            Task DeleteBucketAsync(string bucketName);
        }
    }
}
