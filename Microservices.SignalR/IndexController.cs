using Microsoft.AspNetCore.Mvc;

namespace Microservices.SignalR.Controllers
{
    public class IndexController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Redirect("index.html");
        }
    }
}
