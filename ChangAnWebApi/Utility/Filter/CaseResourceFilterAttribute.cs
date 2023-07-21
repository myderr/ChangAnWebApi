using Microsoft.AspNetCore.Mvc.Filters;

namespace TangTang.Demo.WebAPI.Utility.Filter
{
    /// <summary>
    /// 自定义ResourceFilter
    /// </summary>
    public class CaseResourceFilterAttribute : Attribute, IResourceFilter
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static Dictionary<string, object> CaseResourceData = new Dictionary<string, object>();
        /// <summary>
        /// 在Case资源之后
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("CaseResourceFilterAttribute.OnResourceExecuted");
        }

        /// <summary>
        /// 在Case资源之前
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("CaseResourceFilterAttribute.OnResourceExecuting");
        }
    }
}
