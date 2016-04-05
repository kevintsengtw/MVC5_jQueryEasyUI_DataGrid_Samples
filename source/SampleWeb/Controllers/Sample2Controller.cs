using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb.Services;
using System.Web.Mvc;

namespace SampleWeb.Controllers
{
    public class Sample2Controller : Controller
    {
        // ASP.NET MVC 使用 jQuery EasyUI DataGrid 分頁功能 (DataGrid Pagination)
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_8.html

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
            jo.Add("rows", service.GetJsonForGrid(page, rows));

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
    }
}