using Microsoft.AspNetCore.Mvc;

namespace SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get(
            [FromServices]IPaymentService paymentService)
        {
            return paymentService.GetMessage();
        }
    }
}