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

## 配置刷新Token的机制
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
![[Pasted image 20260104225104.png]]
![[Pasted image 20260104225136.png]]
# 基于RBAC的权限管理
权限：
		获取指定组的指定关键字的权限 ([get] /api/permission-management/permissions)
		 授予/取消角色/用户权限 ([put] /api/permission-management/permissions)
角色：
		获取所有角色  ([get] /api/identity/roles/all)
		条件查询获取所有角色 ([get] /api/identity/roles)
		添加新角色 ([post] /api/identity/roles )
		根据角色id获取角色信息  ([get] /api/identity/roles/{id} )
		根据角色id更新角色信息（是否公开，是否默认）([put] /api/identity/roles/{id} )
		根据角色id删除角色信息  ([delete] /api/identity/roles/{id} )
用户：
		根据用户id获取用户信息（[get] /api/identity/users/{id}）
		根据用户id更新用户信息（[put] /api/identity/users/{id}）
		根据用户id删除用户（[delete] /api/identity/users/{id}）
		条件查询获取所有的用户 （[get] /api/identity/users）
		添加新用户（[post] /api/identity/users）
		根据用户id获取该用户被分配的角色 （[get] /api/idenrtity/users/{id}/roles）
		给指定用户重新分配角色（会覆盖之前的） （[put] /api/idenrtity/users/{id}/roles）
		获取系统中**当前登录用户有权分配**的所有角色列表 （[get] /api/identity/users/assignable-roles）
		确定具体的**用户名**来获取该用户的详细信息（[get] /api/identity/user/by-username/{username} ）
		通过**电子邮箱地址**来精准查找并获取用户信息 （[get] /api/identity/user/by-email/{email}）