using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb2.Services;
using System.Web.Mvc;

namespace SampleWeb2.Controllers
{
    public class Sample4Controller : Controller
    {
        // ASP.NET MVC 使用 jQuery EasyUI DataGrid - 多欄排序 (Multiple Column Sorting) Part.1 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_10.html

        // ASP.NET MVC 使用 jQuery EasyUI DataGrid - 多欄排序 (Multiple Column Sorting) Part.2 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_11.html

        // ASP.NET MVC 使用 jQuery EasyUI DataGrid - 多欄排序 (Multiple Column Sorting) Part.3 
        // http://kevintsengtw.blogspot.tw/2013/10/aspnet-mvc-jquery-easyui-datagrid_12.html

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