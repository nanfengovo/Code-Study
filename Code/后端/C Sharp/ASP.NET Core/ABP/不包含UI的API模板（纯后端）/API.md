# 创建项目
> abp new ApiDemo -u none -version 8.3.4

# 不验证防伪令牌
在host层的module类下的ConfigureServices方法中配置下面的
```
        Configure<AbpAntiForgeryOptions>(options =>
        {
            options.TokenCookie.Expiration = TimeSpan.Zero;
            options.AutoValidate = false; //表示不验证防伪令牌
                                          //options.AutoValidateIgnoredHttpMethods.Remove("GET");
                                          //options.AutoValidateFilter =
                                          //    type => !type.Namespace.StartsWith("MyProject.MyIgnoredNamespace");
        });
```
#  获取Token
## 配置新的客户端令牌种子数据
在DomianOpeniddict文件夹下的OpenIddictDataSeedContributor 类中的CreateApplicationsAsync()方法里新增新配置的客户id(ABP框架原生的对文档调试的支持更好，第三方服务获取Token需要配置额外的客户端id)
![[61d34f98c9e387725bbd3e56674b7ee.png]]
