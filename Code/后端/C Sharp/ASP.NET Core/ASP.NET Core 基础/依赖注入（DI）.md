# 控制反转（Inversion of Control,IOC）
# 依赖注入（Dependency Injection,DI）
> 依赖注入是实现控制反转的一种方式
> 依赖注入可以简化模块的组装程度，降低模块间的耦合

# 实现控制反转的两种方式
## 1、服务定位器（ServiceLocatior）
> ServicaeLocator.GetService<>

## 2、依赖注入（Dependency Injection ,DI）

声明

# DI相关的概念
服务（service）: 对象
注册服务
服务容器：负责管理注册的服务
查询服务：创建对象及关联对象
对象生命周期 

## 根据类型来获取和注册服务
可以分别指定服务类型（service type）和实现类型（implementation type） 这两者可能相同也可能不同。服务类型