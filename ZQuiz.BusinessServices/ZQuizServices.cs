using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ZQuiz.BusinessEntities;
using ZQuiz.DataModel;
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
            this.InitializeMapper_ModelToEntity();
        }

        public int CalculateRanking(TesterEntity tester)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<QuestionEntity> GetAllQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.GetAll().ToList();
            if (questions.Any())
            {
                var questionModel = Mapper.Map<List<Question>, List<QuestionEntity>>(questions);
                return questionModel;
            }

            return null;
        }

        /// <summary>
        /// Load previous tester data
        /// </summary>
        /// <param name="name">Tester name</param>
        /// <returns></returns>
        public TesterEntity LoadTesterByName(string name)
        {
            var testerEntity = null as TesterEntity;
            var tester = this._unitOfWork.TesterRespository.GetSingle(t => t.Name == name);
            if (tester != null)
            {
                tester.TesterQuestions = this._unitOfWork.TesterQuestionRepository.GetMany(tq => tq.TesterId == tester.TesterId).ToList();
                testerEntity = Mapper.Map<Tester, TesterEntity>(tester);
            }

            return testerEntity;
        }

        /// <summary>
        /// Register new tester by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TesterEntity Register(string name)
        {
            using (var scope = new TransactionScope())
            {
                var tester = this._unitOfWork.TesterRespository.GetSingle(t => t.Name == name);

                if (tester == null)
                {
                    tester = new Tester
                    {
                        Name = name,
                        IsCompleted = false,
                        Score = 0,
                        TotalScore = 0
                    };
                    _unitOfWork.TesterRespository.Insert(tester);
                    _unitOfWork.Save();
                }
                scope.Complete();

                var testerEntity = Mapper.Map<Tester, TesterEntity>(tester);

                return testerEntity;
            }
        }

        public TesterEntity SaveTest(TesterEntity tester)
        {
            throw new NotImplementedException();
        }

        public TesterEntity SubmitTest(TesterEntity tester)
        {
            throw new NotImplementedException();
        }

        private void InitializeMapper_ModelToEntity()
        {
            Mapper.Initialize(cfg =>
           {
               cfg.CreateMap<Question, QuestionEntity>();
               cfg.CreateMap<Choice, ChoiceEntity>();
               cfg.CreateMap<Tester, TesterEntity>();
               cfg.CreateMap<TesterQuestion, TesterQuestionEntity>();
           });
        }
    }
}
