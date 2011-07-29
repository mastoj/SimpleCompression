namespace SimpleCompression
{

    internal class FileResource
    {
        public string FilePath { get; set; }

        public FileResource(string filePath)
        {
            FilePath = filePath;
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as FileResource;
            if (otherObj == null)
            {
                return false;
            }
            return FilePath == otherObj.FilePath;
        }

        public override int GetHashCode()
        {
            return (FilePath != null ? FilePath.GetHashCode() : 0);
        }
    }
}
