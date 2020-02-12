using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult ReportPart()
        {
            return File("~/page_to_render_exports.html", "text/html");
        }
    }
}