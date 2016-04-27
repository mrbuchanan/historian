using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard.Dashboard.Content
{
    /// <summary>
    /// An in memory cache for assemblies
    /// </summary>
    internal class AssemblyCache
    {
        /// <summary>
        /// Dictionary for storing loaded assemblies
        /// </summary>
        private readonly Dictionary<string, Assembly> _cache;

        public AssemblyCache()
        {
            // create cache
            _cache = new Dictionary<string, Assembly>();
        }

        /// <summary>
        /// Get and/or load the specified assembly
        /// </summary>
        /// <param longName="longName">The full longName of the assmebly to get</param>
        /// <returns>An assembly</returns>
        public Assembly Get(string longName)
        {
            Assembly assembly;
            var success = _cache.TryGetValue(longName, out assembly);

            if (!success)
            {
                assembly = Assembly.Load(longName);
                _cache[longName] = assembly;
            }

            return assembly;
        }

        public Assembly GetByShortName(string shortName)
        {
            var found = _cache.FirstOrDefault(a => a.Value.GetName().Name == shortName);

            return found.Value;
        }
    }
}
