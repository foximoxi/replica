using System;

namespace R.Config.Filters.Security
{
    public interface IResourceAccessFilter
    {
        bool CanAccess();
    }
}