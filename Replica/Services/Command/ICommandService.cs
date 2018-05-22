using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Services
{
    public interface ICommandService : IService
    {
        void InsertTestData();
        bool InvokeCommand(string path,object ctx);
    }
}