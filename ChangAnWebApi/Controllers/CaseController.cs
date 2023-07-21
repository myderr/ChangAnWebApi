using Microsoft.AspNetCore.Mvc;
using TangTang.Demo.WebAPI.Utility.Filter;
using TangTang.Demo.WebAPI.Utility.Swagger;

namespace ChangAnWebApi.Controllers
{
    /// <summary>
    /// 案例控制器
    /// </summary>
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V1))]//自定义Swagger版本声明版本
    [ApiVersion("1.0")]//框架支持Swagger版本声明版本
    //{version:apiVersion}
    [Route("[controller]/{version:apiVersion}")]
    public class CaseController : ControllerBase
    {
        private readonly ILogger<CaseController> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public CaseController(ILogger<CaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [CaseAsyncResourceFilter]
        public object GetUser()
        {
            _logger.LogError("我是错误");
            _logger.LogInformation("我是明细");
            return Ok();
        }

        /// <summary>
        /// 根据Id获取信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("{Id:int}")]
        public object GetObjectById(int Id)
        {
            return Ok();
        }

        /// <summary>
        /// 新增信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost()]
        public int AddObject(object obj)
        {
            return 1;
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPut()]
        public int UpDateObject(object obj)
        {
            return 1;
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete()]
        public int DeleteObject(int Id)
        {
            return 1;
        }
    }
}
