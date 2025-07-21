namespace S3AdvancedV2.Models
{
    public class S3ObjectInfo
    {
        public string Key { get; set; }  // Unique identifier for the object in the S3 bucket
        public long Size { get; set; }  // Size of the object in bytes
        public DateTime LastModified { get; set; }  // Last modified date and time of the object
        public string ContentType { get; set; }  // MIME type of the object
    }
}
