# 改造后端
> 配置支持密码的鉴权验证和刷新Token,不验证防伪令牌，接口注释,配置Cors

## 1、配置不验证防伪令牌
在ABP_AdminHttpApiHostModule.cs的ConfigureServices方法中添加
```c#
Configure<AbpAntiForgeryOptions>(options =>
{
    options.TokenCookie.Expiration = TimeSpan.Zero;
    options.AutoValidate = false; //表示不验证防伪令牌
                                  //options.AutoValidateIgnoredHttpMethods.Remove("GET");
                                  //options.AutoValidateFilter =
                                  //    type => !type.Namespace.StartsWith("MyProject.MyIgnoredNamespace");
});
```
## 2、配置支持密码的鉴权验证和刷新Token
在OpenIddictDataSeedContributor .cs的CreateApplicationsAsync方法中
```c#
 // 新增支持密码模式的客户端（修正后）
 await CreateApplicationAsync(
     name: "ABP_Admin_Password",
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
              "ABP_Admin"
     },
     scopes: commonScopes,
     // 注意：在某些 ABP 封装版本中，可能还需要显式设置 AllowOfflineAccess
     redirectUri: null,
     clientUri: null
 );
```
## 3、添加接口注释功能
ABP_Admin.Application层，ABP_Admin.Application.Contracts层，ABP_Admin.HttpApi.Host层的 PropertyGroup中添加
```c#
  <PropertyGroup>
	<GenerateDocumentationFile>true</GenerateDocumentationFile>
	<NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>

```

ABP_AdminHttpApiHostModule.cs的ConfigureSwaggerServices方法中添加
```c#
// 1. ���� Host ��Ŀ������ע�ͣ�ͨ������ Controller��
var hostXml = $"{typeof(ABP_AdminHttpApiHostModule).Assembly.GetName().Name}.xml";
var hostPath = Path.Combine(AppContext.BaseDirectory, hostXml);
if (File.Exists(hostPath)) options.IncludeXmlComments(hostPath);

// 2. ���� Application.Contracts ��Ŀ��ע�ͣ������ӿں� DTO ������
var contractsXml = "ABP_Admin.Application.Contracts.xml";
var contractsPath = Path.Combine(AppContext.BaseDirectory, contractsXml);
if (File.Exists(contractsPath))
{
    options.IncludeXmlComments(contractsPath);
}

// 3. ���� Application ��Ŀ��ע�ͣ�ʵ�����ע�ͣ�
var applicationXml = "ABP_Admin.Application.xml";
var applicationPath = Path.Combine(AppContext.BaseDirectory, applicationXml);
if (File.Exists(applicationPath))
{
    options.IncludeXmlComments(applicationPath);
}
```
![[Pasted image 20260114235324.png]]
## 4、配置CORS
```C#
{
  "App": {
    "SelfUrl": "https://localhost:44371",
    "CorsOrigins": "http://localhost:9527",
    "RedirectAllowedUrls": ""
  },
  "ConnectionStrings": {
    "Default": "Server=.;Database=ABP-Admin;User Id=sa;Password=Abcd,1234;Encrypt=True;TrustServerCertificate=True;"
  },
  "AuthServer": {
    "Authority": "https://localhost:44371",
    "RequireHttpsMetadata": false,
    "SwaggerClientId": "ABP_Admin_Swagger"
  },
  "StringEncryption": {
    "DefaultPassPhrase": "IYyXZrz9jCBhGfSQ"
  }
}

```

# 改造前端
>  换登录页的logo和名字；去掉验证码登录，注册账号和其他账号登录；实现对接后端接口登录和Token的无感刷新

## 1、换登录页的logo和名字
## 2、去掉验证码登录注册账号和其他账号登录
## 3、实现对接后端接口登录和Token的无感刷新
### Soybeanadmin原来的登录流程
![[Untitled Diagram.svg]]
这里不用Login改用鉴权的接口
### 前端对接鉴权接口
![[Pasted image 20260118140719.png]]

#### 安装qs
> pnpm install qs

```TS
export function fetchLogin(userName: string, password: string) {

  return request<Api.Auth.LoginToken>({

    url: '/connect/token',

    method: 'post',

    data: qs.stringify({

      grant_type: 'password',

      client_id: 'ABP_Admin_Password',

      scope: 'ABP_Admin offline_access',

      username: userName,

      password: password

    }),headers:{

      'Content-Type': 'application/x-www-form-urlencoded'

    }

  });

}
```
![[后端/C Sharp/ASP.NET Core/ABP/项目/RBAC System/Untitled Diagram.svg]]

# 后端
## 接口
> base url: https://localhost:44371

## 登录鉴权模块
### 1、获取Token
> 地址：/connect/token
> 请求类型：post

#### 请求示例
Body: Content-Type: application/x-www-form-urlencoded

