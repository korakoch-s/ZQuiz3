using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZQuiz.BusinessEntities
{
    public class TesterQuestion
    {
        public int TesterId { get; set; }
        public Tester Tester { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int? AnsChoiceId { get; set; }
        public Choice Choice { get; set; }
    }
}
