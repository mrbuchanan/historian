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
    internal static class HostingExtensions
    {
        /// <summary>
        /// Host a static page
        /// </summary>
        /// <param name="app">The App Builder to use</param>
        /// <param name="path">The path to host the page on</param>
        /// <param name="assembly">The assembly the page comes from (embedded resource)</param>
        /// <param name="resource">The name/path to the resource</param>
        /// <param name="replacements">Any replacements</param>
        public static void HostPage(this IAppBuilder app, string path, string resource,
            object replacements = null)
        {
            HostPage(app, path, resource, (e) => replacements);
        }

        /// <summary>
        /// Host a static page
        /// </summary>
        /// <param name="app">The App Builder to use</param>
        /// <param name="path">The path to host the page on</param>
        /// <param name="resource">The name/path to the resource</param>
        /// <param name="getModel">A method that takes the context and returns any replacements required</param>
        public static void HostPage(this IAppBuilder app, string path, string resource,
            Func<IOwinContext, object> getModel)
        {
            HostPageAsync(app, path, resource, async (context) => getModel(context));
        }

        /// <summary>
        /// Host a static page
        /// </summary>
        /// <param name="app">The App Builder to use</param>
        /// <param name="path">The path to host the page on</param>
        /// <param name="resource">The name/path to the resource</param>
        /// <param name="getModel">A method that takes the context and returns any replacements required</param>
        public static void HostPageAsync(this IAppBuilder app, string path, string resource,
            Func<IOwinContext, Task<object>> getModel)
        {
            // check inputs
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            if (string.IsNullOrWhiteSpace(resource)) throw new ArgumentNullException(nameof(resource));

            var assemblyName = new AssemblyName(ContentExtensions.ContentAssembly);

            app.Use(async (context, next) =>
            {
                var runNext = true;

                // get current request path
                var currentPath = context.Request.Path;

                // if we don't have a request for embedded content
                // run the next thing in the OWIN pipeline
                if (currentPath.HasValue)
                {
                    // get the value
                    var currentPathValue = currentPath.Value;

                    // check that it contains our path for embedded content
                    var hasContentPath = currentPathValue.Equals(path);

                    // if we don't have a request for embedded content
                    // run the next thing in the OWIN pipeline
                    if (hasContentPath)
                    {

                        // create a resource helper
                        var resourceHelper = new ResourceHelper();

                        // attempt to get the file parts
                        var fileParts = resource.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        var extension = ".txt";

                        // if we have some file parts
                        if (fileParts.Length > 0)
                        {
                            // get the last one
                            var lastIndex = fileParts.Length - 1;

                            // if we have a valid index, grab the extension
                            if (lastIndex > -1) extension = $".{fileParts[lastIndex]}";
                        }


                        // get resource from assembly
                        var view = resourceHelper.GetResourceAsString(assemblyName.FullName, resource);

                        // get replacements
                        var replacements = await getModel(context);

                        // always perform replacements
                        view = StringMapper.Replace(view, replacements);

                        // force response type to match resource type
                        context.Response.ContentType = MimeTypeMap.GetMimeType(extension);

                        // ensure nothing else runs after this
                        runNext = false;

                        // return content of resource
                        await context.Response.WriteAsync(view);
                    }
                }

                if (runNext)
                {
                    await next.Invoke();
                }
            });
        }
    }
}
