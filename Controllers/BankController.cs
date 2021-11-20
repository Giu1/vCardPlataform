using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace vCardPlatform.Controllers
{
    [RoutePrefix("api/bank")]
    public class BankController : ApiController
    {

        private string bankId = "Bank_1";
        // GET: Bank

        [Route("")]
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok(bankId);
        }
    }
}