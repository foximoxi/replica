using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using R.Public;

namespace R.Config
{
    public interface IComponent: IComponentBase
    {
        Task Invoke(IRequestContext context);
        R.Config.EndPointUri CustomUri { get; }
        void Start();
    }
}