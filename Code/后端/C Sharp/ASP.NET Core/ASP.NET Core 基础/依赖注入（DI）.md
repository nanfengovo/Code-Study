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

# DI综合案例
## 需求说明
1、目的：演示DI的能力
2、有配置服务、日志服务，然后再开发一个邮件发送器服务。可以通过配置服务来从文件、环境变量、数据库等地方读取配置，可以通过日志服务来将程序运行时中的日志信息写入文件控制台数据库等
3、说明：案例中开发了自己的日志、配置等接口，这只是在揭示原理

## 实现1
1、创建4个.NET Core类库项目，ConfigServices是配置服务的项目，LogServices是日志服务的项目，MailServices是邮件发送器的项目，然后再建一个.NETCore控制台项目MailServicesConsole来调用MailServices.MailServices项目引用ConfigServices项目和LogServices项目，而MailServicesConsole项目引用MailServices项目
2、编写类库项目LogServices,创建ILogProvider接口，编写实现类ConsoleLogProvider.编写一个ConsoleLogProviderExtensions定义扩展方法AddConsoleLog,namespace和IServiceCollection一致    

依赖注入的基本使用
> Install-package Microsoft.Extensions.DependencyInjection
```
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<ILogProvider, ConsoleLogProvider>();
            services.AddScoped<IMailService, MailService>();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var mail = serviceProvider.GetRequiredService<IMailService>();
                mail.SendEmail("Hello", "1", "111");
            }

            Console.Read();
```
