﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZQuiz.BusinessServices;

namespace ZQuiz.WebApi.Controllers
{
    /// <summary>
    /// Register new tester session
    /// </summary>
    public class RegisterController : ApiController
    {
        private readonly IZQuizService _services;

        public RegisterController(IZQuizService service)
        {
            _services = service;
        }

        /// <summary>
        /// Register new tester session
        /// </summary>
        /// <param name="name">Tester name</param>
        /// <returns></returns>
        // GET: api/register
        public IHttpActionResult Get(string name)
        {
            var tester = this._services.Register(name);
            if(tester != null)
            {
                return Ok(tester);
            }else
            {
                return NotFound();
            }
        }

    }
}
