using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ArcGISServerGraphAPI.Models
{
    public class GraphNode
    {
        private readonly string _path;
        private readonly List<FieldInfo> _fields;
        private readonly List<FieldInfo> _include;

        public GraphNode(string path, string fields, string include)
            : this(path, FieldBuilder.GetFields(Split(fields)), FieldBuilder.GetFields(Split(include)))
        {
        }

        public GraphNode(string path, List<FieldInfo> fields, List<FieldInfo> include)
        {
            _path = path;
            _fields = fields;
            _include = include;

            if (!_fields.Any())
                _fields.Add(new FieldInfo("*"));
        }

        public void ProcessNode()
        {
            var rootObject = ArcGISServerDataProvider.GetData(_path);
            var token = RemoveFields(rootObject, _fields);

            foreach (var include in _include)
            {
                var array = token[include.Name] as JArray;
                if (array != null)
                {
                    var newArray = new JArray();
                    foreach (var item in array)
                    {
                        var itemCore = new GraphNode(GetPath(item), _fields.First(x => x.Name == include.Name).Fields,
                            include.Fields);
                        itemCore.ProcessNode();
                        newArray.Add(itemCore.Result);
                    }

                    token[include.Name] = newArray;
                }
            }

            Result = token;
        }

        private string GetPath(JToken token)
        {
            if (token is JObject)
            {
                if (token["id"] != null)
                    return Path.Combine(_path, token["id"].ToString());
                if (token["name"] != null)
                    return Path.Combine(_path, token["name"].ToString());
            }

            return Path.Combine(_path, token.ToString());
        }

        private static List<string> Split(string data)
        {
            return data.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private JToken RemoveFields(JToken token, List<FieldInfo> fields)
        {
            var container = token as JContainer;
            if (container == null)
                return token;

            List<JToken> tokensToRemove = new List<JToken>();
            foreach (var child in container.Children())
            {
                var prop = child as JProperty;
                if (prop == null)
                    continue;

                if (!fields.Any(x => x.Name == "*") && !fields.Select(x => x.Name).Contains(prop.Name))
                    tokensToRemove.Add(child);
            }

            foreach (var el in tokensToRemove)
                el.Remove();

            return token;
        }

        public object Result { get; set; }
    }
}