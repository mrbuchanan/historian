using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard.Dashboard.Content
{
    /// <summary>
    /// A helper for getting embedded resources
    /// </summary>
    internal class ResourceHelper
    {
        protected readonly AssemblyCache _cache;

        public ResourceHelper()
        {
            _cache = new AssemblyCache();
        }

        protected string GetShortAssemblyName(string longName)
        {
            var loaded = _cache.Get(longName);

            return loaded.GetName().Name;
        }

        /// <summary>
        /// Get whether or not a resource exists in the provided assembly
        /// </summary>
        /// <param name="assembly">The assembly to search</param>
        /// <param name="resource">The resource to find</param>
        /// <returns>True if the resource exists, False if it does not</returns>
        public virtual bool ContainsResource(string assembly, string resource)
        {
            // get assembly instance
            var loadedAssembly = _cache.Get(assembly);

            // if it's loaded
            if (loadedAssembly != null)
            {
                // get full resource name
                resource = string.Format("{0}.{1}", loadedAssembly.GetName().Name, resource);

                // get resource as a stream
                var resources = loadedAssembly.GetManifestResourceNames();

                return resources.Contains(resource);
            }

            // if no assembly loaded
            return false;
        }

        /// <summary>
        /// Gets an embedded resource, from the given assembly and resource name
        /// </summary>
        /// <param name="assembly">The name of the assembly to get resources from</param>
        /// <param name="resource">The full resource name</param>
        /// <returns>A stream containing the resource</returns>
        /// <example>
        /// var resourceHelper = new EmbeddedResourceHelper();
        /// var assembly = "LivestockOneWeb";
        /// return resourceHelper.GetResource(assembly, "Content.16placeholder.png");
        /// </example>
        public virtual Stream GetResource(string assembly, string resource)
        {
            // get assembly instance
            var loadedAssembly = _cache.Get(assembly);

            // if it's loaded
            if (loadedAssembly != null)
            {
                // get full resource name
                resource = string.Format("{0}.{1}", loadedAssembly.GetName().Name, resource);

                // get resource as a stream
                var resourceStream = loadedAssembly.GetManifestResourceStream(resource);

                // return found resource
                return resourceStream;
            }

            // if no assembly loaded
            return null;
        }

        /// <summary>
        /// Gets an embedded resource, from the given assembly and resource name
        /// </summary>
        /// <param name="assembly">The name of the assembly to get resources from</param>
        /// <param name="resource">The full resource name</param>
        /// <returns>A string containing the resource</returns>
        /// <example>
        /// var resourceHelper = new EmbeddedResourceHelper();
        /// var assembly = "LivestockOneWeb";
        /// return resourceHelper.GetResourceAsString(assembly, "Content.16placeholder.png");
        /// </example>
        public virtual string GetResourceAsString(string assembly, string resource)
        {
            // get resource
            try
            {
                var s = GetResource(assembly, resource);

                // return resource as string
                return ToString(s);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a stream into a strong
        /// </summary>
        /// <param name="s">The stream to convert to a string</param>
        /// <returns>The string representation of the stream</returns>
        public string ToString(Stream s)
        {
            // if no stream, return
            if (s == null) return null;

            s.Seek(0, SeekOrigin.Begin);

            // use streamreader
            using (var sRead = new StreamReader(s))
            {
                // read to string
                return sRead.ReadToEnd();
            }
        }

        /// <summary>
        /// Convert Stream to a MemoryStream
        /// </summary>
        /// <param name="s">The stream to convert</param>
        /// <returns>A MemoryStream containing the contents of the provided Stream</returns>
        public MemoryStream ToMemoryStream(Stream s)
        {
            if (s == null) return null;

            // seek to start of stream
            s.Seek(0, SeekOrigin.Begin);

            // create buffer
            var buffer = new byte[16 * 1024];

            // use a MemoryStream
            using (MemoryStream ms = new MemoryStream())
            {
                // container for read
                int read;

                // iterate over whole stream
                while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                {
                    // write contents from buffer
                    ms.Write(buffer, 0, read);
                }

                // return MemoryStream
                return ms;
            }
        }

        /// <summary>
        /// Converts a Stream to a byte Array
        /// </summary>
        /// <param name="s">The Stream to convert</param>
        /// <returns>The contents of the Stream as a byte array</returns>
        public byte[] ToByteArray(Stream s)
        {
            // get MemoryStream
            var ms = ToMemoryStream(s);

            // convert to byte array
            return ToByteArray(ms);
        }

        /// <summary>
        /// Converts a MemoryStream to a byte array
        /// </summary>
        /// <param name="ms">The MemoryStream to convert</param>
        /// <returns>A byte array containing the contents of the MemoryStream</returns>
        public byte[] ToByteArray(MemoryStream ms)
        {
            if (ms == null) return null;

            return ms.ToArray();
        }

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public byte[] ToByteArray(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;

            return Encoding.Default.GetBytes(s);
        }

        /// <summary>
        /// Convert a byte array to a stream
        /// </summary>
        /// <param name="bytes">The bytes to convert</param>
        /// <returns>A Stream</returns>
        public Stream ToStream(byte[] bytes)
        {
            if (bytes == null) return null;

            var stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
