using Microsoft.AspNetCore.Mvc;
using TangTang.Demo.WebAPI.Utility.Swagger;

namespace ChangAnWebApi.Controllers
{
    /// <summary>
    /// 文件资源
    /// </summary>
    [ApiVersion("1.0")]
    //{version:apiVersion}
    //[ApiExplorerSettings(IgnoreApi = false, GroupName = nameof(ApiVersions.V3))]//老版
    [Route("[controller]/{version:apiVersion}")]
    public class CaseFileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public CaseFileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult UploadFile(IFormCollection form)
        {
            return new JsonResult(new
            {
                Success = true,
                Message = "上传成功",
                FileName = form.Files.FirstOrDefault()?.FileName
            });
        }
    }
}
