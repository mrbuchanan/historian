using Microsoft.Owin;
using MimeTypes;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard.Dashboard.Content
{
    internal static class ContentExtensions
    {
        private const string ContentPath = "/content/";
        public const string ContentAssembly = "Historian.Dashboard";
        private const string ContentAssemblyPath = "Dashboard.Content";

        public static void HostContent(this IAppBuilder app, DashboardOptions options)
        {
            app.Use((context, next) =>
            {
                // get the request and check path
                var request = context.Request;
                var hasPath = request.Path.HasValue;
                var isCorrectPath = hasPath && request.Path.Value.Contains(ContentPath);
                var isGetRequest = request.Method == "GET";

                // if we have the right path
                if(hasPath && isCorrectPath && isGetRequest)
                {
                    // get full path to content
                    var path = request.Path.Value;
                    path = path.Replace(ContentPath, "");
                    path = path.Replace("/", ".");
                    path = $"{ContentAssemblyPath}.{path}";

                    // get path parts
                    var pathParts = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                    // set default extension
                    var extension = ".txt";

                    // if we have some path parts
                    if(pathParts != null)
                    {
                        // get last index
                        var lastIndex = pathParts.Length - 1;

                        // get extension from parts
                        if (lastIndex > -1) extension = $".{pathParts[lastIndex]}";
                    }

                    // create resource helper
                    var helper = new ResourceHelper();

                    // get resource
                    var resource = helper.GetResource(ContentAssembly, path);
                    var resourceAsBytes = helper.ToByteArray(resource);

                    // set response type
                    context.Response.ContentType = MimeTypeMap.GetMimeType(extension);

                    // if we have no data
                    if(resourceAsBytes == null)
                    {
                        // return a 404 for the resource
                        context.Response.StatusCode = 404;
                        return Task.FromResult(0);
                    }

                    // return resource contents
                    return context.Response.WriteAsync(resourceAsBytes);
                }

                return next.Invoke();
            });
        }
    }
}
