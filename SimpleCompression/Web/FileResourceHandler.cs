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
//        public FileResourceHandler(RequestContext requestContext) : base(requestContext) {}

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            string filePath = context.Request.RawUrl;
            string fileName = Path.GetFileName(context.Request.RawUrl);
            string fileExt = Path.GetExtension(fileName);
            bool isCssFile = false;
            if (fileExt.Equals(".css"))
            {
                isCssFile = true;
            }
            context.Response.ContentType = isCssFile ? "text/css" : "text/javascript";

            string cacheKey = fileName;
            List<FileResource> files = ResourceManager.Instance.GetResourcesFromCache(cacheKey);

            if (files == null || files.Count == 0)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            List<string> allFiles = new List<string>();
            // Build return string
            StringBuilder sb = new StringBuilder();
            ICompress compressor = new YUICompression();
            foreach (FileResource fileResource in files)
            {
                string file = fileResource.FilePath;

                file = context.Server.MapPath(file);
                allFiles.Add(file);
                using (StreamReader input = new StreamReader(file))
                {
                    if (fileResource.Compress)
                    {
                        Func<string, string> compressFunction = isCssFile ? (Func<string, string>)compressor.CompressCssString : 
                            (Func<string, string>)compressor.CompressJavascriptString;
                        sb.Append(compressFunction(input.ReadToEnd()));
                    }
                    else
                    {
                        sb.Append(input.ReadToEnd());
                    }
                }
            }

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.AddFileDependencies(allFiles.ToArray());
            context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            string output = sb.ToString();
            context.Response.Write(output);
        }
    }
}
