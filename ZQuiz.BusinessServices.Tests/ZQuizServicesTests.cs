using NUnit.Framework;
using ZQuiz.BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel.UnitOfWork;
using ZQuiz.DataModel;
using ZQuiz.DataModel.GenericRepository;
using ZQuiz.TestHelper;
using Moq;
using ZQuiz.BusinessEntities;
using System.Linq.Expressions;

namespace ZQuiz.BusinessServices.Tests
{
    /// <summary>
    /// ZQuiz services test
    /// </summary>
    [TestFixture()]
    public class ZQuizServicesTests
    {
        private IZQuizService _zquizService;
        private IUnitOfWork _unitOfWork;

        private List<Tester> _testers;
        private List<Question> _questions;
        private List<Choice> _choices;
        private List<TesterQuestion> _testerQuestions;

        private GenericRespository<Tester> _testerRepository;
        private GenericRespository<Question> _questionRepository;
        private GenericRespository<Choice> _choiceRepository;
        private GenericRespository<TesterQuestion> _testerQuestionRepository;

        private ZQuiz3DBEntities _dbEntities;

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            _testers = SetupTesters();
            _questions = SetupQuestions();
            _choices = SetupChoices();
            _testerQuestions = SetupTesterQuestions();
        }

        [SetUp]
        public void ReinitializeTest()
        {
            _testers = SetupTesters();
            _questions = SetupQuestions();
            _choices = SetupChoices();
            _testerQuestions = SetupTesterQuestions();

            _dbEntities = new Mock<ZQuiz3DBEntities>().Object;
            var unitOfWork = new Mock<IUnitOfWork>();

            _testerRepository = SetupTesterRepository();
            _questionRepository = SetupQuestionRepository();
            _choiceRepository = SetupChoiceRepository();
            _testerQuestionRepository = SetupTesterQuestionRepository();

            unitOfWork.SetupGet(s => s.TesterRespository).Returns(_testerRepository);
            unitOfWork.SetupGet(s => s.QuestionRepository).Returns(_questionRepository);
            unitOfWork.SetupGet(s => s.ChoiceRepository).Returns(_choiceRepository);
            unitOfWork.SetupGet(s => s.TesterQuestionRepository).Returns(_testerQuestionRepository);

            _unitOfWork = unitOfWork.Object;
            _zquizService = new ZQuizServices(_unitOfWork);
        }


        #region Private setup mock data
        /// <summary>
        /// Setup dummy testers data
        /// </summary>
        /// <returns></returns>
        private static List<Tester> SetupTesters()
        {
            var testerId = new int();

            var testers = DataInitializer.GetAllTesters();
            foreach (Tester tester in testers)
            {
                tester.TesterId = ++testerId;
            }

            return testers;
        }

        /// <summary>
        /// Setup dummy questions data
        /// </summary>
        /// <returns></returns>
        private static List<Question> SetupQuestions()
        {
            var questionId = new int();
            var choiceId = new int();

            var questions = DataInitializer.GetAllQuestions();
            foreach (Question quest in questions)
            {
                quest.QuestionId = ++questionId;
                foreach (Choice ch in quest.Choices)
                {
                    ch.ChoiceId = ++choiceId;
                    ch.Question = quest;
                    ch.QuestionId = quest.QuestionId;
                }
            }

            return questions;
        }

        /// <summary>
        /// Setup dummy choice data
        /// </summary>
        /// <returns></returns>
        private static List<Choice> SetupChoices()
        {
            var choices = new List<Choice>();

            var questionId = new int();
            var choiceId = new int();

            var questions = DataInitializer.GetAllQuestions();
            foreach (Question quest in questions)
            {
                quest.QuestionId = ++questionId;
                foreach (Choice ch in quest.Choices)
                {
                    ch.ChoiceId = ++choiceId;
                    ch.Question = quest;
                    ch.QuestionId = quest.QuestionId;
                    choices.Add(ch);
                }
            }

            return choices;
        }

        /// <summary>
        /// Setup dummy tester question data
        /// </summary>
        /// <returns></returns>
        private static List<TesterQuestion> SetupTesterQuestions()
        {
            var testerQuestions = DataInitializer.GetAllTesterQuestions();

            return testerQuestions;
        }

        #endregion

