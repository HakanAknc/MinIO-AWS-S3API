using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using S3AdvancedV2.Models;

namespace S3AdvancedV2.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3Settings _settings;

        public S3Service(IOptions<S3Settings> options)
        {
            _settings = options.Value;

            var config = new AmazonS3Config
            {
                ServiceURL = _settings.ServiceURL,
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
        }

        // This method uploads a file to S3 and returns the key of the uploaded file.
        public async Task<string> UploadAsync(IFormFile file)
        {
            var key = $"uploads/{DateTime.UtcNow:yyyy/MM/dd}/{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();

            var request = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                BucketName = _settings.BucketName,
                Key = key,
                ContentType = file.ContentType
            };

            var transfer = new TransferUtility(_s3Client);
            await transfer.UploadAsync(request);

            return key;
        }

        // This method lists all files in the specified S3 bucket with an optional prefix.
        public async Task<List<string>> ListAsync(string prefix = "uploads/")
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects.Select(obj => obj.Key).ToList();
        }

        // This method deletes the specified file from S3 using its key.
        public async Task DeleteAsync(string key)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        // This method generates a pre-signed URL for the specified key, allowing temporary access to the file.
        public string GetPreSignedUrl(string key, int expireInSeconds = 3600)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddSeconds(expireInSeconds)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        // This method downloads the file from S3 and returns a stream along with its content type.
        public async Task<(Stream stream, string contentType)> DownloadAsync(string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request);
            return (response.ResponseStream, response.Headers.ContentType);
        }

        // This method lists all files in the S3 bucket with detailed information such as size, last modified date, and content type!
        public async Task<List<S3ObjectInfo>> ListDetailedAsync(string prefix = "uploads/")
        {
            var result = new List<S3ObjectInfo>();

            var request = new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects)
            {
                // Dosyanın meta verilerine erişmek için başlıkları getir
                var metadata = await _s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
                {
                    BucketName = _settings.BucketName,
                    Key = obj.Key
                });

                result.Add(new S3ObjectInfo
                {
                    Key = obj.Key,
                    Size = (long)obj.Size,
                    LastModified = (DateTime)obj.LastModified,
                    ContentType = metadata.Headers.ContentType
                });
            }

            return result;
        }

        // This method lists all files in the S3 bucket with a specific file extension, allowing for filtering by file type.
        public async Task<List<string>> ListByExtensionAsync(string extension, string prefix = "uploads/")
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            return response.S3Objects
                .Where(o => o.Key.EndsWith(extension, StringComparison.OrdinalIgnoreCase))
                .Select(o => o.Key)
                .ToList();
        }

        // This method lists all files in the S3 bucket that were uploaded on a specific date, formatted as yyyy/MM/dd.
        public async Task<List<string>> ListByDateAsync(DateTime date)
        {
            var prefix = $"uploads/{date:yyyy/MM/dd}/";

            var request = new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(request);

            return response.S3Objects.Select(o => o.Key).ToList();
        }

        // This method renames a file in S3 by copying it to a new key with a new file name and then deleting the old file.
        public async Task<string> RenameAsync(string oldKey, string newFileName)
        {
            var extension = Path.GetExtension(oldKey);
            var folder = Path.GetDirectoryName(oldKey)?.Replace("\\", "/");

            var newKey = $"{folder}/{Guid.NewGuid()}_{newFileName}{extension}";

            // 1. Kopyala
            var copyRequest = new CopyObjectRequest
            {
                SourceBucket = _settings.BucketName,
                SourceKey = oldKey,
                DestinationBucket = _settings.BucketName,
                DestinationKey = newKey
            };

            await _s3Client.CopyObjectAsync(copyRequest);

            // 2. Sil
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _settings.BucketName,
                Key = oldKey
            });

            return newKey;
        }

        // This method provides basic bucket operations such as listing, creating, and deleting buckets.
        // 1. Listele
        public async Task<List<string>> ListBucketsAsync()
        {
            var response = await _s3Client.ListBucketsAsync();
            return response.Buckets.Select(b => b.BucketName).ToList();
        }

        // 2. Oluştur
        public async Task CreateBucketAsync(string bucketName)
        {
            await _s3Client.PutBucketAsync(bucketName);
        }

        // 3. Sil
        public async Task DeleteBucketAsync(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
        }
    }
}
