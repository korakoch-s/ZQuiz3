﻿using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using ZQuiz.BusinessEntities;
using ZQuiz.BusinessServices;
using ZQuiz.DataModel;
using ZQuiz.DataModel.GenericRepository;
using ZQuiz.DataModel.UnitOfWork;
using ZQuiz.TestHelper;
using ZQuiz.WebApi.Controllers;

namespace ZQuiz3.WebApi.Tests
{
    class ControllerTest
    {
        [TestFixture()]
        public class QuizControllerTests
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
            private HttpClient _client;
            private IHttpActionResult _response;
            private const string SERVER_BASE_URL = "http://localhost:54263/";

            /// <summary>
            /// Initial setup for tests
            /// </summary>
            [TestFixtureSetUp()]
            public void Setup()
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

                _client = new HttpClient { BaseAddress = new Uri(SERVER_BASE_URL) };

            }

            public void ReinitializeTest()
            {
                _testers = SetupTesters();
                _questions = SetupQuestions();
                _choices = SetupChoices();
                _testerQuestions = SetupTesterQuestions();

                _client = new HttpClient { BaseAddress = new Uri(SERVER_BASE_URL) };
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

                mockRepo.Setup(t => t.GetMany(It.IsAny<Func<Tester, bool>>()))
                    .Returns((Func<Tester, bool> expr) =>
                    {
                        return _testers.Where(expr);
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

                mockRepo.Setup(t => t.GetMany(It.IsAny<Func<TesterQuestion, bool>>()))
                    .Returns((Func<TesterQuestion, bool> expr) =>
                    {
                        return _testerQuestions.Where(expr).ToList();
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

            [Test()]
            public void Get_QuizTest()
            {
                var quizController = new QuizController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                _response = quizController.Get();
                var resp = _response as OkNegotiatedContentResult<IEnumerable<QuestionEntity>>;

                Assert.IsNotNull(resp);

                var responseResults = resp.Content;
                Assert.AreEqual(responseResults.Any(), true);
                var questionList = responseResults.Select(
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

            [Test()]
            public void Get_RegisterTest()
            {
                var quizController = new RegisterController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                var newTester = new TesterEntity()
                {
                    Name = "New_Tester_Name",
                    IsCompleted = false,
                    Score = 0,
                    TotalScore = 0
                };
                var maxTesterIdBeforeAdd = _testers.Max(t => t.TesterId);
                newTester.TesterId = maxTesterIdBeforeAdd + 1;

                _response = quizController.Get(newTester.Name);


                var returnTester = (_response as OkNegotiatedContentResult<TesterEntity>).Content;
                Assert.IsNotNull(returnTester, "Should return TesterEntity object");
                Assert.AreEqual(maxTesterIdBeforeAdd + 1, _testers.Last().TesterId);
                Assert.IsTrue(TesterComparer.CompareModelAndEntity(_testers.Last(), returnTester));

            }

            [Test()]
            public void Get_RegisterExistTest()
            {
                var quizController = new RegisterController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                var tester = _testers.Find(t => t.TesterId == 2);

                _response = quizController.Get(tester.Name);
                var returnTester = (_response as OkNegotiatedContentResult<TesterEntity>).Content;

                Assert.IsNotNull(returnTester, "Should return TesterEntity object");
                Assert.IsTrue(TesterComparer.CompareModelAndEntity(tester, returnTester));

            }

            [Test()]
            public void Get_LoadTest()
            {
                var quizController = new LoadController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                var tester = _testers.Find(t => t.TesterId == 3);

                _response = quizController.Get(tester.Name);
                var returnTester = (_response as OkNegotiatedContentResult<TesterEntity>).Content;

                Assert.IsNotNull(returnTester, "Should return TesterEntity object");
                Assert.IsTrue(TesterComparer.CompareModelAndEntity(tester, returnTester));
                Assert.Greater(returnTester.TesterQuestions.Count, 0, "TesterQuestions must > 0");

            }

            [Test()]
            public void POST_SaveTest()
            {
                var quizController = new SaveController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                var firstTester = _testers.First();
                var ttEntity = new TesterEntity();

                //mock modify test information

                ttEntity.TesterId = firstTester.TesterId;
                ttEntity.Name = firstTester.Name;

                firstTester.TesterQuestions = new List<TesterQuestion>();
                ttEntity.TesterQuestions = new List<TesterQuestionEntity>();
                foreach (var qt in _questions)
                {
                    var tqe = new TesterQuestionEntity()
                    {
                        TesterId = firstTester.TesterId,
                        QuestionId = qt.QuestionId,
                        AnsChoiceId = qt.Choices.Last().ChoiceId,
                    };
                    ttEntity.TesterQuestions.Add(tqe);

                    var tq = new TesterQuestion()
                    {
                        TesterId = firstTester.TesterId,
                        QuestionId = qt.QuestionId,
                        AnsChoiceId = qt.Choices.Last().ChoiceId,
                    };
                    firstTester.TesterQuestions.Add(tq);
                }

                _response = quizController.Post(ttEntity);
                var saveTester = (_response as OkNegotiatedContentResult<TesterEntity>).Content;

                Assert.IsNotNull(saveTester, "Should return tester entity");

                Assert.AreEqual(firstTester.TesterId, saveTester.TesterId, "Same tester id");
                Assert.AreEqual(firstTester.Name, saveTester.Name, "Same tester name");
                Assert.AreEqual(firstTester.IsCompleted, saveTester.IsCompleted, "Same tester status");
                Assert.AreEqual(firstTester.IsCompleted, saveTester.IsCompleted, "Same tester status");
                Assert.AreEqual(firstTester.TesterQuestions.Count, saveTester.TesterQuestions.Count, "Same test item count");

                firstTester.TesterQuestions.OrderBy(tq => tq.QuestionId);
                saveTester.TesterQuestions.OrderBy(tq => tq.QuestionId);

                int index = 0;
                foreach (var ftq in firstTester.TesterQuestions)
                {
                    var stq = saveTester.TesterQuestions.ElementAt(index);
                    Assert.AreEqual(ftq.QuestionId, stq.QuestionId, "Same question");
                    Assert.AreEqual(ftq.AnsChoiceId, stq.AnsChoiceId, "Same answer");
                    index++;
                }
            }

            [Test()]
            public void POST_SubmitTest()
            {
                var quizController = new SubmitController(_zquizService)
                {
                    Request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(SERVER_BASE_URL + "api/quiz")
                    }
                };

                quizController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

                var firstTester = _testers.Last();
                var ttEntity = new TesterEntity();

                int expScore = 0;
                int expTotalScore = 0;
                int expRank = 1; //First submit must return 1

                //mock modify test information

                ttEntity.TesterId = firstTester.TesterId;
                ttEntity.Name = firstTester.Name;

                firstTester.TesterQuestions = new List<TesterQuestion>();
                ttEntity.TesterQuestions = new List<TesterQuestionEntity>();
                foreach (var qt in _questions)
                {
                    var tqe = new TesterQuestionEntity()
                    {
                        TesterId = firstTester.TesterId,
                        QuestionId = qt.QuestionId,
                        AnsChoiceId = qt.Choices.ElementAt(3).ChoiceId,
                    };
                    ttEntity.TesterQuestions.Add(tqe);

                    var tq = new TesterQuestion()
                    {
                        TesterId = firstTester.TesterId,
                        QuestionId = qt.QuestionId,
                        AnsChoiceId = qt.Choices.ElementAt(3).ChoiceId,
                    };

                    expScore += qt.Choices.ElementAt(3).Score;
                    expTotalScore += qt.TotalScore;

                    firstTester.TesterQuestions.Add(tq);
                }

                Assert.AreEqual(40, expScore);  //Depend on mockup data
                Assert.AreEqual(50, expTotalScore); //Depend on mockup data

                _response = quizController.Post(ttEntity);
                var saveTester = (_response as OkNegotiatedContentResult<TesterEntity>).Content;

                Assert.IsNotNull(saveTester, "Should return tester entity");

                Assert.AreEqual(firstTester.TesterId, saveTester.TesterId, "Same tester id");
                Assert.AreEqual(firstTester.Name, saveTester.Name, "Same tester name");
                Assert.AreEqual(firstTester.TesterQuestions.Count, saveTester.TesterQuestions.Count, "Same test item count");

                firstTester.TesterQuestions.OrderBy(tq => tq.QuestionId);
                saveTester.TesterQuestions.OrderBy(tq => tq.QuestionId);

                int index = 0;
                foreach (var ftq in firstTester.TesterQuestions)
                {
                    var stq = saveTester.TesterQuestions.ElementAt(index);
                    Assert.AreEqual(ftq.QuestionId, stq.QuestionId, "Same question");
                    Assert.AreEqual(ftq.AnsChoiceId, stq.AnsChoiceId, "Same answer");
                    index++;
                }

                Assert.AreEqual(expScore, saveTester.Score, "Tester Score");
                Assert.AreEqual(expTotalScore, saveTester.TotalScore, "Total Score");
                Assert.AreEqual(expRank, saveTester.Rank, "Rank");
            }

            #region clean up test
            /// <summary>
            /// Tears down each test data
            /// </summary>
            [TearDown]
            public void DisposeTest()
            {
                if (_response != null)
                    _response = null;
                if (_client != null)
                    _client.Dispose();

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

                _zquizService = null;
                _unitOfWork = null;
                _testerRepository = null;
                _questionRepository = null;
                _choiceRepository = null;
                _testerQuestionRepository = null;

                if (_dbEntities != null)
                    _dbEntities.Dispose();

                if (_response != null)
                    _response = null;
                if (_client != null)
                    _client.Dispose();

            }
            #endregion
        }
    }
}
