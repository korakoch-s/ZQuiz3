using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZQuiz.BusinessEntities
{
    public class TesterQuestionEntity
    {
        public int TesterId { get; set; }
        public TesterEntity Tester { get; set; }
        public int QuestionId { get; set; }
        public QuestionEntity Question { get; set; }
        public int? AnsChoiceId { get; set; }
        public ChoiceEntity Choice { get; set; }
    }
}
