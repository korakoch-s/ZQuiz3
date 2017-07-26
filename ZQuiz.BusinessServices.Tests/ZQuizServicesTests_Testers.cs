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
    public class ZQuizServicesTests_Testers
    {
        private IZQuizService _zquizService;
        private IUnitOfWork _unitOfWork;
        private List<Tester> _testers;
        private GenericRespository<Tester> _testerRepository;
        private ZQuiz3DBEntities _dbEntities;

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            _testers = SetupTesters();
        }

        [SetUp]
        public void ReinitializeTest()
        {
            _testers = SetupTesters();
            _dbEntities = new Mock<ZQuiz3DBEntities>().Object;
            _testerRepository = SetupTesterRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.TesterRespository).Returns(_testerRepository);
            _unitOfWork = unitOfWork.Object;
            _zquizService = new ZQuizServices(_unitOfWork);
        }

        /// <summary>
        /// Setup dummy questions data
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
            _testerRepository = null;
            if (_dbEntities != null)
                _dbEntities.Dispose();
            _testers = null;
        }

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [TestFixtureTearDown]
        public void DisposeAllObjects()
        {
            _testers = null;
        }

    }
}