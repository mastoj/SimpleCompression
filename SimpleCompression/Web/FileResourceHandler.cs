using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Caching;
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
            string fileName = Path.GetFileName(context.Request.RawUrl);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            ResourceType resourceType = GetResourceTypeFromFileName(fileName);
            context.Response.ContentType = resourceType == ResourceType.Css ? "text/css" : "text/javascript";

            if (!WriteFromCache(context, fileName))
            {
                var cachedItem = ResourceCacheManager.Instance.GetResourcesFromCache(fileName);

                if (cachedItem == null)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                var configuration = cachedItem.Item2;
                List<FileResource> files = cachedItem.Item1;
                if (files == null || files.Count == 0)
                {
                    context.Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return;
                }

                StringBuilder sb = new StringBuilder();

                List<string> addedFiles = AddResponseToBuffer(context, sb, configuration, resourceType, files);
                string output = CompressResponse(sb, configuration, resourceType);
                AddToCache(context, fileName, output);
                context.Response.Write(output);
            }
        }

        private string CompressResponse(StringBuilder sb, SimpleCompressionConfiguration configuration, ResourceType resourceType)
        {
            if (configuration.Compress)
            {
                ICompress compressor = configuration.Compressor;
                Func<string, string> compressFunction = resourceType == ResourceType.Css
                                                            ? (Func<string, string>) compressor.CompressCssString
                                                            : compressor.CompressJavascriptString;
                return compressFunction(sb.ToString());
            }
            else
            {
                return sb.ToString();
            }
        }

        private bool WriteFromCache(HttpContext context, string fileName)
        {
            var output = ResourceCacheManager.Instance.GetFileFromCache(fileName);
            if (!string.IsNullOrEmpty(output))
            {
                context.Response.Write(output);
                return true;
            }
            return false;
        }

        private void AddToCache(HttpContext context, string fileName, string content)
        {
            ResourceCacheManager.Instance.AddFileToCache(fileName, content);
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
            var addedFiles = new List<string>();
            foreach (FileResource fileResource in files)
            {
                string file = fileResource.FilePath;

                file = context.Server.MapPath(file);
                addedFiles.Add(file);
                using (StreamReader input = new StreamReader(file))
                {
                    sb.Append(input.ReadToEnd());
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
//            context.Response.Cache.SetExpires(DateTime.Now.AddHours(12));
            context.Response.Cache.SetSlidingExpiration(true);
            context.Response.Cache.SetMaxAge(new TimeSpan(1));
        }
    }
}
