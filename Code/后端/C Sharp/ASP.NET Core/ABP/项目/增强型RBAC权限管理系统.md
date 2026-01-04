> 传统的RBAC是指基于角色的权限管理，具体来说就是新增权限 → 给角色分配权限 → 给用户分配角色
> 增强型的RBAC：新增权限→ 给角色分配权限 → 给用户分配角色（这里也可以直接给用户分配权限）

# 技术选型
> vue3 + Asp.net.core webapi  + sqlserver
> 简单版（V1快速实现）：Soybeanadmin(**NaiveUI** 版) +Abp (8.3.4)
> 进阶版 （V2学习React）: Soybeanadmin(React 版 )+ Abp(8.3.4)
> 最终版（V3手搓）：计划使用Vue3,React各写一个版本的前端 ，后端用原生的   Asp.net.core webapi  自己去搭建架构 + pgsql  ；尽量再实现一个angular版的和WPF版的，甚至使用ele把web发布为桌面的

# 简单版（V1快速实现）：Soybeanadmin(**NaiveUI**版) +Abp (8.3.4)
## 前端
### 前端技术栈
* Vue3
* TypeScript
* ElegantRouter
* Pinia
* UnoCSS*

## 后端
## 新建项目
> abp new RbacV1 -u none -v 8.3.4

## 替换数据库连接字符串
> "Server=.;Database=OMSV1.0;User Id=sa;Password=aaaa2624434145;Encrypt=True;TrustServerCertificate=True"

## 改造ABP原生的二次防伪验证为通过携带Token访问
### 关闭双重防伪验证
在RbacV1HttpApiHostModule.cs的ConfigureServices方法中添加
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

### 配置额外的客户端id
在OpenIddictDataSeedContributor.cs的CreateApplicationsAsync()方法中添加新的客户端id
```
            // 新增支持密码模式的客户端（修正后）
            await CreateApplicationAsync(
                name: "RRbacV1_Password", // 新的client_id
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
                    "RRbacV1" // 项目对应的scope（必须和你的项目名称一致）
                },
                scopes: commonScopes, // 复用公共scope（和原有客户端保持一致）
                redirectUri: null, // 密码模式无需重定向地址
                clientUri: null
            );
```

### 配置刷新Token的机制
```
            // 新增支持密码模式的客户端（修正后）
            await CreateApplicationAsync(
                name: "RRbacV1_Password",
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Password Grant Client",
                secret: null,
                grantTypes: new List<string>
                {
                     OpenIddictConstants.GrantTypes.Password,
                     OpenIddictConstants.GrantTypes.RefreshToken // 【1. 新增】允许刷新令牌模式
                },
                permissions: new List<string>
                {
                     OpenIddictConstants.Permissions.Endpoints.Token,
                     OpenIddictConstants.Permissions.GrantTypes.Password,
                     OpenIddictConstants.Permissions.GrantTypes.RefreshToken, // 【2. 新增】显式授权刷新令牌权限
                     OpenIddictConstants.Permissions.Scopes.Email,
                     OpenIddictConstants.Permissions.Scopes.Profile,
                     OpenIddictConstants.Permissions.Scopes.Roles,
                     "RRbacV1"
                },
                scopes: commonScopes,
                // 注意：在某些 ABP 封装版本中，可能还需要显式设置 AllowOfflineAccess
                redirectUri: null,
                clientUri: null
            );
```

### 配置CORS
```
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=OMSV1.0;User Id=sa;Password=aaaa2624434145;Encrypt=True;TrustServerCertificate=True"
  },
  "OpenIddict": {
    "Applications": {
      "RbacV1_Swagger": {
        "ClientId": "RbacV1_Swagger",
        "RootUrl": "https://localhost:44376"
      }
    }
  }
}

```

```
private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
{
    context.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder
                .WithOrigins(configuration["App:CorsOrigins"]?
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(o => o.RemovePostFix("/"))
                    .ToArray() ?? Array.Empty<string>())
                .WithAbpExposedHeaders()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}
```