| 参数名        | 参数值                | 类型     | 是否必需 | 说明                       |
| ---------- | ------------------ | ------ | ---- | ------------------------ |
| grant_type | password           | string | 是    | 密码模式                     |
| client_id  | ABP_Admin_Password | string | 是    | 标识 是哪个“客户端应用”在请求 Token   |
| username   | admin              | string | 是    | 登录用户名（ABP 的 Identity 用户） |
| password   | 1q2w3E*            | string | 是    | 用户密码                     |
| scope      | ABP_Admin          | string | 是    | 声明你这个 Token **想要哪些权限范围** |

#### 状态码
> 200
#### 响应示例
```json
{

  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCRTlCN0NBQkZCRTE2NzI5NTZEQzE3MDI3REFENDVCQjg0RjRGOEQiLCJ4NXQiOiJPLW0zeXItLUZuS1ZiY0Z3SjlyVVc3aFBUNDAiLCJ0eXAiOiJhdCtqd3QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MS8iLCJleHAiOjE3Njg0MDEwMzIsImlhdCI6MTc2ODM5NzQzMiwiYXVkIjoiQUJQX0FkbWluIiwic2NvcGUiOiJBQlBfQWRtaW4iLCJqdGkiOiI5NThhMTY3Yi0yMTBiLTQxMmUtYTlmNS0yNjI2M2Y3MTcyYTQiLCJzdWIiOiI3Zjg1YWUwYS1mZjVjLTNmNGMtNTU1NS0zYTFlYmY4MTQ4MzUiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJhZG1pbiIsImVtYWlsIjoiYWRtaW5AYWJwLmlvIiwicm9sZSI6ImFkbWluIiwiZ2l2ZW5fbmFtZSI6ImFkbWluIiwicGhvbmVfbnVtYmVyX3ZlcmlmaWVkIjoiRmFsc2UiLCJlbWFpbF92ZXJpZmllZCI6IkZhbHNlIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsIm9pX3Byc3QiOiJBQlBfQWRtaW5fUGFzc3dvcmQiLCJjbGllbnRfaWQiOiJBQlBfQWRtaW5fUGFzc3dvcmQiLCJvaV90a25faWQiOiI1ODUzOGNiYS1mODhhLTRlOTItZTM2MS0zYTFlY2VlMDQwNGMifQ.o7mkmmxFc8KG5bngmO81ZSjVXPhBXafze5xh8W9RqA1g67oJ66Auz7b1HcJUGr9NCvglK1SqAuJ17gUy6-pA6RookJIAh9ehpzjIqCODBWkLlKB9xvTOxaajmRrQ43p5hLpgGlPQHiu4oxR5xky2yaU4uxzls2GbymNFk-eN1nVcRwi4aZj8V3wBs8YrFrECAFIi6u3Sc8dbm1W9WOyUByH6pT6VwXjXeuwVHlyi4OehOJHVQi0mzv079Dj6LwY-yod-mi_KyRWc7renvURwJw0WW8lDL5ywi3Z1K7pBujc2EzV4E45i3_sVXrdpSeJxxmYsz2QvhzE8avvi0o5iog",

  "token_type": "Bearer",

  "expires_in": 3599

}
```

### 2、登录时允许刷新Token
> 地址：/connect/token
> 请求类型：post

#### 请求示例
Body: Content-Type: application/x-www-form-urlencoded

