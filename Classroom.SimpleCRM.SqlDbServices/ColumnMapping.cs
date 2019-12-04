using System.Collections.Generic;
using System.Linq;

namespace Classroom.SimpleCRM.SqlDbServices
{
    public class ColumnMapping
    {
        public List<ColumnMappingValue> Mappings { get; private set; }

        public ColumnMapping(IEnumerable<ColumnMappingValue> mappings)
        {
            Mappings = mappings.ToList();
        }
    }
}