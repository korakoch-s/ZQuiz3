using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZQuiz.BusinessServices;

namespace ZQuiz.WebApi.Controllers
{
    /// <summary>
    /// Load existed tester information
    /// </summary>
    public class LoadController : ApiController
    {
        private readonly IZQuizService _services;

        public LoadController(IZQuizService service)
        {
            _services = service;
        }

        /// <summary>
        /// Load existed tester information
        /// </summary>
        /// <param name="name">Tester name</param>
        /// <returns></returns>
        // GET: api/register
        public IHttpActionResult Get(string name)
        {
            var tester = this._services.LoadTesterByName(name);
            if (tester != null)
            {
                return Ok(tester);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
