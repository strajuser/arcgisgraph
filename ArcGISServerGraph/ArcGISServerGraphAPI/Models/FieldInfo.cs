using System.Collections.Generic;

namespace ArcGISServerGraphAPI.Models
{
    public class FieldInfo
    {
        public FieldInfo(string name = "")
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<FieldInfo> Fields { get; set; }
    }
}