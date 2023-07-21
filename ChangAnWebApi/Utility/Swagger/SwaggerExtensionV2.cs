using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TangTang.Demo.WebAPI.Utility.Swagger
{
    /// <summary>
    /// Swagger扩展--框架支持版本信息
    /// </summary>
    public static class SwaggerExtensionV2
    {
        /// <summary>
        /// 扩展Swagger完整配置
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSwaggerGenExtV2(this WebApplicationBuilder builder)
        {
            #region 添加API版本支持
            {
                //添加API版本支持
                builder.Services.AddApiVersioning(o =>
                {
                    //是否在响应的header信息中返回API版本信息
                    o.ReportApiVersions = true;
                    //默认的API版本
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    //未指定API版本时，设置API版本为默认的版本
                    o.AssumeDefaultVersionWhenUnspecified = true;
                });

                //配置API版本信息
                builder.Services.AddVersionedApiExplorer(option =>
                {
                    //Api版本信息
                    option.GroupNameFormat = "'v'VVVV";
                    //未指定API版本时，设置API版本为默认版本
                    option.AssumeDefaultVersionWhenUnspecified = true;
                });
            }
            #endregion

            builder.Services.AddEndpointsApiExplorer();
            _ = builder.Services.AddSwaggerGen(option =>
            {
                #region 组件支持版本展示
                {
                    //根据API版本信息生成API文档
                    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var item in provider.ApiVersionDescriptions)
                    {
                        option.SwaggerDoc(item.GroupName, new OpenApiInfo
                        {
                            Contact = new OpenApiContact
                            {
                                Name = "堂堂",
                                Email = "15550"
                            },
                            Description = "学习SwaggerAPI管理",
                            Title = "WebAPI文档",
                            Version = item.ApiVersion.ToString()
                        });
                    }
                    //在Swagger 文档显示的API地址中将版本信息参数替换成实际的版本号
                    option.DocInclusionPredicate((version, apiDescription) =>
                    {
                        if (!version.Equals(apiDescription.GroupName))
                            return false;
                        IEnumerable<string> values = apiDescription!.RelativePath.Split('/')
                        .Select(v => v.Replace("-----v{version}", apiDescription.GroupName));
                        apiDescription.RelativePath = string.Join("/", values);
                        return true;
                    });
                    //参数使用驼峰命名方式
                    option.DescribeAllParametersInCamelCase();
                    //取消API文档需要输入版本信息
                    option.OperationFilter<RemoveVersionFromParameter>();
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
                    option.OperationFilter<FileUploadFilterV2>();
                }
                #endregion
            });
        }

        /// <summary>
        /// swagger中间件配置应用
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerExtV2(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                #region 调用第三方程序包支持的版本控制
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    //默认加载最新版本的API文档
                    foreach (var item in provider.ApiVersionDescriptions.Reverse())
                    {
                        option.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", $"正式版本{item.GroupName.ToUpperInvariant()}");
                    }
                }
                #endregion
            });
        }
    }

    /// <summary>
    /// 删除Swagger版本输入
    /// </summary>
    public class RemoveVersionFromParameter : IOperationFilter
    {
        /// <summary>
        /// 扩展
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if (versionParameter != null)
            {
                //删除暂时有问题
                //operation.Parameters.Remove(versionParameter);
            }
        }
    }

    /// <summary>
    /// 扩展文件上传展示选择文件按钮
    /// </summary>
    public class FileUploadFilterV2 : IOperationFilter
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
