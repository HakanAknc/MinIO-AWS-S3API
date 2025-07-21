namespace S3AdvancedV2.Models
{
    public class S3Settings
    {
        public string ServiceURL { get; set; }  // Custom endpoint URL for S3 service (optional)
        public string AccessKey { get; set; }  // AWS Access Key ID 
        public string SecretKey { get; set; }  // AWS Secret Access Key
        public string BucketName { get; set; }  // Name of the S3 bucket
        public string Region { get; set; }  // AWS Region where the bucket is located
    }
}
