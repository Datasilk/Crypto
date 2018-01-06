using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Crypto
{
    /// <summary>
    /// A collection of extension methods that directly or indirectly augment the System.Collections.Generic.IEnumerable interface.
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Indicates whether the enumerable is empty.
        /// </summary>
        /// <param name="value">The enumerable that will be tested.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this IEnumerable value)
        {
            if (value is string valueAsString)
            {
                return (valueAsString == "");
            }

            if (value is Array valueAsArray)
            {
                return (valueAsArray.LongLength == 0L);
            }

            if (value is ICollection valueAsCollection)
            {
                return (valueAsCollection.Count == 0);
            }

            return (value.GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Indicates whether the enumerable is null or empty.
        /// </summary>
        /// <param name="value">The enumerable that will be tested.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this IEnumerable value)
        {
            if (value is string valueAsString)
            {
                return string.IsNullOrEmpty(valueAsString);
            }

            return (value.IsNull() || value.IsEmpty());
        }

        /// <summary>
        /// Iterates through the input collection and returns the results as a <see cref=" DataTable"/>.
        /// </summary>
        /// <typeparam name="T">The type that a table wil be derived from.</typeparam>
        /// <param name="rows">The collection of rows that will be added to the table</param>
        /// <param name="tableName">The name of the table.</param>
        public static DataTable ToDataTable<T>(this IEnumerable<T> rows, string tableName)
        {
            if (rows.IsNull())
            {
                throw new ArgumentNullException(paramName: nameof(rows));
            }

            var type = typeof(T).UnwrapIfNullable();
            var table = new DataTable(!tableName.IsNullOrWhiteSpace() ? tableName : type.FullName);

            if (typeof(IDictionary<string, object>).IsAssignableFrom(type))
            {
                var enumerator = rows.GetEnumerator();

                if (enumerator.MoveNext())
                {
                    foreach (var kvp in (enumerator.Current as IDictionary<string, object>).OrderBy(a => a.Key, StringComparer.OrdinalIgnoreCase))
                    {
                        table.Columns.Add(kvp.Key, (kvp.Value?.GetType()?.UnwrapIfNullable() ?? typeof(object)));
                    }

                    do
                    {
                        table.Rows.Add((enumerator.Current as IDictionary<string, object>).OrderBy(a => a.Key, StringComparer.OrdinalIgnoreCase).Select(a => a.Value).ToArray());
                    } while (enumerator.MoveNext());
                }
            }
            else if (!type.IsValueType && (type != typeof(string)))
            {
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(a => (a.GetGetMethod(nonPublic: false) != null)).OrderBy(a => a.Name).ToArray();
                var propertyGetters = PropertyReader.Create<T>(includePublic: true, includePrivate: false, includeInstance: true, includeStatic: false).GetterCache.Select(a => a.Value).ToArray();

                for (var i = 0; i < properties.Length; i++)
                {
                    table.Columns.Add(properties[i].Name, properties[i].PropertyType.UnwrapIfNullable());
                }

                foreach (var row in rows)
                {
                    var values = new object[properties.Length];

                    for (var i = 0; i < properties.Length; i++)
                    {
                        values[i] = propertyGetters[i](row);
                    }

                    table.Rows.Add(values);
                }
            }
            else
            {
                table.Columns.Add("Value", type);

                foreach (var row in rows)
                {
                    table.Rows.Add(row);
                }
            }

            return table;
        }
    }
}