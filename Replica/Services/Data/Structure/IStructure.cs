using System;
using System.Collections.Generic;
using System.Text;

namespace R.Config
{
    public interface IStructure
    {
        FieldCollection Fields { get; }
        string Name { get; }
        string Database { get; }
        string PK { get; }
        int TestRows { get; set; }
    }
}