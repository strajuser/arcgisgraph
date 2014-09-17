using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ArcGISServerGraphAPI.Models
{
    public class MapBuilder
    {
        public object BuildMap()
        {
            var folderFields = new FieldInfo("folders");
            var fields = new List<FieldInfo>
            {
                folderFields,
                new FieldInfo("*")
            };
            folderFields.Fields = fields;

            var root = new GraphNode("", fields, new List<FieldInfo> { folderFields });
            root.ProcessNode();

            var map = root.Result as JObject;
            if (map == null)
                return null;

            var services = GetServices(map);

            var folders = map.Children().OfType<JProperty>().FirstOrDefault(x => x.Name == "folders");
            if (folders!= null)
                folders.Remove();
            
            map["services"] = services;

            return map;
        }

        private JArray GetServices(JToken obj)
        {
            IEnumerable<JToken> rez = new JArray();

            var folders = obj["folders"] as JArray;
            if (folders != null)
                rez = folders.Aggregate(rez, (current, folder) => current.Concat(GetServices(folder)));

            var services = obj["services"] as JArray;
            if (services != null)
                rez = rez.Concat(services);
            return new JArray(rez);
        }
    }
}