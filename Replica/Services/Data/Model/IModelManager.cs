using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.Config;
using X.Services.Plugins;

namespace X.Services
{
    public interface IModelManager
    {
        X.Services.Plugins.DbConnector DataSource { get; set; }
        IList<IStructure> Structures { get;}
        IEnumerable<string> GetNames();
        IStructure Get(string name);
        IStructure this[string name] { get; }
        void Save(IStructure model);
        void Remove(string name);
        void Remove(IStructure structure);

        void RegisterStructure(IStructure structure);
        void Update(ICollection<IStructure> structures);
        void CompleteUpdate();
    }
}