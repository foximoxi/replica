using System;
using System.Collections.Generic;
using System.Text;
using R.Config;
using System.Threading.Tasks;

namespace R.Services
{
    public interface IComponentService : IService
    {
        void Update(R.Config.Update.IUpdatePackage pkg);
        void CompleteUpdate();
        void ReleaseConfiguration();
        List<R.Component.CustomRestComponent> Rest { get; set; }
        List<R.Component.StaticResourceComponent> Static { get; set; }
    }
}