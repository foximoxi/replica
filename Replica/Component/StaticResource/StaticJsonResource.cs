using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using R.Config;

namespace R.Component
{
    public class StaticJsonResource: RestComponent
    {
        public override Task Invoke(IRequestContext ctx)
        {
            return base.Invoke(ctx);
        }
    }
}