| 参数名        | 参数值                      | 类型     | 是否必需 | 说明                       |
| ---------- | ------------------------ | ------ | ---- | ------------------------ |
| grant_type | password                 | string | 是    | 密码模式                     |
| client_id  | ABP_Admin_Password       | string | 是    | 标识 是哪个“客户端应用”在请求 Token   |
| username   | admin                    | string | 是    | 登录用户名（ABP 的 Identity 用户） |
| password   | 1q2w3E*                  | string | 是    | 用户密码                     |
| scope      | ABP_Admin offline_access | string | 是    | 声明你这个 Token **想要哪些权限范围** |
|            |                          |        |      |                          |
#### 状态码
>  200
#### 响应示例
```json
{

  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCRTlCN0NBQkZCRTE2NzI5NTZEQzE3MDI3REFENDVCQjg0RjRGOEQiLCJ4NXQiOiJPLW0zeXItLUZuS1ZiY0Z3SjlyVVc3aFBUNDAiLCJ0eXAiOiJhdCtqd3QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MS8iLCJleHAiOjE3Njg0MDE2NjksImlhdCI6MTc2ODM5ODA2OSwiYXVkIjoiQUJQX0FkbWluIiwic2NvcGUiOiJBQlBfQWRtaW4gb2ZmbGluZV9hY2Nlc3MiLCJqdGkiOiJlZjViZTFhOC1hNmIyLTRjNjItODY1Zi01ZTExMmI4ZDIyMWQiLCJzdWIiOiI3Zjg1YWUwYS1mZjVjLTNmNGMtNTU1NS0zYTFlYmY4MTQ4MzUiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJhZG1pbiIsImVtYWlsIjoiYWRtaW5AYWJwLmlvIiwicm9sZSI6ImFkbWluIiwiZ2l2ZW5fbmFtZSI6ImFkbWluIiwicGhvbmVfbnVtYmVyX3ZlcmlmaWVkIjoiRmFsc2UiLCJlbWFpbF92ZXJpZmllZCI6IkZhbHNlIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsIm9pX3Byc3QiOiJBQlBfQWRtaW5fUGFzc3dvcmQiLCJvaV9hdV9pZCI6ImE1NTg2MDEwLTY4MDYtODQ5My1mYTcyLTNhMWVjZWU5ZjZmYSIsImNsaWVudF9pZCI6IkFCUF9BZG1pbl9QYXNzd29yZCIsIm9pX3Rrbl9pZCI6IjEzYTA4OGQzLWE0ZTEtZGFlNi0yZDU0LTNhMWVjZWU5ZjcwZSJ9.Hqv0kmBsgsOAHfamULE2SvzYxy_Yelgfnsnsy1RQo-3CAWefhBd2XF_4JicL1xzI9HoMjDVBEhdSLj66-dOiFgIBq9eH3LEROo44GIdaU8ybykC_qOeB4DaWR-AQqDNixvYcQO0_DaLSdfU-lYga5UXuGxi-2SHdeeTyZOuKoDdM4h2ra3JhGnzASx8WsZgsTNDYEDo0gOhdy6YahZN5w4wq7bNTLNtarCJsKxYELwYH345lM8UJq6ZP07OMqSkQAbm8ry1P-xnm4MznFd1qTvDV_qRHMW4MM8_ghRvLcNdSiaqaQlz8EFmbyUQqS_hpquu75i7vTWr0dKkcp1_X1Q",

  "token_type": "Bearer",

  "expires_in": 3599,

  "refresh_token": "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiI4RjI1MTYwQzQ3M0Y4OEJDRUNBNTRBMTMzQUEzOTBBRkU2MjU1OUQzIiwidHlwIjoib2lfcmVmdCtqd3QiLCJjdHkiOiJKV1QifQ.umq6_kMZWEGRR7oYO1aJwmVKTKVuS4gQRyvPW_yOcP0foNgepQ_Bam3qETq8RYAYNuVVcWCNqyPwtrosI3CuCrf3FQZWRTNDCWp6vAj4KSk7pGPb4qcwNR8_jzitIzkLalNKkHuOTkQtFRnuRJEzqKBYRtXIgV1CLyuLjfRrtwwPb31gNuqZU4DhvSiuKgBQe4YO46RunFb6arUeQmyCVSYAU51aQRHdml5kycOU2UBE5GiAa9t3B2tnMkUTiHwiU7R3yJTS2BTwCg_yrAdo8RqD08Pr4M1e7pBfpYQYbmvooC4JtuAOn_1liRwHDDnxyqxAtX3sAUuhzeZ_tfh5Hg.O-PhiuGTzCt9k7CYuS7PCg.FG-Fa5KaS85nhC2sZNECurvwBGhArW3_rR1DQlc79Qe9A-jl5JaCwa7h80B5HJgovqqFY5i_uZiJSouirNOh07Tfl2lRnw4bn_X3vjgGuQ9pLohlwW9ngacZiwxDy6tH59Mpjmi9WNQWsWDTztKbl6tj4aki_Jl8QUCsu3nmNBlZkDzL8s8rUueMbf5Ib9jEUTRj3S2H-9vLslcgsd5Y1xGZ6IkVY0lXtzMwD78o3-_uf2A1G8NiOoPM7KaXNV9qj6TiVrT2cD03puM-pQ3M198nC16eIpFTB8lN5KAh_dhnqK_BCr82WymsdZKNExL0q6_1w5GK-k_eA47hf8IsTIRrx9zZ3__9XiGOahD4Au1TaEm_dnMuDo9AGHSKXVFjJvNBKv6R-homzoKwo_MJdDRsrkgqrKmmNSFBXduaZFVgD9IBeA9ilesdWFDyoMxVaKWA9M6QZ02U5penmF8qXmp0EJfMs-DD7Q8p118tF8Dmqer-mYZVAORh_hGDlm-6JJar7CT19tsduDwg55MVEP_l4Dq6Pn26fbYR9PIo15f5YVbuhpWSrXcIQM6OEd0QyDxJr8Yh6_S9Wz4h61drfaNQiUv7UbG0S4BtR3BOrQF5BSf6f6NV6CzxmPXVYEyS1ie_Mci32R1h6Qo--M6Qe2T5eIY7Vu2M1cHHBaQHPqF4o0yDqi20aX40AewwAGKpL0BynJKVbs7u0dPZ4cVhvPtHxQe9Rc3aTctkl2fypDGIg1SnovnRHyMiJvmWnqKVA8osfwYEUE-9E5e_6NyRghfRR_vIgVZ5EqisRlF0PezDV_s5UeBdl_6xderfnovnEv3fiNJJrpAqPmALbDnjf2Io1wT6e4mPduyscOz3JlVJz_Qyhxsp13Z7yZh8pRIRC4XMfJBMJFRStiOL30AewjnevrmPbbSv7a4gn5QR5T9lFiYkpfrPr_rSizFfF0tp2p85x_UV5AhH-swuOhnqUZE0yxCoKx38Ct7KbZGpgDQkfrfn3aX5koOgYowQijO8p-JZLQRuXPBXWd4gmPJe7zpKTAgAot55YUCyr3QdDGQlOFvYP4mFLaAh1w_hEoiyePXRjOCw6CDoCIjXST3D0GPkDSLvsx9HvhIPRygpqVzG-u-TfXSlWm5xQjDtQrEWRFPwgyEtij45nBMXJS2-1biQnM0nTCID1hUDjwnkldv2o7_NjRx4ItWEY8xLTawSSP8il2g6MKX6Ht_ujzUvb-xbOIzBum7MzrsyAaAY7zK2rzCgKDbwRjeyvfuOLHKsYxrlW6L8A9w86cHlMgj9UgI7mP-FOSpS5820lRESKa0dIYbWJ1UZF9oejl_ZYDps0nbS01bAFGYyaBfMMJ37uObLB14Bnqim4b_MUzEs1FofMcdI5LJ0Jqwn7505GpWvZLcDqaHX8-DKE3WwiyvQmPpN-mv5A3yP6qweaIWbkpS1e0rMlpdr1qGRShk7dEu4LbyxWYSESS5-cBFax6A-yCunRFisMz3bKkArZl0euUFI49xA5gut1NElodXpkqjCwBsYpMmDZUbmlUv2mUCbOmU39BaCDXzcGDc5OPTYTa0ULrLD7XdCQaC1j5HGIexOoHqOXrKZsadnklHEGnacK5zu24craZ014BiQUx2gejPBw4mnSq6kleZMEu5YRZ8hdc-8YVbI-kkK1KopsXExTG2g_4Ao85zqyQH-uAKtnrlJRg6OqV2bJ0YXzExDhKdQGi1Iw9buN8TZaeKH5KrDG04aVjPQFk9B1jH6juLsn3Z_N6vzwT3TmjVqgrVt-ZwsEX-6xcyZwh09s0jlGrAP6SIl5BIABuZEwsbr6q2cmg0-ReAuumOYFKQpGrdGphHwfSSTBlpLT9EY0V-iRS_CGVqQYsHN5vTj2qBIAF_cxgPmJKgkEjCIrbDEPFXV0pt11k_1o80-11fVgX-UQ_QOuX7HTaJARnGx6tSFC9HiUnvZ7sKXLhLYs0klu7P74MgeylVQ0XTtwp6jv3XXoXV6oDi4YNpUB81RsBSndDpQ82UYGMXJggeLga02gVGSKrwbXw-0cjAgJ4f9_dneMxXBIiSdaAYLNQaDJplTzdZ7Q8goMn_jyw1EokayI3IdYmhw8qjLiilsgsFg9vHEPeZTI6MHsBQx-Xhx4lD06E3V_yvluQnXILjjoP0Rlrqmnrz-oK-OP8TQpn5M8buv7zEVSdl9M8T9jh5y1CY17RltGkNkcrWYWAnUsfNOEXUt_v4PYWbnbEf-qA4I2mMLtlW9nwkWrgLveMWlBY2YlpcSGYsvxg_TZIUX4_-adbkoktap.miBReODn9mh5TDhWZGR3pXLT_DugZspncECJpV6grxk"

}
```

