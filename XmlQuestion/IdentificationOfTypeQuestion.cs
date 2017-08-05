using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlQuestion
{

    public enum TypeQuestion
    {
        ShortAnswer,
        MultiChoice,
        TrueFalse,
        Matching
    }

    public class QuestionWithType
    {
        public TypeQuestion type;
        public Question Question;
    }

    public class IdentificationOfTypeQuestion
    {
        public List<QuestionWithType> ListQuestionWithType = new List<QuestionWithType>();
        List<Question> questions;

        public IdentificationOfTypeQuestion() { }
        public IdentificationOfTypeQuestion(List<Question> questions)
        {
            this.questions = questions.ToList();

            ReceivingTypeQuestion();
        }

        public void ReceivingTypeQuestion()
        {
            foreach (var el in questions)
            {

                if (el.textQuestion.Contains("___"))
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.ShortAnswer });
                    continue;
                }

                int countAnswers = el.answers.Count(x => x.text != "");
                int countResultAnswers = el.answers.Count(x => x.result != "");

                if (countAnswers == 2 && countResultAnswers == 1)
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.TrueFalse });
                    continue;
                }

                if (el.answers.FindAll(x => (x.result != "1" && x.result != "") ).Count > 0)
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.Matching });
                    continue;
                }

                ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.MultiChoice });

            }
        }


    }
}
