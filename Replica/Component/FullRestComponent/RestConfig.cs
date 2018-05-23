using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class RestConfig
    {
        public string Uri { get; set; }
        public bool Get { get; set; }
        public bool Put { get; set; }
        public bool Post{ get; set; }
        public bool Delete { get; set; }
        public bool All { get; set; }
        public string Schema { get; set; }
    }
}