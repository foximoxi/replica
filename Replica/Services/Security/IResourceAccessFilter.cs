using System;

namespace X.Config.Filters.Security
{
    public interface IResourceAccessFilter
    {
        bool CanAccess();
    }
}