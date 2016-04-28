using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Dashboard.Dashboard.Content
{
    internal class StringMapper
    {
        /// <summary>
        /// Perform the given replacements on the provided text
        /// </summary>
        /// <param name="value">The value to search and replace</param>
        /// <param name="values">The map of key/values to replace</param>
        /// <returns>The replaced text</returns>
        public static string Replace(string value, IDictionary<string, object> values)
        {
            foreach (var key in values.Keys)
            {
                var val = values[key];
                val = val ?? String.Empty;
                if (val != null)
                {
                    value = value.Replace("{" + key + "}", val.ToString());
                }
            }
            return value;
        }

        /// <summary>
        /// Perform the given replacements on the provided text
        /// </summary>
        /// <param name="value">The value to search and replace</param>
        /// <param name="values">The map of key/values to replace</param>
        /// <returns>The replaced text</returns>
        public static string Replace(string value, object values)
        {
            return Replace(value, Map(values));
        }

        /// <summary>
        /// Maps an object to key/value pairs
        /// </summary>
        /// <param name="values">The object to map</param>
        /// <returns>The mapped key/value pairs</returns>
        public static IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }
    }
}
