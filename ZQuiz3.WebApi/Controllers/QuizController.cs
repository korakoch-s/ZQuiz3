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
    /// Quiz controller for quiz questions
    /// </summary>
    public class QuizController : ApiController
    {
        private readonly IZQuizService _services;

        public QuizController(IZQuizService service)
        {
            _services = service;
        }

        /// <summary>
        /// Load all questions for quiz session.
        /// </summary>
        /// <returns></returns>
        // GET: api/quiz
        public IHttpActionResult Get()
        {
            var questions = this._services.GetAllQuestions();
            return Ok(questions);
        }
    }
}
