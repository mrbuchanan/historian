﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian
{
    public class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Group { get; set; }

        public List<string> Tags { get; set; }

        public List<Message> Messages { get; set; }
    }
}
