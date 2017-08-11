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
        public WriterXMLQuestion(List<QuestionWithType> ListQuestionWithType, string category, string nameQuestion)
        {
            this.category = category;
            this.nameQuestion = nameQuestion;

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
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement XMLQuestionHead = xDoc.CreateElement("question");
            XmlAttribute XMLQuestionHeadType = xDoc.CreateAttribute("type");
            XmlText XMLQuestionHeadTypeName = xDoc.CreateTextNode("category");
            XmlElement XMLQuestionHeadCategory = xDoc.CreateElement("category");
            XmlElement XMLQuestionHeadCategoryText = xDoc.CreateElement("text");
            XmlText XMLQuestionHeadCategoryTextText = xDoc.CreateTextNode("$course$/" + this.category);

            xRoot.AppendChild(XMLQuestionHead);
            XMLQuestionHead.Attributes.Append(XMLQuestionHeadType);
            XMLQuestionHeadType.AppendChild(XMLQuestionHeadTypeName);
            XMLQuestionHead.AppendChild(XMLQuestionHeadCategory);
            XMLQuestionHeadCategory.AppendChild(XMLQuestionHeadCategoryText);
            XMLQuestionHeadCategoryText.AppendChild(XMLQuestionHeadCategoryTextText);
            
            foreach (var el in ListQuestionWithType)
            {
                xRoot.AppendChild(WriteQuestion(el, xDoc));
            }

            xDoc.Save("users.xml");// Закрытие
        }

        XmlElement WriteQuestion(QuestionWithType question, XmlDocument xDoc)
        {
            XmlElement XMLQuestion = xDoc.CreateElement("question");

            XmlAttribute XMLQuestionType = xDoc.CreateAttribute("type");
            XMLQuestion.Attributes.Append(XMLQuestionType);

            XmlText XMLQuestionTypeText = xDoc.CreateTextNode(question.type.ToString().ToLower());
            XMLQuestionType.AppendChild(XMLQuestionTypeText);

            XmlElement XMLQuestionName = xDoc.CreateElement("name");
            XMLQuestion.AppendChild(XMLQuestionName);

            XmlElement XMLQuestionNameText = xDoc.CreateElement("text");
            XMLQuestionName.AppendChild(XMLQuestionNameText);

            XmlText XMLQuestionCategoryTextText = xDoc.CreateTextNode(nameQuestion + " " + question.Question.NumberQuestion);
            XMLQuestionNameText.AppendChild(XMLQuestionCategoryTextText);

            XmlElement XMLQuestionText = xDoc.CreateElement("questiontext");
            XMLQuestion.AppendChild(XMLQuestionText);

            XmlElement XMLQuestionTextText = xDoc.CreateElement("text");
            XMLQuestionText.AppendChild(XMLQuestionTextText);

            XmlCDataSection XMLQuestionTextTextCDDataText = xDoc.CreateCDataSection("<p>" + question.Question.TextQuestion + "</p>");
            XMLQuestionTextText.AppendChild(XMLQuestionTextTextCDDataText);

            XmlElement XMLQuestionGeneralFeedback = xDoc.CreateElement("generalfeedback");
            XMLQuestion.AppendChild(XMLQuestionGeneralFeedback);

            XmlElement XMLQuestionGeneralFeedbackText = xDoc.CreateElement("text");
            XMLQuestionGeneralFeedback.AppendChild(XMLQuestionGeneralFeedbackText);

            XmlElement XMLQuestionDefaultGrade = xDoc.CreateElement("defaultgrade");
            XMLQuestion.AppendChild(XMLQuestionDefaultGrade);

            XmlElement XMLQuestionDefaultGradeText = xDoc.CreateElement("text");
            XMLQuestionDefaultGrade.AppendChild(XMLQuestionDefaultGradeText);

            XmlText XMLQuestionDefaultGradeTextText = xDoc.CreateTextNode("1.0000000");
            XMLQuestionDefaultGradeText.AppendChild(XMLQuestionDefaultGradeTextText);




            //
            //XmlCDataSection cdata = xDoc.CreateCDataSection("<p>" + textQuestion + "</p>");//7

            switch (question.type)
            {
                case TypeQuestion.ShortAnswer:

                    break;
                case TypeQuestion.MultiChoice:
                    CreateQuestionMultiChoice(ref XMLQuestion, xDoc, question);
                    break;
                case TypeQuestion.TrueFalse:
                    break;
                case TypeQuestion.Matching:
                    break;
            }



            return XMLQuestion;
        }

        public void WriteStartFile()
        {
            //подготовка файла для использования
            File.Delete("users.xml");//удаление если файл уже существует(чтобы не было конфликта перезаписи) 
            XmlTextWriter textWritter = new XmlTextWriter("users.xml", null); //создание xml
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


        void CreateQuestionMultiChoice(ref XmlElement XMLQuestion, XmlDocument xDoc, QuestionWithType QWT)
        {
            AddXmlElement(ref XMLQuestion, xDoc, "penalty", "0.3333333");

            //XmlElement XMLQuestionPenalty = xDoc.CreateElement("penalty");
            //XMLQuestion.AppendChild(XMLQuestionPenalty);

            //XmlText XMLQuestionPenaltyText = xDoc.CreateTextNode("0.3333333");
            //XMLQuestionPenalty.AppendChild(XMLQuestionPenaltyText);

            AddXmlElement(ref XMLQuestion, xDoc, "hidden", "0");

            //XmlElement XMLQuestionHidden = xDoc.CreateElement("hidden");
            //XMLQuestion.AppendChild(XMLQuestionHidden);

            //XmlText XMLQuestionHiddenText = xDoc.CreateTextNode("0");
            //XMLQuestionHidden.AppendChild(XMLQuestionHiddenText);

            //правельный ответ один или несколько
            bool singleCorrectAnswer = QWT.Question.Answers.Count(x => x.Result == "1") == 1;

            AddXmlElement(ref XMLQuestion, xDoc, "single", singleCorrectAnswer.ToString().ToLower());

            //XmlElement XMLQuestionSingle = xDoc.CreateElement("single");
            //XMLQuestion.AppendChild(XMLQuestionSingle);

            //XmlText XMLQuestionSingleText = xDoc.CreateTextNode(singleCorrectAnswer.ToString().ToLower());
            //XMLQuestionSingle.AppendChild(XMLQuestionSingleText);

            AddXmlElement(ref XMLQuestion, xDoc, "shuffleanswers", "true");

            ////XmlElement XMLQuestionShuffleAnswers = xDoc.CreateElement("shuffleanswers");
            ////XMLQuestion.AppendChild(XMLQuestionShuffleAnswers);

            ////XmlText XMLQuestionShuffleAnswersText = xDoc.CreateTextNode("true");
            ////XMLQuestionShuffleAnswers.AppendChild(XMLQuestionShuffleAnswersText);

            AddXmlElement(ref XMLQuestion, xDoc, "answernumbering", "abc");
            //XmlElement XMLQuestionAnswerNumbering = xDoc.CreateElement("answernumbering");
            //XMLQuestion.AppendChild(XMLQuestionAnswerNumbering);

            //XmlText XMLQuestionAnswerNumberingText = xDoc.CreateTextNode("abc");
            //XMLQuestionAnswerNumbering.AppendChild(XMLQuestionAnswerNumberingText);

            #region feedback
            XmlElement XMLQuestionCorrectFeedback = xDoc.CreateElement("correctfeedback");
            XMLQuestion.AppendChild(XMLQuestionCorrectFeedback);

            XmlElement XMLQuestionCorrectFeedbackText = xDoc.CreateElement("text");
            XMLQuestion.AppendChild(XMLQuestionCorrectFeedback);

            AddAtribute(ref XMLQuestionCorrectFeedback, xDoc, "format", "html");

            //XmlAttribute XMLQuestionCorrectFeedbackAttributeFormat = xDoc.CreateAttribute("format");
            //XMLQuestionCorrectFeedback.Attributes.Append(XMLQuestionCorrectFeedbackAttributeFormat);

            //XmlText XMLQuestionCorrectFeedbackAttributeFormatText = xDoc.CreateTextNode("html");
            //XMLQuestionCorrectFeedback.AppendChild(XMLQuestionCorrectFeedbackAttributeFormatText);




            #endregion
            //correctfeedback




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
                double countInCorrectAnswer = QWT.Question.Answers.Count(x => x.Result == "0");

                answerFractionCorrect = ((double)(100.00 / countCorrectAnswer)).ToString("F5");
                answerFractionInCorrect = ((double)(-100.00 / countInCorrectAnswer)).ToString("F5");
            }



            //singleCorrectAnswer.ToString();











        }


        void CreateQuestionShortAnswer(ref XmlElement XMLQuestion, QuestionWithType QWT)
        {

        }






        //public WriterXMLQuestion()
        //{

        //    //подготовка файла для использования
        //    File.Delete("users.xml");//удаление если файл уже существует(чтобы не было конфликта перезаписи) 
        //    XmlTextWriter textWritter = new XmlTextWriter("users.xml", null); //создание xml
        //    textWritter.WriteStartDocument();// Начало чтения документа
        //    textWritter.WriteStartElement("quiz");// Создание корневого узла (для возможности работы с файлом)
        //    textWritter.WriteEndElement();// Конец записи
        //    textWritter.Close();// Закрытие файла








    }
}
