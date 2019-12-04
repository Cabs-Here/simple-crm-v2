using System.Collections.Generic;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class ColumnMappingValue
    {
        /// <summary>
        /// A class property the client may specify for sort
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// The destination column(s) that form the property value.
        /// Typically single value matching property name, but not required.
        /// </summary>
        public IList<string> ColumnNames { get; set; }

        public ColumnMappingValue(string propName)
        {
            PropertyName = propName;
            ColumnNames = new List<string>(new[] { propName });
        }
        public ColumnMappingValue(string propName, IEnumerable<string> columns)
        {
            PropertyName = propName;
            ColumnNames = new List<string>(columns);
        }
    }
}