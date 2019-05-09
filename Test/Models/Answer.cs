using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Answer
    {
        private int questionNumber;
        private string typeOf;
        private int answerNumber;

        public Answer(int qn,string type,int an)
        {
            questionNumber = qn;
            typeOf = type;
            answerNumber = an;
        }

        public int QuestionNumber { get => questionNumber; set => questionNumber = value; }
        public string TypeOf { get => typeOf; set => typeOf = value; }
        public int AnswerNumber { get => answerNumber; set => answerNumber = value; }

        public override string ToString()
        {
            return $"qn {questionNumber} {typeOf} an {answerNumber}";
        }
    }
}
