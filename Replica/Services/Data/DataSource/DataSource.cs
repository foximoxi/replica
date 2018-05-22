using System;

namespace X.Config
{
    public class DataSource
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public DatabaseEngine Engine { get;set; }
        public bool IsDefaultSource { get; set; }
        public bool ManagedFlag { get; set; }

        public bool Equals(DataSource ds)
        {
            if (String.Compare(Name, ds.Name, true) != 0)
                return false;
            if (String.Compare(ConnectionString, ds.ConnectionString, true) != 0)
                return false;
            if (Engine != ds.Engine)
                return false;
            if (IsDefaultSource != ds.IsDefaultSource)
                return false;
            return true;
        }
    }
}