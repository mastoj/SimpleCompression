using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Routing;
using System.Web.Mvc;

namespace SimpleCompression.Web
{
    public class FileResourceHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            string filePath = context.Request.RawUrl;
            string fileName = Path.GetFileName(context.Request.RawUrl);
            ResourceType resourceType = GetResourceTypeFromFileName(fileName);
            context.Response.ContentType = resourceType == ResourceType.Css ? "text/css" : "text/javascript";
            var cachedItem = ResourceCacheManager.Instance.GetResourcesFromCache(fileName);

            if (cachedItem == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var configuration = cachedItem.Item2;
            List<FileResource> files = cachedItem.Item1;
            if(files == null || files.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            StringBuilder sb = new StringBuilder();

            List<string> addedFiles = AddResponseToBuffer(context, sb, configuration, resourceType, files);
            AddCacheDependecy(context, addedFiles);
            string output = sb.ToString();
            context.Response.Write(output);
        }

        private ResourceType GetResourceTypeFromFileName(string fileName)
        {
            string fileExt = Path.GetExtension(fileName);
            switch(fileExt.ToLower())
            {
                case ".css": return ResourceType.Css;
                case ".js": return ResourceType.Javascript;
                default: throw new ArgumentException("Uknown file type: " + fileName);
            }
        }

        private List<string> AddResponseToBuffer(HttpContext context, StringBuilder sb, SimpleCompressionConfiguration configuration, ResourceType resourceType, List<FileResource> files)
        {
            ICompress compressor = configuration.Compressor;
            Func<string, string> compressFunction = resourceType == ResourceType.Css ? (Func<string, string>)compressor.CompressCssString :
                compressor.CompressJavascriptString;

            var addedFiles = new List<string>();
            foreach (FileResource fileResource in files)
            {
                string file = fileResource.FilePath;

                file = context.Server.MapPath(file);
                addedFiles.Add(file);
                using (StreamReader input = new StreamReader(file))
                {
                    if (configuration.Compress)
                    {
                        sb.Append(compressFunction(input.ReadToEnd()));
                    }
                    else
                    {
                        sb.Append(input.ReadToEnd());
                    }
                }
            }
            return addedFiles;
        }

        private void AddCacheDependecy(HttpContext context, List<string> allFiles)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.AddFileDependencies(allFiles.ToArray());
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
        }
    }
}
