using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.Controllers
{
    public class HomeController : Controller
    {
        #region Methods
        public IActionResult ReportPart()
        {
            return File("~/page_to_render_exports.html", "text/html");
        } 
        #endregion
    }
}