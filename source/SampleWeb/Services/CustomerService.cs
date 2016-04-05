using System;
using System.Linq;
using System.Linq.Dynamic;
using Newtonsoft.Json.Linq;
using SampleWeb.Helpers;
using SampleWeb.Models;

namespace SampleWeb.Services
{
    public class CustomerService
    {
        private Northwind db = new Northwind();

        /// <summary>
        /// Totals the count.
        /// </summary>
        /// <returns></returns>
        public int TotalCount()
        {
            return db.Customers.Count();
        }

        /// <summary>
        /// Gets the json for grid.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public JArray GetJsonForGrid(int page = 1, int pageSize = 10)
        {
            JArray ja = new JArray();

            var categories = db.Customers.OrderBy(x => x.CustomerID)
                               .Skip(( page - 1 ) * pageSize)
                               .Take(pageSize);

            foreach (var item in categories)
            {
                var itemObject = new JObject
                                 {
                                     { "CustomerID", item.CustomerID },
                                     { "CompanyName", item.CompanyName },
                                     { "ContactName", item.ContactName },
                                     { "ContactTitle", item.ContactTitle },
                                     { "Address", item.Address },
                                     { "City", item.City },
                                     { "Region", item.Region },
                                     { "PostalCode", item.PostalCode },
                                     { "Country", item.Country },
                                     { "Phone", item.Phone },
                                     { "Fax", item.Fax }
                                 };
                ja.Add(itemObject);
            }
            return ja;
        }

        /// <summary>
        /// Gets the json for grid.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="propertyName">The sort.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public JArray GetJsonForGrid(
            int page = 1,
            int pageSize = 10,
            string propertyName = "CustomerID",
            string order = "asc")
        {
            //取得 Entity 所有的 Property 名稱
            var entityPropertyNames = EntityHelper.EntityPropertyNames<Customer>(db);

            if (!entityPropertyNames.Contains(propertyName))
            {
                propertyName = "CustomerID";
            }
            if (!order.Equals("asc", StringComparison.OrdinalIgnoreCase)
                &&
                !order.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                order = "asc";
            }

            JArray ja = new JArray();

            var query = db.Customers.AsQueryable();
            query = query.OrderBy(string.Format("{0} {1}", propertyName, order));
            query = query.Skip(( page - 1 ) * pageSize).Take(pageSize);

            foreach (var item in query)
            {
                var itemObject = new JObject
                                 {
                                     { "CustomerID", item.CustomerID },
                                     { "CompanyName", item.CompanyName },
                                     { "ContactName", item.ContactName },
                                     { "ContactTitle", item.ContactTitle },
                                     { "Address", item.Address },
                                     { "City", item.City },
                                     { "Region", item.Region },
                                     { "PostalCode", item.PostalCode },
                                     { "Country", item.Country },
                                     { "Phone", item.Phone },
                                     { "Fax", item.Fax }
                                 };
                ja.Add(itemObject);
            }
            return ja;
        }
    }
}