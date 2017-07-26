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

                var testerEntity = Mapper.Map<Tester, TesterEntity>(tester);
                if (testerEntity.IsCompleted)
                {
                    testerEntity.Rank = this.CalculateRanking(testerEntity);
                }
                scope.Complete();

                return testerEntity;
            }
        }

        public TesterEntity SaveTest(TesterEntity tester)
        {
            var exTester = this._unitOfWork.TesterRespository.GetById(tester.TesterId);
            var retTester = null as TesterEntity;

            if (exTester != null)
            {
                using (var scope = new TransactionScope())
                {
                    exTester.Name = tester.Name;
                    exTester.IsCompleted = tester.IsCompleted;

                    exTester.Score = 0;
                    exTester.TotalScore = 0;
                    exTester.TesterQuestions.Clear();
                    foreach (var tqe in tester.TesterQuestions)
                    {
                        exTester.TesterQuestions.Add(new TesterQuestion()
                        {
                            TesterId = exTester.TesterId,
                            QuestionId = tqe.QuestionId,
                            AnsChoiceId = tqe.AnsChoiceId
                        });
                        var quest = _unitOfWork.QuestionRepository.GetSingle(qt => qt.QuestionId == tqe.QuestionId);
                        if (quest != null)
                        {
                            exTester.TotalScore += quest.TotalScore;
                        }
                        var choi = _unitOfWork.ChoiceRepository.GetSingle(ch => ch.ChoiceId == tqe.AnsChoiceId);
                        if (choi != null)
                        {
                            exTester.Score += choi.Score;
                        }
                    }
                    _unitOfWork.TesterRespository.Update(exTester);
                    _unitOfWork.Save();

                    retTester = Mapper.Map<Tester, TesterEntity>(exTester);

                    scope.Complete();
                }
            }

            return retTester;
        }

        public TesterEntity SubmitTest(TesterEntity tester)
        {
            tester.IsCompleted = true;
            var saveTester = this.SaveTest(tester);

            //Calculate ranking before return
            saveTester.Rank = this.CalculateRanking(saveTester);

            return saveTester;
        }

        private int CalculateRanking(TesterEntity tester)
        {
            var allTesters = _unitOfWork.TesterRespository.GetMany(tt => tt.IsCompleted == true).ToList();
            var testersDesc = allTesters.OrderByDescending(tt => tt.Score);
            int rank = 0;
            foreach (var tt in testersDesc)
            {
                rank++;
                if (tt.TesterId == tester.TesterId)
                {
                    return rank;
                }
            }
            return 0;
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
