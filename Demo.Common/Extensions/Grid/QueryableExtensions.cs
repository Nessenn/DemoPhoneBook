using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Threading.Tasks;

namespace Demo.Common.Extensions.Grid
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the Kendo DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
        public static async Task<DataSourceResult> ToDataSourceResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter)
        {
            if (filter.Filters.Count() != 0)
            {
                // Filter the data first
                queryable = Filter(queryable, filter);
            }

            // Calculate the total number of records (needed for paging)
            var total = queryable.Count();

            // Sort the data
            queryable = Sort(queryable, sort);

            // Finally page the data
            queryable = Page(queryable, take, skip);

            return new DataSourceResult
            {
                Data = await queryable.ToListAsync(),
                Total = total
            };
        }

        private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
        {
            if (filter != null)
            {
                // Collect a flat list of all filters
                var filters = filter.All().Distinct().ToList();

                //fix data type
                var entity = typeof(T);
                foreach (var f in filters)
                {
                    System.Reflection.PropertyInfo p = entity.GetProperty(FirstCharToUpper(f.Field), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    var newValue = CastPropertyValue(p, Convert.ToString(f.Value));
                    f.Value = newValue;
                }
               
                // Get all filter values as array (needed by the Where method of Dynamic Linq)
                var values = filters.Select(f => f.Value).ToArray();

                filter.Logic = "AND";

                // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                string predicate = filter.ToExpression(filters);

                // Use the Where method of Dynamic Linq to filter the data
                queryable = queryable.Where(predicate, values);
            }

            return queryable;
        }

        private static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        private static object CastPropertyValue(PropertyInfo property, string value)
        {
            if (property == null || String.IsNullOrEmpty(value))
                return null;
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                if (Enum.IsDefined(enumType, value))
                    return Enum.Parse(enumType, value);
            }
            if (property.PropertyType == typeof(bool))
                return value == "1" || value == "true" || value == "on" || value == "checked";
            else if (property.PropertyType == typeof(Uri))
                return new Uri(Convert.ToString(value));
            else
            {
                if (property.PropertyType.GenericTypeArguments.Length == 0)
                {
                    return Convert.ChangeType(value, property.PropertyType);
                }
                else
                {
                    return Convert.ChangeType(value, property.PropertyType.GenericTypeArguments.FirstOrDefault());
                }
            }
        }

        private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort != null && sort.Any())
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = String.Join(",", sort.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }
    }
}
