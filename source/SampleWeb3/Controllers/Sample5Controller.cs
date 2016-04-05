using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb3.Services;
using System.Web.Mvc;

namespace SampleWeb3.Controllers
{
    public class Sample5Controller : Controller
    {
        // ASP.NET MVC 使用 jQuery EasyUI DataGrid - Checkbox 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_19.html

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