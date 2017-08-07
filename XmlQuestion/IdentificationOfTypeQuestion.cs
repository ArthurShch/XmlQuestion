using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlQuestion
{
    //типы вопросов
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

    //класс определяющий типы вопросов 
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

        //функция определения типа вопросов
        public void ReceivingTypeQuestion()
        {
            foreach (var el in questions)
            {
                if (el.TextQuestion.Contains("___"))
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.ShortAnswer });
                    continue;
                }

                int countAnswers = el.Answers.Count(x => x.Text != "");
                int countResultAnswers = el.Answers.Count(x => x.Result != "");

                if (countAnswers == 2 && countResultAnswers == 1)
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.TrueFalse });
                    continue;
                }

                if (el.Answers.FindAll(x => (x.Result != "1" && x.Result != "") ).Count > 0)
                {
                    ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.Matching });
                    continue;
                }

                ListQuestionWithType.Add(new QuestionWithType() { Question = el, type = TypeQuestion.MultiChoice });
            }
        }
    }
}
