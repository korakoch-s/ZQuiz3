using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.BusinessEntities;
using ZQuiz.DataModel.UnitOfWork;

namespace ZQuiz.BusinessServices
{
    /// <summary>
    /// ZQuiz services for quiz application
    /// </summary>
    public class ZQuizServices : IZQuizService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Service constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ZQuizServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CalculateRanking(TesterEntity tester)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionEntity> GetAllQuestions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Load previous tester data
        /// </summary>
        /// <param name="name">Tester name</param>
        /// <returns></returns>
        public TesterEntity LoadTesterByName(string name)
        {
            throw new NotImplementedException();
        }

        public TesterEntity Register(string name)
        {
            throw new NotImplementedException();
        }

        public TesterEntity SaveTest(TesterEntity tester)
        {
            throw new NotImplementedException();
        }

        public TesterEntity SubmitTest(TesterEntity tester)
        {
            throw new NotImplementedException();
        }
    }
}
