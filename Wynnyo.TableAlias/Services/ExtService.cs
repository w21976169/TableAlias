using System;
using Wynnyo.TableAlias.Db;
using Wynnyo.TableAlias.Entities;
using Wynnyo.TableAlias.Extensions;

namespace Wynnyo.TableAlias.Services
{
    public class ExtService
    {
        public DbContext _dbContext;

        public ExtService()
        {
            _dbContext = new DbContext();
        }

        #region 扩展测试

        public void TestQueryExt()
        {
            // 参数为 null: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            _dbContext.Db.Queryable<LogEntity>().ASIf(null).ToDataTable();

            // 参数为 "": SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            _dbContext.Db.Queryable<LogEntity>().ASIf("").ToDataTable();

            // 参数为 "Sys_Log_1": SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_1]
            _dbContext.Db.Queryable<LogEntity>().ASIf("Sys_Log_1").ToDataTable();
        }

        #endregion

    }
}