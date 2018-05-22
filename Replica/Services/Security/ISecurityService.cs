using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using X.Public;
using X.Config;

namespace X.Services
{    
    public interface ISecurityService : IService
    {
        void RegisterSettingsObject(object settingsObj, Type settingsType);
    }
}