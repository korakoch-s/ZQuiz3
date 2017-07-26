using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZQuiz.BusinessServices;

namespace ZQuiz.WebApi.Controllers
{
    public class RegisterController : ApiController
    {
        private readonly IZQuizService _services;

        public RegisterController(IZQuizService service)
        {
            _services = service;
        }

        // GET: api/register
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Register/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Register
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Register/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Register/5
        public void Delete(int id)
        {
        }
    }
}
