namespace S3AdvancedV2.Models
{
    public class S3ObjectInfo
    {
        public string Key { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string ContentType { get; set; }
    }
}
