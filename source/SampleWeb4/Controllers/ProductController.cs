using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleWeb4.Models;
using System.Linq;
using System.Web.Mvc;

namespace SampleWeb4.Controllers
{
    public class ProductController : Controller
    {
        private Northwind db = new Northwind();

        public ActionResult GetByCategory(int? categoryId = null)
        {
            if (!categoryId.HasValue)
            {
                return new EmptyResult();
            }

            var products = db.Products
                             .Where(x => x.CategoryID == categoryId.Value)
                             .OrderBy(x => x.ProductID);

            return PartialView("_GetByCategory", products.ToList());
        }

        public ActionResult GetJsonByCategory(int? categoryId = null)
        {
            var products = db.Products
                .Where(x => x.CategoryID == categoryId.Value)
                .OrderBy(x => x.ProductID);

            JArray ja = new JArray();

            foreach (var item in products)
            {
                var itemObject = new JObject
                {
                    {"CategoryID", item.CategoryID},
                    {"ProductID", item.ProductID},
                    {"ProductName", item.ProductName},
                    {"QuantityPerUnit", item.QuantityPerUnit},
                    {"UnitPrice", item.UnitPrice},
                    {"UnitsInStock", item.UnitsInStock},
                    {"UnitsOnOrder", item.UnitsOnOrder}
                };
                ja.Add(itemObject);
            }

            return Content(JsonConvert.SerializeObject(ja), "application/json");
        }

    }
}