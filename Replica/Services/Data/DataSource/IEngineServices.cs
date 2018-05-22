using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.Config;

namespace X.Services.Plugins.Db
{
    public interface IEngineServices
    {
        //void Close(bool dispose = true);
        //void Open(string connectionString);
        bool IsOpen { get; }

        //SCHEMA
        void CreateTable(string tableName, Field[] fields,bool createConstraints=true);
        void DropTable(string tableName);
        void AddField(Field field, string tableName);
        void RemoveField(Field field, string tableName);
        void ClearTable(string tableName);
        void CreateConstraint(string tableName, Field[] fields);
        void RemoveConstraint(string tableName, Field[] fields);
        //void RenameField(Field field, string newName);
        //void ChangeFieldType(Field field, FieldType newType);
        void RemoveField(string fieldName, string tableName);
        bool TableExist(string tableName);
        void DeleteAllTables(string instanceName);
        bool UseDatabase(string databaseName);
        string[] TableNames { get; }
        IList<Field> TableFields(string tableName);
    }
}
