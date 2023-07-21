using Microsoft.AspNetCore.Mvc.Filters;

namespace TangTang.Demo.WebAPI.Utility.Filter
{
    /// <summary>
    /// 自定义的异步ResourceFilter
    /// </summary>
    public class CaseAsyncResourceFilterAttribute : Attribute, IAsyncResourceFilter
    {
        /// <summary>
        /// 请求方法前后
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            //执行Action
            await next.Invoke();
        }
    }
}
