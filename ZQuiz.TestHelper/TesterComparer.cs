using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.DataModel;

namespace ZQuiz.TestHelper
{
    public class TesterComparer : IComparer, IComparer<Tester>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as Tester;
            var rhs = actual as Tester;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(Tester expected, Tester actual)
        {

            int temp = 0;

            temp = expected.TesterId.CompareTo(actual.TesterId) != 0 ? temp : expected.Name.CompareTo(actual.Name);

            return temp;
        }
    }
}
