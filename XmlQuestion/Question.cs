using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlQuestion
{
    public class Question
    {
        public string textQuestion;
        public string numberQuestion;

        public List<Answer> answers = new List<Answer>();
    }

    public class Answer
    {
        public string text;
        public string result;
    }
}
