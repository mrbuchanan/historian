using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard.Dashboard.Content
{
    internal class ContentCache
    {
        private static Dictionary<string, byte[]> Content = new Dictionary<string, byte[]>(); 

        public static byte[] Get(string path)
        {
            byte[] content = null;

            var success = Content.TryGetValue(path, out content);

            if (!success) return null;

            return content;
        }

        public static void Add(string path, byte[] content)
        {
            if (Content.ContainsKey(path)) Content[path] = content;
            else Content.Add(path, content);
        }
    }
}
