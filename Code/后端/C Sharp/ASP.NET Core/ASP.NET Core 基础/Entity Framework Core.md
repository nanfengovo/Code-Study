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

