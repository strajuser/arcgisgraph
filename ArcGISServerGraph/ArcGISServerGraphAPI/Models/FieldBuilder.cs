using System.Collections.Generic;
using System.Linq;

namespace ArcGISServerGraphAPI.Models
{
    public static class FieldBuilder
    {
        public static List<FieldInfo> GetFields(IEnumerable<string> fields)
        {
            fields = fields.ToList();

            return fields.Where(x => !x.Contains("/")).Select(x => new FieldInfo
            {
                Name = x,
                Fields = GetFields(fields.Where(y => y.StartsWith(x + "/")).Select(y => y.Replace(x + "/", "")))
            }).ToList();
        }
    }
}