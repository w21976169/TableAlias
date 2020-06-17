using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlSugar;

namespace Wynnyo.TableAlias.Extensions
{
    public static class SqlSugarExtension
    {
        public static ISugarQueryable<T> ASIf<T>(this ISugarQueryable<T> queryable, string tableName)
        {

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                return queryable.AS(tableName);
            }

            return queryable;
        }

        public static IInsertable<T> ASIf<T>(this IInsertable<T> insertable, string tableName)
        {

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                return insertable.AS(tableName);
            }

            return insertable;
        }

        public static IUpdateable<T> ASIf<T>(this IUpdateable<T> updateable, string tableName) where T : class, new()
        {

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                return updateable.AS(tableName);
            }

            return updateable;
        }


        public static IDeleteable<T> ASIf<T>(this IDeleteable<T> deleteable, string tableName) where T : class, new()
        {

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                return deleteable.AS(tableName);
            }

            return deleteable;
        }
    }
}
