# SqlSugar表别名测试

**WynnyoBlog-[Wynnyo.com](http://wynnyo.com)**

## 介绍

SqlSugar 官方默认提供了 4 种别名的定义, 其中 2 种静态配置, 2 种动态配置.

[官方文档](http://www.codeisbug.com/Doc/8/1153)

## 使用方式

### 静态方式

- **实体特性**

  ```c#
  [SugarTable("Sys_Log")] //别名处理
  public class LogEntity {}
  ```

- **使用自定义特性**

  - 在 DbContext 进行配置

  ```c#
  SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
  {
      ConnectionString = Config.ConnectionString,
      DbType = DbType.SqlServer,
      IsAutoCloseConnection = true,
      ConfigureExternalServices = new ConfigureExternalServices()
      {
          EntityService = (property, column) =>
          {
             // 进行 列 的配置
          },
          EntityNameService = (type, entity) =>
          {
              var attributes = type.GetCustomAttributes(true);
              if (attributes.Any(it => it is TableAttribute))
              {
                  entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
              }
          }
      }
  });
  ```

  - 在 Entity 里进行配置

  ```c#
  [Table(Name = "Sys_Log")]   
  public class LogEntity {}
  ```

### 动态方式

- **MappingTables方式**

  - 使用方式: `db.MappingTables.Add(entityName, dbTableName);`
  - 特点:
    - 该方式是基于 Db 层;
    - 在同一个 Db 中, 该别名起作用;
    - 由于在一个 Db 都起作用, 所以要考虑多线程问题, 不同的线程应该都是新的 Db 实例;

- **As方式**

  - 使用方式: 

    ```C#
    Queryable<T>().As(dbTableName)
    Insertable<T>().As(dbTableName)
    Updateable<T>().As(dbTableName)
    Deleteable<T>().As(dbTableName)
    ```

  - 特点:

    - 方式是基于当前操作的;
    - 只在当前 Sql 操作起作用;

## 测试

使用 Serilog 将 Sql 输出 到日志文件.

### LogEntity

```c#
[SugarTable("Sys_Log")]
public class LogEntity
{
  // ....
}
```

### 测试1

- 测试 Db.MappingTables 的作用范围

  ```c#
  public void Test1()
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
      _dbContext.Db.Queryable<LogEntity>().ToDataTable();
      
      dbContext.Db.MappingTables.Add("LogEntity", "Sys_Log_2");
      // 重新 Mapping, 输出应为: SELECT [Id],[Title],[CreateTime] FROM [Sys_Log_2]
      dbContext.Db.Queryable<LogEntity>().ToDataTable();
  }
  ```

  

- 结果:

  ![image-20200616143503352](http://images.wynnyo.com/Markdown/image-20200616143503352.png?x-oss-process=style/wynnyo-style)

- **结论: 动态配置 Mapping 只在当前 Db 中有效.**

### 测试2

- 测试 AS 的作用范围

  ```c#
  public void Test2()
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
  ```

  

- 结果:

  ![image-20200616143535556](http://images.wynnyo.com/Markdown/image-20200616143535556.png?x-oss-process=style/wynnyo-style)

- **结论: 动态配置 AS 只在当前方法中有效;**

### 测试3

- 测试 MappingTables 和 As 的优先级

  ```c#
  public void Test3()
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
  ```

- 结果:

  ![image-20200616143607783](http://images.wynnyo.com/Markdown/image-20200616143607783.png?x-oss-process=style/wynnyo-style)

- **结论: 动态配置As > 动态配置Mapping > 静态配置SugarTable特性**