### 3、刷新Token
地址：/connect/token
> 请求类型：post

#### 请求示例
Body: Content-Type: application/x-www-form-urlencoded

| 参数名           | 参数值                | 类型     | 是否必需 | 说明                     |
| ------------- | ------------------ | ------ | ---- | ---------------------- |
| grant_type    | refresh_token      | string | 是    | 密码模式                   |
| client_id     | ABP_Admin_Password | string | 是    | 标识 是哪个“客户端应用”在请求 Token |
| refresh_token | 上一次的refresh_token  | string | 是    |                        |
|               |                    |        |      |                        |
#### 状态码
>  200
#### 响应示例
```json
{

  "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IjNCRTlCN0NBQkZCRTE2NzI5NTZEQzE3MDI3REFENDVCQjg0RjRGOEQiLCJ4NXQiOiJPLW0zeXItLUZuS1ZiY0Z3SjlyVVc3aFBUNDAiLCJ0eXAiOiJhdCtqd3QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo0NDM3MS8iLCJleHAiOjE3Njg0MDE5ODMsImlhdCI6MTc2ODM5ODM4MywiYXVkIjoiQUJQX0FkbWluIiwic2NvcGUiOiJBQlBfQWRtaW4gb2ZmbGluZV9hY2Nlc3MiLCJqdGkiOiI4ZGQ0MWMzMi0wMWU3LTQxN2YtODhhYy1iNzllZTEwYjYxNzIiLCJzdWIiOiI3Zjg1YWUwYS1mZjVjLTNmNGMtNTU1NS0zYTFlYmY4MTQ4MzUiLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwib2lfcHJzdCI6IkFCUF9BZG1pbl9QYXNzd29yZCIsIm9pX2F1X2lkIjoiYTU1ODYwMTAtNjgwNi04NDkzLWZhNzItM2ExZWNlZTlmNmZhIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiYWRtaW4iLCJnaXZlbl9uYW1lIjoiYWRtaW4iLCJyb2xlIjoiYWRtaW4iLCJlbWFpbCI6ImFkbWluQGFicC5pbyIsImVtYWlsX3ZlcmlmaWVkIjoiRmFsc2UiLCJwaG9uZV9udW1iZXJfdmVyaWZpZWQiOiJGYWxzZSIsImNsaWVudF9pZCI6IkFCUF9BZG1pbl9QYXNzd29yZCIsIm9pX3Rrbl9pZCI6IjkxZjMwN2JjLWNhMjUtZWY0Yi03M2RjLTNhMWVjZWVlYzA1NiJ9.l1JOADHOiNo89lDJR9gMTDAYyrDnMeb9EohSzwoBnvT0mbV6325m4xT559UJI5HDP-91JfTA5nb_96w1mbd48ozYHiGs-IRpEmXtUV_Iqnwf_7epl2fU6lE8bWIY4Q3bPEBVTRbc4XlAYdNFl48wNcuUsRl8BNcKXM7VJRHC-MygQWSyR24HWNRdFre-u6063wWPwoo7alPMf74hvRy9i0JomlLleWNQ8DSGPNuMyTFBt00yGA2fWBu1opA6JnrF3PReaMMFZ0VOh_9RdurJ5xDhuxxD2nEOtwmknH7cFwM5v8YIwAoL5c8sEL-Ty-KS8ayl8_Kd8ugMyeKm3gCEXA",

  "token_type": "Bearer",

  "expires_in": 3600,

  "scope": "ABP_Admin offline_access",

  "refresh_token": "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiI4RjI1MTYwQzQ3M0Y4OEJDRUNBNTRBMTMzQUEzOTBBRkU2MjU1OUQzIiwidHlwIjoib2lfcmVmdCtqd3QiLCJjdHkiOiJKV1QifQ.j-ifxr240z0F_VxsOCyiPPNyYiBSIJVlaybzCEW6ZFvpHGTVhvac5-RF5O6fRp9lNAM0PQ-dcZW5GJX2KnyuYBxNc1pSzr40fY2k04H9H5AQW8zE-71Wa77gy0ViAAeiFyL2n6MLpvPxOu2apn7bZFUS3R23R2LQA5Qz4v9b-3gS3zzT3KDcFKTWHbpjJVuRSrkjjv0vQEGYdEPtnpj2uK_0q1wzij9GM3P1_sdAGzXsAiTlIuzKJyg4KhQYQebJhNbxvgbbiSizMkWnER2ki2Z5xtIFORNqb9VSPq84MOT1iOta8gWbrQqjmq5FehinWOHSE4yC7fGnl_DaJ2GopA.fi71gi8kZ1knDaSY5bNkoQ.Jtjm8FN4NbxaZZNSn-GH_GOQO8qJh19lK6OSfw77W2sQR1yFvWhQcBDPc5LhCTwnLF2vATGV_gDp7Zp_a7PIla0zGaIXsjXv3SiL0xJSsZJstBlQsQ9lm25h0QVqZkaK1bKncCcPc_ILsuuRk0orGE_bLHXHeF5NNDOgVwC5e8V2A9mn6io0O6V7UyeIKKo0w2ikUTJkCgjnLyp5FRqwTgPnCBVyLvk-v7RFvFkl983R6FKLtomXA2py5v_8a6wdsUBb7beMCuB-_qR5iuVCUDvzsoI_itoYsO9bQsD4DpXz5odj1giORcpVEIbMfAcb7qoEyHW9M-WCNA9QMQzAmrTzSGZT2DHJKols_AAyL59HXcwgDiJyL8ZHixhvt5VE5paaGpHlnSRaoHqoS4PdGbiEv_-yhVPzxdaeTvXi1OuPIpzH7pc-9QqUNWTNYjQTb4xtRnrcC90pt0HMRiN5-r1F2C8yrECMXlt12LHq93Pod-0Zh07LqaIlo1j6LFjJg7_r9O-1Qc_T4f8YjdRGDsYZTjAosYnbIdM8AEqmXp0uZ-KleQtSKBawC-ytOkASC9gc-13ebNVwghcQquIzPXeKckH_H4fK1Mo3whZ4JplTJjJYga0D-UjM5oKrBOyk0htlERG8LUqqyot2zwF5jk7MgrOBdDLyeSiX9aBkrkw52tzLRPJW-6fUGRV3G4F2XhUN__WHy79aZK-xp0sQRdhnw7rX4LpZBLtMf664fyEo56QSh_GbfQv8Fl5zsaROCr14iHb9Ne6wQ1iWQ6MxOH8dxQLg0pLUWUuuUaI-Act-wm0lAQbWLegouVO2LMFhxbxCAhqFtPwpeeIvGOUVPZOINXlkYVuDOg7DedKJ53nmxQrdEy6vk-_QH7chbpBtOrVlPuLPDLL0EXsmyWfdFDTwwDSfqqjX8n9ZuE0bP2qeZor34Es9fck_jCdSeG6ZcBVkQ8sRw7yBxoxCgLMAndEbdj4PPBnmtAt1TUmaEPIIfMTBrHsEJaUkguz7nSw6UfhB37pukbKlj9uCulMO7B3AaxuPMTD71ckj-h7h4XfPHNdIJP8v7A7ZjmNcKH4ntcvFQB55CQkkw4T2iVtjNefPzbwVbTMBI6RmAxNNe0DgerXO26st-Hl-Gd88sskUWRFxGlE_Jl4MfsgrxSvss-68AhWvBqURJnbc5X7lp43zQL5QVVXEa-VrFQY-74PLhTFWxvESvTk5iZzRl8aFaccfaGlIhSJCh98qQZchrAHA6bG9ErAfS_UyYhkJQeVXEzXWoewpTlKUXy_SccO1xPGUgNXMYmSXKr8f2BPMpWF4mdzfLYjImAxMwv3oMUIIe0FRNnBTXHDGYhs8b7TdX9KcGRnv0WNLDtdZ1-n7qoo9v-1hkW9QTSDXjjDCkNVMnMQk7AyvedvRHifaqEOs_j_F1RMDgeHTIL6dOu15linbUh1vVAMDspac5VZ5fjXVX2kP68GBVXpgHxNy4inmH8n155tirzKj8IsVAdUVKTcS20ijEFE0Etn06ti8WynudB3gVldqIJIvS6KvddQKk9QlCnXCLfhXd3_BrU7fsXC0rs3oEMTMZlOCQa905j0-PzNURB4TWdvYuYfYjFPl4m8qUWcsQ20y8q0sSzXCN9SzAoSmJz0GWmWac1ChOiM3Ff-d54Ly1okoOXrYLnqTKXCX3QM4rMCNNWmG-YuWdj74tY0Yuyp7DC-Mou8iDx6I7y5vuhw1s1qxBe6avG10jDqkyZIvjjuGJOXklWL5DwnDqd0mxqty1CWCxeQn-uHrA34sp3xmesgKEXkx4HLdgKaYjfZE_jnNv7Qn9QZOa9fQIUdTOTOhylQUzsQTP48uf0u_k36rJfxpygJAp6gzGAYQv-225Wr8JarjUmfhXKOLIzRIhzM9Z8qw2DhXiJ5mxPvGSuKkq-64zjmyutgMTavqBK-S3tabLUCxF3Pdr7EMVwd8MOgxZ-IoFAvEZNuBHhqcZti7d7UoRC28HvNa1gVmCS_89YSyUsDjz1MzIdnY1O3B-cqAQp-xoPDXd3EZSWIYQDLSzD52YSViiCk2f9gvcTkgODIYTXT5fSQrEQnfU_SrDl0jyOO27aotueHyFnA3mRRrkSdkvnU9ciy50k1F3Mq8CfTIxftrtCp3fQnlhdkMV-o7ZnRo5b3HkYXTL1ndXvFRDUEOBq2jKRadf1PlIjuvJR7CZma7hPHireKa7wbl2lcGAiUt212XTfiZPDRxC_oXhUKXi0k1Brd_ir0OJC-hZIJ2cz_oOEfG-VLKvskmikqcOkEmYHbUPV0DQcnOHPDtPI4stFtK7aWCh9bMEbNKOX2F73l6klOK3q3F_4NrjmC3NB7hxR2VkQ8IbFaT5sLKU0X34jDo4n6DDxrHPqjpstqyk9tU0bTVijG4NgH0FQzB_SGwy8o8NyHd-PSkz0xIzSK1bW_u__TlUFh468szptqxjuiBmKaVq7uYu_c14eWlzk8KEGif77iD.Mfc38ZhHtPuCyscWG8rTA8O3mQi9iwXcO-iOu7ycSMc"

}
```

