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
            List<FileResource> files = ResourceManager.Instance.GetResourcesFromCache(fileName);

            if (files == null || files.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            StringBuilder sb = new StringBuilder();
            ICompress compressor = SimpleCompressionConfiguration.CompressorToUse;
            Func<string, string> compressFunction = resourceType == ResourceType.Css ? (Func<string, string>)compressor.CompressCssString :
                (Func<string, string>)compressor.CompressJavascriptString;

            List<string> addedFiles = AddResponseToBuffer(context, sb, compressor, compressFunction, files);
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
                case ".js": return ResourceType.javascript;
                default: throw new ArgumentException("Uknown file type: " + fileName);
            }
        }

        private List<string> AddResponseToBuffer(HttpContext context, StringBuilder sb, ICompress compressor, Func<string, string> compressFunction, List<FileResource> files)
        {
            var addedFiles = new List<string>();
            foreach (FileResource fileResource in files.OrderByDescending(y => y.Priority))
            {
                string file = fileResource.FilePath;

                file = context.Server.MapPath(file);
                addedFiles.Add(file);
                using (StreamReader input = new StreamReader(file))
                {
                    if (fileResource.Compress)
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
