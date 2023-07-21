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
//        listenOptions.UseHttps(@"C:\Users\40621\Desktop\����\����\ChangAnWebApi\ChangAnWebApi\֤��\39.406213216.xyz.pfx",
//            "stc053802e7");
//    });
//}));
#endif
//����log4net
//builder.Logging.AddLog4Net("CfgFile/log4net1.config");

//����
builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services
    .AddControllers(option =>
    {
        //ͳһ·��ǰ׺
        option.Conventions.Insert(0, new RouteConvention(new RouteAttribute("api/")));
    })
    .AddJsonOptions(option =>
    {
        //��������������������
        option.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

builder.Services.AddMvc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//��չ���� ����swagger
builder.AddSwaggerGenExtV2();
//��չ���� ��������
builder.AddCrossDomainExt();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //��չ���� swagger �����м��
    app.UseSwaggerExtV2();
}

//��չ���� �����м��
app.UseCrossDomainExt();

//app.UseHttpsRedirection();

// ���þ�̬�ļ�����
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