### 4、登录
> 地址：/api/account/login
> 请求类型：post

#### 请求示例
Body: Content-Type: application/json

```json
{

  "userNameOrEmailAddress": "admin",

  "password": "1q2w3E*",

  "rememberMe": true

}
```

#### 状态码
>  200
#### 相应示例
```json
{

    "result": 1,

    "description": "Success"

}
```

### 5、登出
> 地址：/api/account/logout
> 请求类型：get

#### 请求示例
null

#### 状态码
> 204
#### 响应示例
null

### 6、检查密码
> 地址：/api/account/check-password
> 请求类型：post

#### 请求示例：
Body: Content-Type: application/json

```json
{

  "userNameOrEmailAddress": "admin",

  "password": "1q2w3E*",

  "rememberMe": true

}
```

#### 状态码
> 200

#### 响应示例
```json
{

    "result": 1,

    "description": "Success"

}
```

[^1]: 

## 权限管理模块
### 1、 根据角色/用户获取权限
> 地址：/api/permission-management/permissions
> 请求类型：get

#### 请求示例
Params:

| 参数名          | 值     | 类型     | 备注       |
| ------------ | ----- | ------ | -------- |
| providerName | R     | string | 根据角色还是用户 |
| providerKey  | admin | string | 角色名/用户名  |
#### 状态码
> 200

