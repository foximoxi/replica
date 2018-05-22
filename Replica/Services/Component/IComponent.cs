using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using X.Public;

namespace X.Config
{
    public interface IComponent: IComponentBase
    {
        Task Invoke(IRequestContext context);
        X.Config.EndPointUri CustomUri { get; }
        void Start();
    }
}