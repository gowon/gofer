using System;
using System.Data;
using DbUp.Engine;

namespace Gofer.Core.Migrations
{
    public class SqlServerCreateDatabaseScript : IScript
    {
        private readonly string _databaseName;

        public SqlServerCreateDatabaseScript(string databaseName)
        {
            _databaseName = databaseName;
        }

        public string ProvideScript(Func<IDbCommand> dbCommandFactory)
        {
            return $"CREATE DATABASE {_databaseName}";
        }
    }
}