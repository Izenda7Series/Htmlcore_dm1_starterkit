using Microsoft.AspNetCore.Mvc;

namespace ApiCoreStarterKit.Controllers
{
    public class BaseController : Controller
    {
        #region Methods
        protected JsonResult AddJsonResult(bool success)
        {
            var resultMessage = success ? "Success" : "Failure";

            return Json(new { resultMessage });
        } 
        #endregion
    }
}
