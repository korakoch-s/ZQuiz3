using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.BusinessEntities;

namespace ZQuiz.BusinessServices
{
    /// <summary>
    /// ZQuiz business service contract
    /// </summary>
    public interface IZQuizService
    {
        TesterEntity Register(string name);
        IEnumerable<QuestionEntity> GetAllQuestions();
        TesterEntity LoadTesterByName(string name);
        TesterEntity SaveTest(TesterEntity tester);
        TesterEntity SubmitTest(TesterEntity tester);
    }
}
