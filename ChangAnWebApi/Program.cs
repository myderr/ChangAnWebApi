using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using TangTang.Demo.WebAPI.Utility.CrossDomain;
using TangTang.Demo.WebAPI.Utility.Route;
using TangTang.Demo.WebAPI.Utility.Swagger;

//https://39.406213216.xyz/swagger/index.html

var builder = WebApplication.CreateBuilder(args);
#if DEBUG
//builder.WebHost.ConfigureKestrel((options =>
//{
//    options.Listen(IPAddress.Any, 443, listenOptions =>
//    {
//        listenOptions.UseHttps(@"C:\Users\40621\Desktop\车机\服务\ChangAnWebApi\ChangAnWebApi\证书\39.406213216.xyz.pfx",
//            "stc053802e7");
//    });
//}));
#endif
//配置log4net
//builder.Logging.AddLog4Net("CfgFile/log4net1.config");

//缓存
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services
    .AddControllers(option =>
    {
        //统一路由前缀
        option.Conventions.Insert(0, new RouteConvention(new RouteAttribute("api/")));
    })
    .AddJsonOptions(option =>
    {
        //在这里解决中文乱码问题
        option.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

builder.Services.AddMvc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//扩展方法 配置swagger
builder.AddSwaggerGenExtV2();
//扩展方法 跨域请求
builder.AddCrossDomainExt();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //扩展方法 swagger 引用中间件
    app.UseSwaggerExtV2();
}

//扩展方法 跨域中间件
app.UseCrossDomainExt();

//app.UseHttpsRedirection();

// 启用静态文件服务
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
