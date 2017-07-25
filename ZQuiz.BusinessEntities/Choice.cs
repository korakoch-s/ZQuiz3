using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZQuiz.BusinessEntities
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        public string Title { get; set; }
        public int Score { get; set; }
        public Question Question { get; set; }
    }
}
