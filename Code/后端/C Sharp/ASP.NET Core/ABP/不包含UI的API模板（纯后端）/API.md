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
ABP框架原生的对文档调试的支持更好，第三方服务获取Token需要配置额外的客户端id
## 配置新的客户端令牌种子数据
在DomianOpeniddict文件夹下的OpenIddictDataSeedContributor 类中的CreateApplicationsAsync()方法里新增新配置的客户id
![[61d34f98c9e387725bbd3e56674b7ee.png]]
```
// 新增支持密码模式的客户端（修正后）
await CreateApplicationAsync(
    name: "ApiDemo_Password", // 新的client_id
    type: OpenIddictConstants.ClientTypes.Public, // 公开客户端（无需密钥）
    consentType: OpenIddictConstants.ConsentTypes.Implicit, // 跳过授权确认（常量写法更规范）
    displayName: "Password Grant Client",
    secret: null, // 公开客户端无需secret
    grantTypes: new List<string>
    {
        OpenIddictConstants.GrantTypes.Password // 核心：指定密码模式（替换掉AuthorizationCode）
    },
    permissions: new List<string>
    {
        OpenIddictConstants.Permissions.Endpoints.Token, // 允许访问token端点
        OpenIddictConstants.Permissions.GrantTypes.Password, // 显式授权密码模式
        OpenIddictConstants.Permissions.Scopes.Email,
        OpenIddictConstants.Permissions.Scopes.Profile,
        OpenIddictConstants.Permissions.Scopes.Roles,
        "ApiDemo" // 项目对应的scope（必须和你的项目名称一致）
    },
    scopes: commonScopes, // 复用公共scope（和原有客户端保持一致）
    redirectUri: null, // 密码模式无需重定向地址
    clientUri: null
);
```
![[bb7cf808179e470c8afcf17d00f0e7c.png]]