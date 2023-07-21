using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TangTang.Demo.WebAPI.Utility.Swagger
{
    /// <summary>
    /// Swagger扩展--自定义版本信息
    /// </summary>
    public static class SwaggerExtensionV1
    {
        /// <summary>
        /// 扩展Swagger完整配置
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSwaggerGenExtV1(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(option =>
            {
                #region 自定义分版本的Swagger配置
                {
                    //要启用swagger版本管理要在Api控制器或者方法上添加特性[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))] 在这用枚举
                    typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                    {
                        option.SwaggerDoc(version, new OpenApiInfo()
                        {
                            Title = $"默默学习-版本:{version}Api文档",
                            Version = version,
                            Description = $"通用版本的CoreApi版本{version}"
                        });
                    });
                }
                #endregion

                #region 配置展示注释
                {
                    //xml文档绝对路径
                    var file = Path.Combine(AppContext.BaseDirectory, "TangTang.Demo.WebAPI.xml");
                    //true 显示控制器层注释
                    option.IncludeXmlComments(file, true);
                    //对action的名称进行排序，如果有多个，就可以看到效果了
                    option.OrderActionsBy(o => o.RelativePath);
                }
                #endregion

                #region 扩展安全定义
                {
                    //添加安全定义
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "请输入token，格式为Bearef xxxxxxxx(注意中间必须有空格)",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    //添加安全要求
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference=new OpenApiReference()
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{ }
                            }
                    });
                }
                #endregion

                #region 扩展文件上传按钮
                {
                    option.OperationFilter<FileUploadFilterV1>();
                }
                #endregion
            });
        }

        /// <summary>
        /// swagger中间件配置应用
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtV1(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                #region 自定义分版本控制
                {
                    foreach (string version in typeof(ApiVersions).GetEnumNames())
                    {
                        option.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"版本：{version}");
                    }
                }
                #endregion
            });
        }
    }

    /// <summary>
    /// 版本枚举
    /// </summary>
    public enum ApiVersions
    {
        /// <summary>
        /// 第一版本
        /// </summary>
        V1,
        /// <summary>
        /// 第二版本
        /// </summary>
        V2,
        /// <summary>
        /// 第三版本
        /// </summary>
        V3
    }

    /// <summary>
    /// 扩展文件上传展示选择文件按钮
    /// </summary>
    public class FileUploadFilterV1 : IOperationFilter
    {
        /// <summary>
        /// 扩展文件上传展示选择文件按钮
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            const string FileUploadContentType = "multipart/form-data";
            if (operation.RequestBody == null ||
                !operation.RequestBody.Content.Any(x =>
                x.Key.Equals(FileUploadContentType, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            //限制提交类型
            if (context.ApiDescription.ParameterDescriptions[0].Type == typeof(IFormCollection))
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Description = "文件上传",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            FileUploadContentType,new OpenApiMediaType
                            {
                                Schema=new OpenApiSchema
                                {
                                    Type="object",
                                    Required = new HashSet<string>{ "file"},
                                    Properties =new Dictionary<string, OpenApiSchema>
                                    {
                                        {
                                            "file",new OpenApiSchema()
                                            {
                                                Type="string",
                                                Format="binary"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
