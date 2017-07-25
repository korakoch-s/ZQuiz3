using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZQuiz.BusinessEntities
{
    public class Question
    {
        public int QuestionId {get; set;}
        public string Title { get; set; }
        public int TotalScore { get; set; }
        public IEnumerable<Choice> Choices { get; set; }
    }
}
