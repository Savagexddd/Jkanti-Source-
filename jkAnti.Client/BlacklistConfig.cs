using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jkAnti.Client
{
    class BlacklistConfig
    {
        public List<string> peds { get; set; }

        public List<string> props { get; set; }

        public List<int> propHashes { get; set; }

        public List<string> weapons { get; set; }

        public List<string> vehicles { get; set; }

        public List<string> messages { get; set; }

        public List<string> events { get; set; }
    }
}
