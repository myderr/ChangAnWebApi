using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace TangTang.Demo.WebAPI.Utility.Route
{
    /// <summary>
    /// 全局路由前缀配置
    /// </summary>
    public class RouteConvention : IApplicationModelConvention
    {

        /// <summary>
        /// 路由前缀变量
        /// </summary>
        private readonly AttributeRouteModel attributeRouteModel;

        /// <summary>
        /// 调用时传入指定的路由前缀
        /// </summary>
        /// <param name="routeTemplateProvider"></param>
        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            attributeRouteModel = new AttributeRouteModel(routeTemplateProvider);
        }

        /// <summary>
        /// 接口的Apply方法，根据情况添加API路由前缀
        /// </summary>
        /// <param name="application"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(ApplicationModel application)
        {
            //遍历所有的Controller
            foreach (var controller in application.Controllers)
            {
                if (controller.ControllerName == "File") continue;
                //1.已经标记了RouteAttribute 的 Controller 如果控制器已经注入了路由，则会在路由前面添加指定的路由内容
                var matchedSelectors = controller.Selectors.Where(x =>
                x.AttributeRouteModel != null).ToList();
                if (matchedSelectors.Any())
                {
                    foreach (var selectprModel in matchedSelectors)
                    {
                        //在当前路由上 在添加一个路由前缀
                        selectprModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(attributeRouteModel, selectprModel.AttributeRouteModel);
                    }
                }
                //2.没有标记RouteAttribute 的 Controller
                var unmatchedSelectors = controller.Selectors.Where(x =>
                x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectprModel in unmatchedSelectors)
                    {
                        //在当前路由上 在添加一个路由前缀
                        selectprModel.AttributeRouteModel = attributeRouteModel;
                    }
                }
            }
        }
    }
}
