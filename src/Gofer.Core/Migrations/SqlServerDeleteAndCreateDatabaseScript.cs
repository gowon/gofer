using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using DbUp.Engine;
using Gofer.Core.Extensions;

namespace Gofer.Core.Migrations
{
    public class SqlServerDeleteAndCreateDatabaseScript : IScript
    {
        private readonly string _databaseName;

        public SqlServerDeleteAndCreateDatabaseScript(string databaseName)
        {
            _databaseName = databaseName;
        }

        public string ProvideScript(Func<IDbCommand> dbCommandFactory)
        {
            var sql = Assembly.GetExecutingAssembly().ReadResourceText("SqlServerDropAndCreateDatabase.sql");
            var command = dbCommandFactory();
            command.CommandText = sql;
            command.Parameters.Add(new SqlParameter("@DatabaseName", _databaseName));
            command.ExecuteNonQuery();

            return string.Empty;
        }
    }
}