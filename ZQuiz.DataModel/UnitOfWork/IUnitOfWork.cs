using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel.GenericRepository;

namespace ZQuiz.DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        GenericRespository<Tester> TesterRespository { get; }
        GenericRespository<Question> QuestionRepository { get; }
        GenericRespository<Choice> ChoiceRepository { get; }
        GenericRespository<TesterQuestion> TesterQuestionRepository { get; }

        /// <summary>
        /// Save method.
        /// </summary>
        void Save();
    }
}
