using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZQuiz.BusinessEntities;
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

            temp += expected.TesterId.CompareTo(actual.TesterId) != 0 ? temp : expected.Name.CompareTo(actual.Name);
            temp += expected.Name.CompareTo(actual.Name);
            temp += expected.IsCompleted.CompareTo(actual.IsCompleted);
            temp += expected.Score.CompareTo(actual.Score);
            temp += expected.TotalScore.CompareTo(actual.TotalScore);

            if (temp > 0)
                return 1;
            else
                return 0;
        }

        public static bool CompareModelAndEntity(Tester testerModel, TesterEntity testerEntity)
        {
            int temp = 0;
            Assert.AreEqual(testerModel.TesterId, testerEntity.TesterId, "TesterId");
            temp += testerModel.TesterId.CompareTo(testerEntity.TesterId);
            Assert.AreEqual(testerModel.Name, testerEntity.Name, "Name");
            temp += testerModel.Name.CompareTo(testerEntity.Name);
            Assert.AreEqual(testerModel.IsCompleted, testerEntity.IsCompleted, "IsCompleted");
            temp += testerModel.IsCompleted.CompareTo(testerEntity.IsCompleted);
            Assert.AreEqual(testerModel.Score, testerEntity.Score, "Score");
            temp += testerModel.Score.CompareTo(testerEntity.Score);
            Assert.AreEqual(testerModel.TotalScore, testerEntity.TotalScore, "TotalScore");
            temp += testerModel.TotalScore.CompareTo(testerEntity.TotalScore);

            if (temp == 0)
                return true;
            else
                return false;
        }

    }
}

