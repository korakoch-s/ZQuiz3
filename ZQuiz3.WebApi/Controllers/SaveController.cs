﻿using System;
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
    /// Save test session information
    /// </summary>
    public class SaveController : ApiController
    {
        private readonly IZQuizService _services;

        public SaveController(IZQuizService service)
        {
            _services = service;
        }

        /// <summary>
        /// Save current tester data
        /// </summary>
        // POST: api/save
        public IHttpActionResult Post([FromBody]TesterEntity tester)
        {
            var retTester = this._services.SaveTest(tester);
            if (retTester != null)
            {
                return Ok(retTester);
            } else
            {
                return InternalServerError();
            }
        }
    }
}
