using System;
using System.Collections.Generic;
using System.Linq;
using DbUp.Engine;
using DbUp.Support;

namespace Gofer.Core.Migrations
{
    public class ExecutionLimitScriptFilter : IScriptFilter
    {
        private readonly int _limit;

        public ExecutionLimitScriptFilter(int limit)
        {
            _limit = limit;
        }

        public IEnumerable<SqlScript> Filter(IEnumerable<SqlScript> sorted, HashSet<string> executedScriptNames,
            ScriptNameComparer comparer)
        {
            var filtered = sorted.Where(s =>
                s.SqlScriptOptions.ScriptType == ScriptType.RunAlways ||
                // ReSharper disable once PossibleUnintendedLinearSearchInSet
                !executedScriptNames.Contains(s.Name, comparer));

            return _limit < 0 ? filtered.SkipLast(Math.Abs(_limit)) :
                _limit > 0 ? filtered.Take(_limit) : filtered;
        }
    }
}