using DocumentFormat.OpenXml.Wordprocessing;

namespace mdsj.lib
{
    internal class ObjectInfo
    {
        private string bucketName;
        private string objectName;
        public string Name;

        public ObjectInfo(string bucketName, string objectName)
        {
            this.bucketName = bucketName;
            this.objectName = objectName;
            this.Name = objectName;
        }
    }
}