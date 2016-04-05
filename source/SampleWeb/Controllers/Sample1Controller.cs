using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb.Models;

namespace SampleWeb.Controllers
{
    public class Sample1Controller : Controller
    {
        // ASP.NET MVC 使用 jQuery EasyUI DataGrid 基礎篇 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid.html

        private Northwind db = new Northwind();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGridJSON()
        {
            var categories = db.Categories.OrderBy(x => x.CategoryID);

            JArray ja = new JArray();

            foreach (var item in categories)
            {
                var itemObject = new JObject
                                 {
                                     { "CategoryID", item.CategoryID },
                                     { "CategoryName", item.CategoryName },
                                     { "Description", item.Description }
                                 };
                ja.Add(itemObject);
            }

            int count = categories.Count();

            JObject result = new JObject();
            result.Add("total", count);
            result.Add("rows", ja);

            return Content(JsonConvert.SerializeObject(result), "application/json");
        }
    }
}