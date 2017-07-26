using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel;

namespace ZQuiz.TestHelper
{
    /// <summary>
    /// Data initializer for unit tests
    /// </summary>
    public class DataInitializer
    {
        /// <summary>
        /// Dummy questions data
        /// </summary>
        /// <returns></returns>
        public static List<Question> GetAllQuestions()
        {
            var questions = new List<Question>();

            for (int i = 1; i <= 5; i++)
            {
                var question = new Question()
                {
                    Title = "Question number " + i,
                    TotalScore = 10,
                    Choices = new List<Choice>()
                };
                for (int j = 1; j <= 5; j++)
                {
                    var choice = new Choice()
                    {
                        Title = "Choice " + j + " of " + question.Title,
                        Score = j * 2
                    };
                    question.Choices.Add(choice);
                }
                questions.Add(question);
            }

            return questions;
        }

        /// <summary>
        /// Dummy testers data
        /// </summary>
        /// <returns></returns>
        public static List<Tester> GetAllTesters()
        {
            var testers = new List<Tester>();
            for (int i=1; i<=5; i++)
            {
                var tester = new Tester()
                {
                    Name = "Tester " + i,
                    IsCompleted = false
                };
                testers.Add(tester);
            }

            return testers;
        }

    }
}
