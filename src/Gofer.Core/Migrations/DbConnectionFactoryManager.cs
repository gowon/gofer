using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DbUp.Engine.Transactions;
using DbUp.Support;

namespace Gofer.Core.Migrations
{
    // https://github.com/DbUp/DbUp/issues/342#issuecomment-402873100
    /// <summary>
    ///     Manages Sql Database Connections
    /// </summary>
    public class DbConnectionFactoryManager : DatabaseConnectionManager
    {
        /// <summary>
        ///     Manages Sql Database Connections
        /// </summary>
        /// <param name="factory"></param>
        public DbConnectionFactoryManager(Func<IDbConnection> factory)
            : base(new DelegateConnectionFactory((log, dbManager) =>
            {
                var connection = factory.Invoke();
                if (connection is SqlConnection sqlConnection && dbManager.IsScriptOutputLogged)
                {
                    sqlConnection.InfoMessage += (sender, e) => log.WriteInformation("{0}\r\n", e.Message);
                }

                return connection;
            }))
        {
        }

        public override IEnumerable<string> SplitScriptIntoCommands(string scriptContents)
        {
            var commandSplitter = new SqlCommandSplitter();
            var scriptStatements = commandSplitter.SplitScriptIntoCommands(scriptContents);
            return scriptStatements;
        }
    }
}