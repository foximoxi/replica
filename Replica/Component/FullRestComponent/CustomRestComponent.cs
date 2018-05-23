using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class CustomRestComponent : RestComponent
    {
        public bool GETEnabled { get; set; }
        public bool PUTEnabled { get; set; }
        public bool POSTEnabled { get; set; }
        public bool DELETEEnabled { get; set; }
        public Type IdentifierType { get; set; } //long,string
        public override Task Invoke(IRequestContext ctx)
        {
            return base.Invoke(ctx);
        }
    }
}