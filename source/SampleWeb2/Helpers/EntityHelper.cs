using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SampleWeb2.Helpers
{
    public class EntityHelper
    {
        /// <summary>
        /// Entities the property names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<string> EntityPropertyNames<T>(DbContext dbContext)
        {
            string entityName = typeof(T).Name;

            var members = new Dictionary<string, IEnumerable<string>>();
            var metadata = ( (IObjectContextAdapter)dbContext ).ObjectContext.MetadataWorkspace;
            var tables = metadata.GetItems<EntityType>(DataSpace.CSpace);

            foreach (var e in tables.OfType<EntityType>())
            {
                members.Add
                    (
                        e.Name,
                        e.Members
                         .Where(m => m.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType)
                         .Select(m => m.Name)
                    );
            }

            var result = members.Where(x => x.Key == entityName)
                                .Select(x => x.Value).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Keys the members.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> KeyMembers<T>(DbContext dbContext) where T : class
        {
            List<string> result = new List<string>();

            string entityName = typeof(T).Name;

            string keyMembers = EntityHelper.GetAllEntityKeyMembers(dbContext)
                                            .FirstOrDefault(x => x.Key == entityName).Value;

            if (!string.IsNullOrWhiteSpace(keyMembers))
            {
                result = keyMembers.Split(',').ToList();
            }
            return result;
        }

        /// <summary>
        /// Gets all entity key members.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetAllEntityKeyMembers(DbContext dbContext)
        {
            var result = new Dictionary<string, string>();

            var metadata = ( (IObjectContextAdapter)dbContext ).ObjectContext.MetadataWorkspace;
            var tables = metadata.GetItems(DataSpace.CSpace);

            foreach (var e in tables.OfType<EntityType>())
            {
                result.Add(e.Name, string.Join(",", e.KeyMembers));
            }
            return result;
        }

        /// <summary>
        /// Gets the property sort tuples.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbContext">The database context.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="order">The order.</param>
        /// <returns>List&lt;Tuple&lt;System.String, System.String&gt;&gt;.</returns>
        public static List<Tuple<string, string>> GetPropertySortTuples<T>(
            DbContext dbContext,
            string propertyName,
            string order)
        {
            var result = new List<Tuple<string, string>>();

            //取得 Entity 所有的 Property 名稱
            var entityPropertyNames = EntityHelper.EntityPropertyNames<T>(dbContext);

            var propertyNameOptions = propertyName.Split(',').ToList();

            var orderOptions = order.Split(',').ToList();

            for (int i = 0; i < propertyNameOptions.Count; i++)
            {
                var columnName = propertyNameOptions[i].Trim();

                var sortOrder = string.IsNullOrWhiteSpace(orderOptions[i])
                    ? "asc"
                    : orderOptions[i];

                var propertyNames =
                    entityPropertyNames as string[] ?? entityPropertyNames.ToArray();

                if (!propertyNames.Contains(columnName))
                {
                    columnName = string.Empty;
                }

                if (!sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    &&
                    !sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    order = "asc";
                }

                if (!string.IsNullOrEmpty(columnName))
                {
                    result.Add(new Tuple<string, string>(columnName, sortOrder));
                }
            }

            return result;
        }
    }
}