#### 响应示例
```json
{

    "entityDisplayName": "admin",

    "groups": [

        {

            "name": "AbpIdentity",

            "displayName": "Identity management",

            "displayNameKey": "Permission:IdentityManagement",

            "displayNameResource": "AbpIdentity",

            "permissions": [

                {

                    "name": "AbpIdentity.Roles",

                    "displayName": "Role management",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Roles.Create",

                    "displayName": "Create",

                    "parentName": "AbpIdentity.Roles",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Roles.Update",

                    "displayName": "Edit",

                    "parentName": "AbpIdentity.Roles",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Roles.Delete",

                    "displayName": "Delete",

                    "parentName": "AbpIdentity.Roles",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Roles.ManagePermissions",

                    "displayName": "Change permissions",

                    "parentName": "AbpIdentity.Roles",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users",

                    "displayName": "User management",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users.Create",

                    "displayName": "Create",

                    "parentName": "AbpIdentity.Users",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users.Update",

                    "displayName": "Edit",

                    "parentName": "AbpIdentity.Users",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users.Update.ManageRoles",

                    "displayName": "Manage roles",

                    "parentName": "AbpIdentity.Users.Update",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users.Delete",

                    "displayName": "Delete",

                    "parentName": "AbpIdentity.Users",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpIdentity.Users.ManagePermissions",

                    "displayName": "Change permissions",

                    "parentName": "AbpIdentity.Users",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                }

            ]

        },

        {

            "name": "FeatureManagement",

            "displayName": "Feature management",

            "displayNameKey": "Permission:FeatureManagement",

            "displayNameResource": "AbpFeatureManagement",

            "permissions": [

                {

                    "name": "FeatureManagement.ManageHostFeatures",

                    "displayName": "Manage host features",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                }

            ]

        },

        {

            "name": "SettingManagement",

            "displayName": "Setting management",

            "displayNameKey": "Permission:SettingManagement",

            "displayNameResource": "AbpSettingManagement",

            "permissions": [

                {

                    "name": "SettingManagement.Emailing",

                    "displayName": "Emailing",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "SettingManagement.Emailing.Test",

                    "displayName": "Emailing test",

                    "parentName": "SettingManagement.Emailing",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "SettingManagement.TimeZone",

                    "displayName": "Time zone",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                }

            ]

        },

        {

            "name": "AbpTenantManagement",

            "displayName": "Tenant management",

            "displayNameKey": "Permission:TenantManagement",

            "displayNameResource": "AbpTenantManagement",

            "permissions": [

                {

                    "name": "AbpTenantManagement.Tenants",

                    "displayName": "Tenant management",

                    "parentName": null,

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpTenantManagement.Tenants.Create",

                    "displayName": "Create",

                    "parentName": "AbpTenantManagement.Tenants",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpTenantManagement.Tenants.Update",

                    "displayName": "Edit",

                    "parentName": "AbpTenantManagement.Tenants",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpTenantManagement.Tenants.Delete",

                    "displayName": "Delete",

                    "parentName": "AbpTenantManagement.Tenants",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpTenantManagement.Tenants.ManageFeatures",

                    "displayName": "Manage features",

                    "parentName": "AbpTenantManagement.Tenants",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                },

                {

                    "name": "AbpTenantManagement.Tenants.ManageConnectionStrings",

                    "displayName": "Manage connection strings",

                    "parentName": "AbpTenantManagement.Tenants",

                    "isGranted": true,

                    "allowedProviders": [],

                    "grantedProviders": [

                        {

                            "providerName": "R",

                            "providerKey": "admin"

                        }

                    ]

                }

            ]

        }

    ]

}
```

