using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb.Services;
using System.Web.Mvc;

namespace SampleWeb.Controllers
{
    public class Sample3Controller : Controller
    {
        // ASP.NET MVC 使用 jQuery EasyUI DataGrid - 排序 (Sorting) 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_9.html

        private CustomerService service = new CustomerService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGridJSON(
            int page = 1,
            int rows = 10,
            string sort = "CustomerID",
            string order = "asc")
        {
            JObject jo = new JObject();
            jo.Add("total", service.TotalCount());
            jo.Add("rows", service.GetJsonForGrid(page, rows, sort, order));

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
    }
}