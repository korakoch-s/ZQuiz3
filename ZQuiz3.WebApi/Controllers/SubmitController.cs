using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZQuiz.BusinessEntities;
using ZQuiz.BusinessServices;

namespace ZQuiz.WebApi.Controllers
{
    /// <summary>
    /// Submit test to see result
    /// </summary>
    public class SubmitController : ApiController
    {
        private readonly IZQuizService _services;

        public SubmitController(IZQuizService service)
        {
            _services = service;
        }

        /// <summary>
        /// Submit current tester data
        /// </summary>
        // POST: api/submit
        public IHttpActionResult Post([FromBody]TesterEntity tester)
        {
            var retTester = this._services.SubmitTest(tester);
            if (retTester != null)
            {
                return Ok(retTester);
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