### 2、更新用户/角色权限
#### 请求示例
>  地址：/api/permission-management/permissions
>  put 请求

Params:

| 参数名          |     | 参数值   | 类型     | 备注      |
| ------------ | --- | ----- | ------ | ------- |
| providerName |     | R     | string | 角色/用户   |
| providerKey  |     | admin | string | 角色名/用户名 |
body
```json
{

  "permissions": [

    {

      "name": "AbpIdentity.Users.Create",

      "isGranted": true

    }

  ]

}
```

####  响应码
> 204

#### 响应示例
null

## 角色管理模块
### 1、获取所有角色
> 地址：/api/identity/roles/all
> 请求类型：get

#### 请求示例
null
#### 响应码
> 200

#### 响应示例
```json
{

    "items": [

        {

            "name": "admin",

            "isDefault": false,

            "isStatic": true,

            "isPublic": true,

            "concurrencyStamp": "7480098245c94df3838a37053ca19cd2",

            "id": "65fd416c-320a-a079-9bfd-3a1ebf8149e3",

            "extraProperties": {}

        }

    ]

}
```

### 2、条件查询获取角色
> 地址：/api/identity/roles
> 请求方式：get
#### 请求示例

| 参数              | 值   |
| --------------- | --- |
| additionalProp1 |     |
| additionalProp2 |     |
| additionalProp3 |     |

#### 响应码
> 200

