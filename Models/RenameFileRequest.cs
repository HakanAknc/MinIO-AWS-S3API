namespace S3AdvancedV2.Models
{
    public class RenameFileRequest
    {
        public string OldKey { get; set; }  // The current key of the file to be renamed
        public string NewFileName { get; set; }  // The new name for the file, which will be used to create a new key
    }
}
