using System;
using System.Collections.Generic;
using System.Text;
using R.Config;

namespace R.Services
{
    public interface ICustomComponentFactory : IService
    {
        List<IComponent> Components { get; }
        IComponent Create(CustomDefinition definition, List<IComponent> services);
        void Clear();
    }
}