        #region Private setup repositories
        /// <summary>
        /// Setup question repository for tests
        /// </summary>
        /// <returns></returns>
        private GenericRespository<Tester> SetupTesterRepository()
        {
            var mockRepo = new Mock<GenericRespository<Tester>>(MockBehavior.Default, _dbEntities);

            mockRepo.Setup(t => t.GetAll()).Returns(_testers);

            mockRepo.Setup(t => t.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Tester>(
                        id => _testers.Find(t => t.TesterId.Equals(id))
                        ));

            mockRepo.Setup(t => t.GetSingle(It.IsAny<Func<Tester, bool>>()))
                .Returns((Func<Tester, bool> expr) =>
                {
                    return _testers.FirstOrDefault(expr);
                });

            mockRepo.Setup(t => t.Insert((It.IsAny<Tester>())))
                .Callback(new Action<Tester>(newTester =>
                    {
                        dynamic maxTesterId = _testers.Last().TesterId;
                        dynamic nextTesterId = maxTesterId + 1;
                        newTester.TesterId = nextTesterId;
                        _testers.Add(newTester);
                    }));

            mockRepo.Setup(t => t.Update(It.IsAny<Tester>()))
                .Callback(new Action<Tester>(tester =>
                   {
                       var oldTester = _testers.Find(t => t.TesterId == tester.TesterId);
                       oldTester = tester;
                   }));

            return mockRepo.Object;
        }

        /// <summary>
        /// Setup question repository for tests
        /// </summary>
        /// <returns></returns>
        private GenericRespository<Question> SetupQuestionRepository()
        {
            var mockRepo = new Mock<GenericRespository<Question>>(MockBehavior.Default, _dbEntities);

            mockRepo.Setup(t => t.GetAll()).Returns(_questions);

            mockRepo.Setup(t => t.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Question>(
                        id => _questions.Find(t => t.QuestionId.Equals(id))
                        ));

            mockRepo.Setup(t => t.GetSingle(It.IsAny<Func<Question, bool>>()))
                .Returns((Func<Question, bool> expr) =>
                {
                    return _questions.FirstOrDefault(expr);
                });

            mockRepo.Setup(t => t.Insert((It.IsAny<Question>())))
                .Callback(new Action<Question>(newQuestion =>
                {
                    dynamic maxQuestionId = _questions.Last().QuestionId;
                    dynamic nextQuestionId = maxQuestionId + 1;
                    newQuestion.QuestionId = nextQuestionId;
                    _questions.Add(newQuestion);
                }));

            mockRepo.Setup(t => t.Update(It.IsAny<Question>()))
                .Callback(new Action<Question>(question =>
                {
                    var oldQuestion = _questions.Find(t => t.QuestionId == question.QuestionId);
                    oldQuestion = question;
                }));

            return mockRepo.Object;
        }

        /// <summary>
        /// Setup choices repository for tests
        /// </summary>
        /// <returns></returns>
        private GenericRespository<Choice> SetupChoiceRepository()
        {
            var mockRepo = new Mock<GenericRespository<Choice>>(MockBehavior.Default, _dbEntities);

            mockRepo.Setup(t => t.GetAll()).Returns(_choices);

            mockRepo.Setup(t => t.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Choice>(
                        id => _choices.Find(t => t.ChoiceId.Equals(id))
                        ));

            mockRepo.Setup(t => t.GetSingle(It.IsAny<Func<Choice, bool>>()))
                .Returns((Func<Choice, bool> expr) =>
                {
                    return _choices.FirstOrDefault(expr);
                });

            mockRepo.Setup(t => t.Insert((It.IsAny<Choice>())))
                .Callback(new Action<Choice>(newChoice =>
                {
                    dynamic maxChoiceId = _choices.Last().ChoiceId;
                    dynamic nextChoiceId = maxChoiceId + 1;
                    newChoice.ChoiceId = nextChoiceId;
                    _choices.Add(newChoice);
                }));

            mockRepo.Setup(t => t.Update(It.IsAny<Choice>()))
                .Callback(new Action<Choice>(choice =>
                {
                    var oldChoice = _choices.Find(t => t.ChoiceId == choice.ChoiceId);
                    oldChoice = choice;
                }));

            return mockRepo.Object;
        }

