using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ArcGISServerGraphAPI.Infrastructure;
using ArcGISServerGraphAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArcGISServerGraphAPI.Controllers
{
    public class GraphController : Controller
    {
        // GET: Graph
        public ActionResult Index(string f = "")
        {
            return Format(Json(new
            {
                name = "ArcGIS Server Graph Extension",
                version = "0.1"
            }), f);
        }

        // GET: Default/Build
        public ActionResult Build(string path, string fields = "", string include = "", string f = "")
        {
            var root = new GraphNode(path, fields, include);
            root.ProcessNode();
            return Format(Json(root.Result), f);
        }

        // GET: Default/Map
        public ActionResult Map(string f = "")
        {
            var builder = new MapBuilder();
            return Format(Json(builder.BuildMap()), f);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentEncoding = contentEncoding
            };
        }

        private JsonResult Format(JsonResult rez, string f)
        {
            var jsonNetResult = rez as JsonNetResult;
            if (jsonNetResult == null || f.ToLower() != "pjson")
                return rez;

            jsonNetResult.Formatting = Formatting.Indented;
            return jsonNetResult;
        }
    }
}