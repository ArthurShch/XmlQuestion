using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlQuestion
{


    class WriterXMLQuestion
    {
        public List<QuestionWithType> ListQuestionWithType = new List<QuestionWithType>();

        public string PathToSaveFile;

        string category;
        string nameQuestion;

        public WriterXMLQuestion() { }
        public WriterXMLQuestion(List<QuestionWithType> ListQuestionWithType, string category, string nameQuestion, string PathToSaveFile)
        {
            this.category = category;
            this.nameQuestion = nameQuestion;
            this.PathToSaveFile = PathToSaveFile;

            this.ListQuestionWithType = ListQuestionWithType.ToList();

            Go();
        }

        public void Go()
        {
            WriteStartFile();
            DoWriteQuestions();
        }


        public void DoWriteQuestions()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(PathToSaveFile);
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement XMLQuestionHead = AddXmlElement(ref xRoot, xDoc, "question");
            AddAtribute(ref XMLQuestionHead, xDoc, "type", "category");
            XmlElement XMLQuestionHeadCategory = AddXmlElement(ref xRoot, xDoc, "category");
            AddXmlElement(ref XMLQuestionHeadCategory, xDoc, "text", "$course$/" + this.category);

            foreach (var el in ListQuestionWithType)
            {
                xRoot.AppendChild(WriteQuestion(el, xDoc));
            }

            xDoc.Save(PathToSaveFile);// Закрытие
        }

        XmlElement WriteQuestion(QuestionWithType question, XmlDocument xDoc)
        {
            XmlElement XMLQuestion = xDoc.CreateElement("question");
            AddAtribute(ref XMLQuestion, xDoc, "type", question.type.ToString().ToLower());

            XmlElement XMLQuestionName = AddXmlElement(ref XMLQuestion, xDoc, "name");
            AddXmlElement(ref XMLQuestionName, xDoc, "text", nameQuestion + " " + question.Question.NumberQuestion);

            XmlElement XMLQuestionText = AddXmlElement(ref XMLQuestion, xDoc, "questiontext");
            AddAtribute(ref XMLQuestionText, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionText, xDoc, question.Question.TextQuestion);

            XmlElement XMLQuestionGeneralFeedback = AddXmlElement(ref XMLQuestion, xDoc, "generalfeedback");
            AddAtribute(ref XMLQuestionGeneralFeedback, xDoc, "format", "html");

            AddXmlElement(ref XMLQuestionGeneralFeedback, xDoc, "text");
            AddXmlElement(ref XMLQuestion, xDoc, "defaultgrade", "1.0000000");

            switch (question.type)
            {
                case TypeQuestion.ShortAnswer:
                    CreateQuestionShortAnswer(ref XMLQuestion, xDoc, question);
                    break;
                case TypeQuestion.MultiChoice:
                    CreateQuestionMultiChoice(ref XMLQuestion, xDoc, question);
                    break;
                case TypeQuestion.TrueFalse:
                    CreateQuestionTrueFalse(ref XMLQuestion, xDoc, question);
                    break;
                case TypeQuestion.Matching:
                    CreateQuestionMatching(ref XMLQuestion, xDoc, question);
                    break;
            }

            return XMLQuestion;
        }

        public void WriteStartFile()
        {
            //подготовка файла для использования
            File.Delete(PathToSaveFile);//удаление если файл уже существует(чтобы не было конфликта перезаписи) 
            XmlTextWriter textWritter = new XmlTextWriter(PathToSaveFile, null); //создание xml
            textWritter.WriteStartDocument();// Начало чтения документа
            textWritter.WriteStartElement("quiz");// Создание корневого узла (для возможности работы с файлом)
            textWritter.WriteEndElement();// Конец записи
            textWritter.Close();// Закрытие файла
        }

        void AddAtribute(ref XmlElement goal, XmlDocument xDoc, string attributeName, string attributeText)
        {
            XmlAttribute AttributeFormat = xDoc.CreateAttribute(attributeName);
            goal.Attributes.Append(AttributeFormat);

            XmlText AttributeText = xDoc.CreateTextNode(attributeText);
            AttributeFormat.AppendChild(AttributeText);
        }

        XmlElement AddXmlElement(ref XmlElement goal, XmlDocument xDoc, string ElName, string ElText = null)
        {
            XmlElement El = xDoc.CreateElement(ElName);

            if (ElText != null)
            {
                XmlText ElementText = xDoc.CreateTextNode(ElText);
                El.AppendChild(ElementText);
            }

            goal.AppendChild(El);

            return El;
        }

        void AddCDATAElement(ref XmlElement goal, XmlDocument xDoc, string CDATAText)
        {
            XmlElement ElText = xDoc.CreateElement("text");
            XmlCDataSection XMLCDDataText = xDoc.CreateCDataSection("<p>" + CDATAText + "</p>");

            ElText.AppendChild(XMLCDDataText);
            goal.AppendChild(ElText);
        }

        void CreateQuestionMatching(ref XmlElement XMLQuestion, XmlDocument xDoc, QuestionWithType QWT)
        {
            AddXmlElement(ref XMLQuestion, xDoc, "penalty", "0.3333333");
            AddXmlElement(ref XMLQuestion, xDoc, "hidden", "0");
            AddXmlElement(ref XMLQuestion, xDoc, "shuffleanswers", "true");

            #region feedback
            XmlElement XMLQuestionCorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "correctfeedback");
            AddAtribute(ref XMLQuestionCorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionCorrectFeedback, xDoc, "Ваш ответ верный.");

            XmlElement XMLQuestionPartiallyCorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "partiallycorrectfeedback");
            AddAtribute(ref XMLQuestionPartiallyCorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionPartiallyCorrectFeedback, xDoc, "Ваш ответ частично правильный.");

            XmlElement XMLQuestionIncorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "incorrectfeedback");
            AddAtribute(ref XMLQuestionIncorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionIncorrectFeedback, xDoc, "Ваш ответ неправильный.");

            #endregion

            AddXmlElement(ref XMLQuestion, xDoc, "shownumcorrect");

            foreach (var el in QWT.Question.Answers)
            {
                XmlElement XMLQuestionSubQuestion = AddXmlElement(ref XMLQuestion, xDoc, "subquestion");
                AddAtribute(ref XMLQuestionSubQuestion, xDoc, "format", "html");

                if (el.Text != "")
                {
                    AddCDATAElement(ref XMLQuestionSubQuestion, xDoc, el.Text);
                }
                else
                {
                    AddXmlElement(ref XMLQuestionSubQuestion, xDoc, "text");
                }

                XmlElement XMLQuestionAnswer = AddXmlElement(ref XMLQuestionSubQuestion, xDoc, "answer");
                AddXmlElement(ref XMLQuestionAnswer, xDoc,"text", el.Result);
            }
        }

        void CreateQuestionMultiChoice(ref XmlElement XMLQuestion, XmlDocument xDoc, QuestionWithType QWT)
        {
            AddXmlElement(ref XMLQuestion, xDoc, "penalty", "0.3333333");
            AddXmlElement(ref XMLQuestion, xDoc, "hidden", "0");
            //правельный ответ один или несколько
            bool singleCorrectAnswer = QWT.Question.Answers.Count(x => x.Result == "1") == 1;

            AddXmlElement(ref XMLQuestion, xDoc, "single", singleCorrectAnswer.ToString().ToLower());
            AddXmlElement(ref XMLQuestion, xDoc, "shuffleanswers", "true");
            AddXmlElement(ref XMLQuestion, xDoc, "answernumbering", "abc");

            #region feedback
            XmlElement XMLQuestionCorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "correctfeedback");
            AddAtribute(ref XMLQuestionCorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionCorrectFeedback, xDoc, "Ваш ответ верный.");

            XmlElement XMLQuestionPartiallyCorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "partiallycorrectfeedback");
            AddAtribute(ref XMLQuestionPartiallyCorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionPartiallyCorrectFeedback, xDoc, "Ваш ответ частично правильный.");

            XmlElement XMLQuestionIncorrectFeedback = AddXmlElement(ref XMLQuestion, xDoc, "incorrectfeedback");
            AddAtribute(ref XMLQuestionIncorrectFeedback, xDoc, "format", "html");
            AddCDATAElement(ref XMLQuestionIncorrectFeedback, xDoc, "Ваш ответ неправильный.");

            #endregion

            AddXmlElement(ref XMLQuestion, xDoc, "shownumcorrect");

            string answerFractionCorrect;
            string answerFractionInCorrect;

            if (singleCorrectAnswer)
            {
                answerFractionCorrect = "100";
                answerFractionInCorrect = "0";
            }
            else
            {
                double countCorrectAnswer = QWT.Question.Answers.Count(x => x.Result == "1");
                double countInCorrectAnswer = QWT.Question.Answers.Count(x => x.Result == "");

                //возможно сдесь должна быть другая логика округления в случае получения целого числа

                answerFractionCorrect = ((double)(100.00 / countCorrectAnswer)).ToString("F5").Replace(',', '.');
                answerFractionInCorrect = ((double)(-100.00 / countInCorrectAnswer)).ToString("F5").Replace(',', '.');
            }

            foreach (var el in QWT.Question.Answers)
            {
                XmlElement XMLQuestionAnswer = AddXmlElement(ref XMLQuestion, xDoc, "answer");

                AddAtribute(ref XMLQuestionAnswer, xDoc, "fraction",
                    el.Result == "1" ? answerFractionCorrect : answerFractionInCorrect);
                AddAtribute(ref XMLQuestionAnswer, xDoc, "format", "html");
                AddCDATAElement(ref XMLQuestionAnswer, xDoc, el.Text);

                XmlElement XMLQuestionAnswerFeedback = AddXmlElement(ref XMLQuestionAnswer, xDoc, "feedback");
                AddAtribute(ref XMLQuestionAnswerFeedback, xDoc, "format", "html");
                AddXmlElement(ref XMLQuestionAnswerFeedback, xDoc, "text");
            }
        }

        void CreateQuestionShortAnswer(ref XmlElement XMLQuestion, XmlDocument xDoc, QuestionWithType QWT)
        {
            AddXmlElement(ref XMLQuestion, xDoc, "penalty", "0.3333333");
            AddXmlElement(ref XMLQuestion, xDoc, "hidden", "0");
            AddXmlElement(ref XMLQuestion, xDoc, "usecase", "0");

            foreach (var el in QWT.Question.Answers)
            {
                XmlElement XMLQuestionAnswer = AddXmlElement(ref XMLQuestion, xDoc, "answer");

                AddAtribute(ref XMLQuestionAnswer, xDoc, "fraction", "100");
                AddAtribute(ref XMLQuestionAnswer, xDoc, "format", "moodle_auto_format");
                AddXmlElement(ref XMLQuestionAnswer, xDoc, "text", el.Text);

                XmlElement XMLQuestionAnswerFeedback = AddXmlElement(ref XMLQuestionAnswer, xDoc, "feedback");
                AddAtribute(ref XMLQuestionAnswerFeedback, xDoc, "format", "html");
                AddXmlElement(ref XMLQuestionAnswerFeedback, xDoc, "text");
            }
        }

        void CreateQuestionTrueFalse(ref XmlElement XMLQuestion, XmlDocument xDoc, QuestionWithType QWT)
        {
            AddXmlElement(ref XMLQuestion, xDoc, "penalty", "1.0000000");
            AddXmlElement(ref XMLQuestion, xDoc, "hidden", "0");

            foreach (var el in QWT.Question.Answers)
            {
                string answerNumber = el.Result == "1" ? "100" : "0";

                XmlElement XMLQuestionAnswer = AddXmlElement(ref XMLQuestion, xDoc, "answer");

                AddAtribute(ref XMLQuestionAnswer, xDoc, "fraction", answerNumber);
                AddAtribute(ref XMLQuestionAnswer, xDoc, "format", "moodle_auto_format");
                AddXmlElement(ref XMLQuestionAnswer, xDoc, "text", el.Result);

                XmlElement XMLQuestionAnswerFeedback = AddXmlElement(ref XMLQuestionAnswer, xDoc, "feedback");
                AddAtribute(ref XMLQuestionAnswerFeedback, xDoc, "format", "html");
                AddXmlElement(ref XMLQuestionAnswerFeedback, xDoc, "text");
            }
        }
    }
}
