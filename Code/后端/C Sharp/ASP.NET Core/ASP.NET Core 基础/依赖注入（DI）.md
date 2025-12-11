# 控制反转（Inversion of Control,IOC）
# 依赖注入（Dependency Injection,DI）
> 依赖注入是实现控制反转的一种方式
> 依赖注入可以简化模块的组装程度，降低模块间的耦合

# 实现控制反转的两种方式
## 1、服务定位器（ServiceLocatior）
> ServicaeLocator.GetService<>

```
using Microsoft.Extensions.DependencyInjection;

namespace DI1
{
    public class Program
    {

        private static readonly IService _service;



        static void Main(string[] args)
        {
            //IService service = new ServiceA();
            //service.Serve();

            //_service.Serve();

            ServiceCollection service = new ServiceCollection();
            service.AddTransient<ServiceA>();
            using (ServiceProvider sp = service.BuildServiceProvider())
            {
                var t = sp.GetService<ServiceA>();
                t.Serve();
            }
                
            
            

        }
    }

    public interface IService
    {
        void Serve();
    }

    public class ServiceA : IService
    {
        public void Serve()
        {
            Console.WriteLine("Service A Called");
        }
    }

    public class ServiceB : IService
    {
        public void Serve()
        {
            Console.WriteLine("Service B Called");
        }
    }
}
```
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

# 生命周期
> 瞬时（Transient），范围(scoped),单例（Singleton）

使用ReferenceEquals(t,t1)来判断是不是同一个对象
```
            ServiceCollection service = new ServiceCollection();
            service.AddTransient<ServiceA>();
            using (ServiceProvider sp = service.BuildServiceProvider())
            {
                var t = sp.GetService<ServiceA>();
                t.Serve();

                var t1 = sp.GetService<ServiceA>();
                Console.WriteLine(ReferenceEquals(t,t1));
            }
            
```

## Scoped 范围可以设定
using的范围就是scoped的范围

不要在长生命周期中引用短生命周期的对象

## IServiceProvider的服务定位器方法：
* T GetService () 如果获取不到对象，则返回null
* *GetRequiredService（） 如果获取不到则抛异常

# 