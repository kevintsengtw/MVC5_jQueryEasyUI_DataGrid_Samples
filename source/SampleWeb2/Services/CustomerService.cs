using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SampleWeb2.Helpers;
using SampleWeb2.Models;

namespace SampleWeb2.Services
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
        /// <param name="propertyName">The sort.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public JArray GetJsonForGrid(
            int page = 1,
            int pageSize = 10,
            string propertyName = "CustomerID",
            string order = "asc")
        {
            // 取得多個排序欄位與順序的 Tuple 結果
            var propertySortTuples =
                EntityHelper.GetPropertySortTuples<Customer>(db, propertyName, order);

            JArray ja = new JArray();

            var query = propertySortTuples.Count().Equals(1)
                ? this.GetSingleColumnSortingQuery(page, pageSize, propertyName, order)
                : this.GetMutilpieColumnSortingQuery(page, pageSize, propertyName, order);

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

        /// <summary>
        /// Gets the single column sorting query.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        private IQueryable<Customer> GetSingleColumnSortingQuery(
            int page,
            int pageSize,
            string propertyName,
            string sortOrder)
        {
            //取得 Entity 所有的 Property 名稱
            var entityPropertyNames = EntityHelper.EntityPropertyNames<Customer>(db);

            if (!entityPropertyNames.Contains(propertyName))
            {
                propertyName = EntityHelper.KeyMembers<Customer>(db).FirstOrDefault();
            }

            if (!sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                &&
                !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                sortOrder = "asc";
            }

            var query = db.Customers.AsQueryable();

            query = sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                ? query.OrderBy<Customer>(propertyName)
                : query.OrderByDescending<Customer>(propertyName);

            query = query.Skip(( page - 1 ) * pageSize).Take(pageSize);

            return query;
        }

        /// <summary>
        /// Gets the mutile column query.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        private IQueryable<Customer> GetMutilpieColumnSortingQuery(
            int page,
            int pageSize,
            string propertyName,
            string order)
        {
            //取得 Entity 所有的 Property 名稱
            var sortExpressions =
                EntityHelper.GetPropertySortTuples<Customer>(db, propertyName, order);

            var query = db.Customers.AsQueryable();
            IOrderedQueryable<Customer> orderQuery = null;

            for (var index = 0; index < sortExpressions.Count; index++)
            {
                if (sortExpressions[index].Item2.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    orderQuery = index.Equals(0)
                        ? query.OrderBy<Customer>(sortExpressions[index].Item1)
                        : orderQuery.ThenBy<Customer>(sortExpressions[index].Item1);
                }
                else
                {
                    orderQuery = index.Equals(0)
                        ? query.OrderByDescending<Customer>(sortExpressions[index].Item1)
                        : orderQuery.ThenByDescending<Customer>(sortExpressions[index].Item1);
                }
            }

            query = orderQuery;
            query = query.Skip(( page - 1 ) * pageSize).Take(pageSize);

            return query;
        }
    }
}