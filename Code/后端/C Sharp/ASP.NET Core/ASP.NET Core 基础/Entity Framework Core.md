# 什么叫ORM
ORM: ObjectRelationalMapping。让开发者用对象操作的形式操作数据库
## 有哪些ORM
EFCore,Dapper,SqlSugar,FreeSql
# EFCore与其他ORM比较
1、Entity Framework Core(EF Core)是微软官方的ORM框架。优点:功能强大、官方支持、生产效率高、力求屏蔽底层数据库差异;缺点:复杂、上手门槛高、不熟悉EFCore的话可能会进坑。
2、Dapper。优点:简单，N分钟即可上手，行为可预期性强;缺点:生产效率低，需要处理底层数据库差异。
3、EF Core是模型驱动(Model-Driven)的开发思想，Dapper是数据库驱动(DataBase-Driven)的开发思想的。没有优劣，只有比较。
4、性能:Dapper等≠性能高;EFCore≠性能差。
5、EF Core是官方推荐、推进的框架，尽量屏蔽底层数据库差异，.NET开发者必须熟悉，根据的项目情况再决定用哪个。

# EFCore和EF比较
1、EF有DB First、ModelFirst、Code First。 EF Core不支持模型优先，推荐使用代码优先，遗留系统可以使用Scaffold-DbContext来生成代码实现类似DBFirst的效果，但是推荐用
Code First 。
2、EF会对实体上的标注做校验，EFCore追求轻量化，不校验。
3、熟悉EF的话，掌握EFCore会很容易，很多用法都移植过来了
EF Core又增加了很多新东西。
4、EF中的一些类的命名空间以及一些方法的名字在EF Core中稍有不同。
5、EF不再做新特性增加。

# 搭建EF Core开发环境
> EFCore 是对于底层ADO.NET Core的封装，因此ADO.NET Core支持的数据库不一定被EFCore支持

## 使用MYSQL 
>https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql

## 使用Sql Server
### 搭建环境1
1、经典步骤:建实体类;建DbContext;生成数据库:编写调用EF Core的业务代码。
2、Book.cs
public class Book
{
	public long Id {get; set; }//主键
	public string Title {get; set; }//标题
	public DateTime PubTime { get; set; }//发布日期
	public double Price {get; set; }//单价
}

> Install-Package Microsoft.EntityFrameworkCore.SqlServer

### 搭建环境2
创建实现了IEntityTypeConfiguration接口的实体配置类，配置实体类和数据库表的对应关系
```
    internal class BookEntityConfig:IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("T_Books");
        }
    }
```

## 搭建环境3
创建继承自DbContext的类
```
public class TestDbContext:DbContext
{
    public DbSet<Book> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string conStr = "Server = .;Database = demo1;MultipleActiveResults = true";
        optionsBuilder.UseSqlServer(conStr);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly)
    }
}
```
## 搭建环境4
1、再在“程序包管理器控制台”中执行如下命令Add-igration InitialCreate会自动在项目的migrations文件夹中中生成操作数据库的C#代码。讲解一下生成代码的作用。
InitialCreate是什么?
2、代码需要执行后才会应用对数据库的操作。“程序包管理器控制台”中执行Update-database。
3、查看一下数据库，表建好了。