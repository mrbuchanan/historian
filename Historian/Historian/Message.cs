using System;
using System.Collections.Generic;

namespace Historian
{
    public class Message
    {
        public Message()
        {
            Tags = new List<string>();
        }

        public string Contents { get; set; }

        public MessageKind Kind { get; set; }

        public string KindName { get { return Kind.ToString(); } }
        
        public string Channel { get; set; }

        public string Title { get; set; }

        public List<string> Tags { get; set; }

        public DateTime Timestamp { get; set; }

        internal bool Check()
        {
            var valid = true;
            valid &= !string.IsNullOrWhiteSpace(Contents);
            valid &= !string.IsNullOrWhiteSpace(Channel);

            return valid;
        }
    }
}