        /// <summary>
        /// Setup choices repository for tests
        /// </summary>
        /// <returns></returns>
        private GenericRespository<TesterQuestion> SetupTesterQuestionRepository()
        {
            var mockRepo = new Mock<GenericRespository<TesterQuestion>>(MockBehavior.Default, _dbEntities);

            mockRepo.Setup(t => t.GetAll()).Returns(_testerQuestions);

            mockRepo.Setup(t => t.GetById(It.IsAny<int>()))
                .Returns(new Func<int, TesterQuestion>(
                        id => _testerQuestions.Find(t => t.TesterId.Equals(id))
                        ));

            mockRepo.Setup(t => t.GetSingle(It.IsAny<Func<TesterQuestion, bool>>()))
                .Returns((Func<TesterQuestion, bool> expr) =>
                {
                    return _testerQuestions.FirstOrDefault(expr);
                });

            mockRepo.Setup(t => t.Insert((It.IsAny<TesterQuestion>())))
                .Callback(new Action<TesterQuestion>(newTesterQuestion =>
                {
                    _testerQuestions.Add(newTesterQuestion);
                }));

            mockRepo.Setup(t => t.Update(It.IsAny<TesterQuestion>()))
                .Callback(new Action<TesterQuestion>(ttq =>
                {
                    var oldTesterQuestion = _testerQuestions.Find(t => (t.TesterId == ttq.TesterId) && (t.QuestionId == ttq.QuestionId));
                    oldTesterQuestion = ttq;
                }));

            return mockRepo.Object;
        }

        #endregion

        /// <summary>
        /// Service should return all questions
        /// </summary>
        [Test()]
        public void GetAllQuestionsTest()
        {
            var questions = _zquizService.GetAllQuestions();

            if (questions != null)
            {
                var questionList = questions.Select(
                    questionEntity =>
                    new Question
                    {
                        QuestionId = questionEntity.QuestionId,
                        Title = questionEntity.Title
                    }).ToList();
                var comparer = new QuestionComparer();
                CollectionAssert.AreEqual(
                    questionList.OrderBy(qt => qt, comparer),
                    _questions.OrderBy(qt => qt, comparer), comparer);
            }

        }

        [Test()]
        public void RegisterTest()
        {
            var newTester = new TesterEntity()
            {
                Name = "New_Unit_Tester_Name",
                IsCompleted = false,
                Score = 0,
                TotalScore = 0
            };
            var maxTesterIdBeforeAdd = _testers.Max(t => t.TesterId);
            newTester.TesterId = maxTesterIdBeforeAdd + 1;
            var returnTester = _zquizService.Register(newTester.Name);
            Assert.AreEqual(maxTesterIdBeforeAdd + 1, _testers.Last().TesterId);
            Assert.IsTrue(TesterComparer.CompareModelAndEntity(_testers.Last(), returnTester));
        }

        [Test()]
        public void RegisterTest_withExistName()
        {
            var tester = _testers.Find(t => t.TesterId == 2);

            var maxTesterIdBeforeAdd = _testers.Max(t => t.TesterId);
            var returnTester = _zquizService.Register(tester.Name);
            Assert.IsTrue(TesterComparer.CompareModelAndEntity(tester, returnTester));
        }

        [Test()]
        public void CalculateRankingTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void LoadTesterByNameTest()
        {
            var tester = _testers.Find(t => t.TesterId == 3);

            var maxTesterIdBeforeAdd = _testers.Max(t => t.TesterId);
            var returnTester = _zquizService.LoadTesterByName(tester.Name);
            Assert.IsTrue(TesterComparer.CompareModelAndEntity(tester, returnTester));
            Assert.IsNotNull(returnTester.TesterQuestions);
            Assert.Greater(returnTester.TesterQuestions.Count, 0, "TesterQuestions must > 0");
        }

        [Test()]
        public void LoadTesterByNameTest_withNonExist()
        {
            var returnTester = _zquizService.LoadTesterByName("No name not exit in repo.");
            Assert.IsNull(returnTester);
        }

        [Test()]
        public void SaveTestTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void SubmitTestTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {
            _zquizService = null;
            _unitOfWork = null;
            _testerRepository = null;
            _questionRepository = null;
            _choiceRepository = null;
            _testerQuestionRepository = null;

            if (_dbEntities != null)
                _dbEntities.Dispose();
            _testers = null;
            _questions = null;
            _choices = null;
            _testerQuestions = null;
        }

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [TestFixtureTearDown]
        public void DisposeAllObjects()
        {
            _testers = null;
            _questions = null;
            _choices = null;
            _testerQuestions = null;
        }

    }
}