#### 响应示例
```json
{

    "totalCount": 1,

    "items": [

        {

            "name": "admin",

            "isDefault": false,

            "isStatic": true,

            "isPublic": true,

            "concurrencyStamp": "7480098245c94df3838a37053ca19cd2",

            "id": "65fd416c-320a-a079-9bfd-3a1ebf8149e3",

            "extraProperties": {}

        }

    ]

}
```

### 3、添加角色
> 地址：/api/identity/roles
> 请求方式：post

#### 请求示例
```json
{

  "name": "Vister",

  "isDefault": true,

  "isPublic": true

}
```
#### 响应码
> 200

#### 响应示例
```json
{

    "name": "Vister",

    "isDefault": true,

    "isStatic": false,

    "isPublic": true,

    "concurrencyStamp": "993c83abf4f346faa8219f3c963f0bac",

    "id": "8513c362-abf3-0ab4-c91f-3a1eee3246ae",

    "extraProperties": {}

}
```

### 4、根据角色id查询角色信息
> 地址：/api/identity/roles/{id}
> 请求类型：get

#### 请求示例
null
#### 响应码
> 200

#### 响应示例
```json
{

    "name": "admin",

    "isDefault": false,

    "isStatic": true,

    "isPublic": true,

    "concurrencyStamp": "7480098245c94df3838a37053ca19cd2",

    "id": "65fd416c-320a-a079-9bfd-3a1ebf8149e3",

    "extraProperties": {}

}
```

### 5、修改指定id的角色
> 地址：/api/identity/roles/{id}
> 请求方式：put

#### 请求示例
```json
{

  "name": "User",

  "isDefault": true,

  "isPublic": true,

  "concurrencyStamp": "993c83abf4f346faa8219f3c963f0bac"

}
```

#### 响应码
> 200

#### 响应示例
```json
{
  "name": "User",
  "isDefault": true,
  "isStatic": false,
  "isPublic": true,
  "concurrencyStamp": "052ebb994d4a4c8bb1e861f3197bf085",
  "id": "8513c362-abf3-0ab4-c91f-3a1eee3246ae",
  "extraProperties": {}
}
```

### 6、删除指定ID的角色
> 地址：/api/identity/roles/{id}
> 请求类型：delete

#### 请求示例
null
#### 响应码
> 204

#### 响应示例
null

## 用户管理模块
### 1、根据用户id获取用户信息
> 地址：/api/identity/users/{id}
> 请求类型： get

#### 请求示例
null
#### 响应码
> 200

#### 响应示例
```json
{

    "tenantId": null,

    "userName": "admin",

    "name": "admin",

    "surname": null,

    "email": "admin@abp.io",

    "emailConfirmed": false,

    "phoneNumber": null,

    "phoneNumberConfirmed": false,

    "isActive": true,

    "lockoutEnabled": true,

    "accessFailedCount": 0,

    "lockoutEnd": null,

    "concurrencyStamp": "79a6a0901d474555a2b2a4b3d9627dc8",

    "entityVersion": 2,

    "lastPasswordChangeTime": "2026-01-11T13:52:30.9291709+00:00",

    "isDeleted": false,

    "deleterId": null,

    "deletionTime": null,

    "lastModificationTime": "2026-01-11T21:52:31.4039186",

    "lastModifierId": null,

    "creationTime": "2026-01-11T21:52:31.0299936",

    "creatorId": null,

    "id": "7f85ae0a-ff5c-3f4c-5555-3a1ebf814835",

    "extraProperties": {}

}
```

### 2、修改指定id的用户
### 3、删除指定ID的用户
### 4、筛选获取指定条件的用户
### 5、新增用户
> 地址 ：/api/identity/users
> 请求方式: post

#### 请求示例
```json
{

  "userName": "test",

  "name": "test",

  "surname": "t",

  "email": "user@example.com",

  "phoneNumber": "string",

  "isActive": true,

  "lockoutEnabled": true,

  "roleNames": [

    "admin"

  ],

  "password": "1q2w3E*"

}
```
#### 响应体
> 200
#### 响应示例
```json
{

    "tenantId": null,

    "userName": "test",

    "name": "test",

    "surname": "t",

    "email": "user@example.com",

    "emailConfirmed": false,

    "phoneNumber": "string",

    "phoneNumberConfirmed": false,

    "isActive": true,

    "lockoutEnabled": true,

    "accessFailedCount": 0,

    "lockoutEnd": null,

    "concurrencyStamp": "9afe2d6cae674a689d2bebf9d57fd1d6",

    "entityVersion": 4,

    "lastPasswordChangeTime": "2026-01-20T16:12:19.4988448+00:00",

    "isDeleted": false,

    "deleterId": null,

    "deletionTime": null,

    "lastModificationTime": "2026-01-21T00:12:19.8004103+08:00",

    "lastModifierId": "7f85ae0a-ff5c-3f4c-5555-3a1ebf814835",

    "creationTime": "2026-01-21T00:12:19.51983+08:00",

    "creatorId": "7f85ae0a-ff5c-3f4c-5555-3a1ebf814835",

    "id": "8322a2c2-ca9e-d888-1d57-3a1eee5a844b",

    "extraProperties": {}

}
```