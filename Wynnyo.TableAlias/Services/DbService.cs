using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Wynnyo.TableAlias.Db;
using Wynnyo.TableAlias.Entities;

namespace Wynnyo.TableAlias.Services
{
    public class DbService
    {
        public DbContext _dbContext;

        public DbService()
        {
            _dbContext = new DbContext();
        }

        /// <summary>
        /// 初始化 Log 表
        /// </summary>
        public void Init()
        {
            // 生成 Log 表, 使用静态配置生成 dbTable: Sys_Log
            _dbContext.Db.CodeFirst.InitTables<LogEntity>();

            // 生成 Log 表, 使用动态配置生成 dbTable: Sys_Log_1
            _dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_1");
            _dbContext.Db.CodeFirst.InitTables<LogEntity>();


            // 生成 Log 表, 使用动态配置生成 dbTable: Sys_Log_2
            _dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_2");
            _dbContext.Db.CodeFirst.InitTables<LogEntity>();
        }

        /// <summary>
        /// Mapping 方式测试
        /// </summary>
        public void TestMapping()
        {
            var dbContext = new DbContext();
            // 使用默认静态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_1");
            // 使用动态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_1]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            //没有更换 db, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_1]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            var dbContext1 = new DbContext();
            // 更换 db, 动态配置失效, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            dbContext1.Db.Queryable<LogEntity>().ToDataTable();

            dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_2");
            // 重新 Mapping, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_2]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();
        }

        /// <summary>
        /// AS 方式测试
        /// </summary>
        public void TestAs()
        {
            var dbContext = new DbContext();
            // 使用默认静态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            // 使用动态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_1]
            dbContext.Db.Queryable<LogEntity>().AS("Sys_Log_1").ToDataTable();

            // As失效, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            // 重新 Mapping, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_2]
            dbContext.Db.Queryable<LogEntity>().AS("Sys_Log_2").ToDataTable();
        }

        /// <summary>
        /// 优先级 测试
        /// </summary>
        public void TestPriority()
        {
            var dbContext = new DbContext();

            dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_2");
            // 使用Mapping动态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_2]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

            // 使用As动态配置, 修改该操作的表别名, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_1]
            dbContext.Db.Queryable<LogEntity>().AS("Sys_Log_1").ToDataTable();

            // 继续使用Mapping动态配置, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_2]
            dbContext.Db.Queryable<LogEntity>().ToDataTable();

        }


    }
}