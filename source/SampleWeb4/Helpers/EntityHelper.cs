using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace SampleWeb4.Helpers
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
    }
}