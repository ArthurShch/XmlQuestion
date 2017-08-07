using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlQuestion
{
    //public interface IQuestion
    //{
    //    string TextQuestion { get; set; }

    //    string NumberQuestion { get; set; }

    //    List<IAnswer> Answers { get; set; }
    //}

    public class Question /*: IQuestion*/
    {
        public string TextQuestion { get; set; }
        public string NumberQuestion { get; set; }

        //public List<IAnswer> Answers { get; set; } = new List<IAnswer>();
        public List<Answer> Answers { get; set; } = new List<Answer>();

        //public List<Answer> answers = new List<Answer>();
    }

    //public interface IAnswer
    //{
    //    string Text { get; set; }
    //    string Result { get; set; }
    //}

    public class Answer /*: IAnswer*/
    {
        public string Text { get; set; }
        public string Result { get; set; }
    }
}
