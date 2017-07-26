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
        private List<Question> _questions;
        private GenericRespository<Question> _questionRepository;
        private ZQuiz3DBEntities _dbEntities;

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            _questions = SetupQuestions();
            _dbEntities = new Mock<ZQuiz3DBEntities>().Object;
            _questionRepository = SetupQuestionRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.QuestionRepository).Returns(_questionRepository);
            _unitOfWork = unitOfWork.Object;
            _zquizService = new ZQuizServices(_unitOfWork);
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
        /// Setup question repository for tests
        /// </summary>
        /// <returns></returns>
        private GenericRespository<Question> SetupQuestionRepository()
        {
            var mockRepo = new Mock<GenericRespository<Question>>(MockBehavior.Default, _dbEntities);

            mockRepo.Setup(q => q.GetAll()).Returns(_questions);

            mockRepo.Setup(q => q.GetById(It.IsAny<int>()))
                .Returns(new Func<int, Question>(
                        id => _questions.Find(q => q.QuestionId.Equals(id))
                        ));

            return mockRepo.Object;
        }

        [Test()]
        public void RegisterTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void CalculateRankingTest()
        {
            throw new NotImplementedException();
        }

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
        public void LoadTesterByNameTest()
        {
            throw new NotImplementedException();
        }

        [Test()]
        public void RegisterTest1()
        {
            throw new NotImplementedException();
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
            _questionRepository = null;
            if (_dbEntities != null)
                _dbEntities.Dispose();
            _questions = null;
        }

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [TestFixtureTearDown]
        public void DisposeAllObjects()
        {
            _questions = null;
        }

    }
}