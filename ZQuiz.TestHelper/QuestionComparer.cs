using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel;

namespace ZQuiz.TestHelper
{
    public class QuestionComparer : IComparer, IComparer<Question>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as Question;
            var rhs = actual as Question;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(Question expected, Question actual)
        {

            int temp = 0;

            temp = expected.QuestionId.CompareTo(actual.QuestionId) != 0 ? temp : expected.Title.CompareTo(actual.Title);

            return temp;
        }
    }
}
