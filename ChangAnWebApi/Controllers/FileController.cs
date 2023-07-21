using Microsoft.AspNetCore.Mvc;

namespace ChangAnWebApi.Controllers
{

    [ControllerName("File")]
    [Route("[controller]/[action]")]
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
