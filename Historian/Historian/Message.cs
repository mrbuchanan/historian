using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    [Serializable]
    public sealed class Message
    {
        public string Contents { get; set; }

        public MessageKind Kind { get; set; }
        
        public string Channel { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Tags { get; set; }

        internal bool Check()
        {
            var valid = true;
            valid &= !string.IsNullOrWhiteSpace(Contents);
            valid &= !string.IsNullOrWhiteSpace(Channel);

            return valid;
        }
    }
}
