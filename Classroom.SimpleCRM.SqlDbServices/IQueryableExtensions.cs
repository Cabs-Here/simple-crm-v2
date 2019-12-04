using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            ColumnMapping mappings)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }
            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var sortClauses = orderBy.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var sortClause in sortClauses)
            {
                var clause = sortClause.Trim().ToUpper();
                var clauseParts = clause.Split(' ');
                if (clauseParts.Length > 2)
                    throw new ArgumentException("Invalid sort clause " + clause);
                if (clauseParts.Length > 1 && clauseParts[1].ToUpper() != "DESC" && clauseParts[1].ToUpper() != "ASC")
                    throw new ArgumentException("Invalid sort direction " + clauseParts[1]);
                var orderDesc = clauseParts[1].ToUpper() == "DESC";

                var map = mappings.Mappings.FirstOrDefault(col => col.PropertyName
                    .Equals(clauseParts[0], StringComparison.InvariantCultureIgnoreCase));
                if (map == null)
                    throw new ArgumentException("Invalid sort field " + clause);

                foreach (var col in map.ColumnNames.Reverse())
                {
                    source = source.OrderBy(col + (orderDesc ? " DESC" : " ASC"));
                }
            }
            return source;
        }
    }
}