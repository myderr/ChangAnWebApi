using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using TangTang.Demo.WebAPI.Utility.Swagger;

namespace TangTang.Demo.WebAPI.Utility.CrossDomain
{
    /// <summary>
    /// 请求跨域扩展
    /// </summary>
    public static class CrossDomainExtension
    {
        /// <summary>
        /// 扩展跨域请求
        /// </summary>
        /// <param name="builder"></param>
        public static void AddCrossDomainExt(this WebApplicationBuilder builder)
        {
            //配置跨域
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "myCors", builde =>
                {
                    builde.WithOrigins("*", "*", "*")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        /// <summary>
        /// 跨域中间件配置应用
        /// </summary>
        /// <param name="app"></param>
        public static void UseCrossDomainExt(this WebApplication app)
        {
            //加上跨域配置
            app.UseCors("myCors");
        }
    }
}
