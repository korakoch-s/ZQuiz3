﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZQuiz.BusinessEntities
{
    public class Tester
    {
        public int TesterId { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public int Score { get; set; }
        public int TotalScore { get; set; }

        public IEnumerable<TesterQuestion> TesterQuestions { get; set; }
    }
}
