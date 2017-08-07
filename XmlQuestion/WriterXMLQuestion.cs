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


        public void DoWriteQuestions()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (var el in ListQuestionWithType)
            {
                xRoot.AppendChild(WriteQuestion(el, xDoc));
            }

            xDoc.Save("users.xml");// Закрытие
        }

        XmlElement WriteQuestion(QuestionWithType question, XmlDocument xDoc)
        {
            //switch (question.type)
            //{
            //    case TypeQuestion.ShortAnswer:

            //        break;
            //    case TypeQuestion.MultiChoice:
            //        break;
            //    case TypeQuestion.TrueFalse:
            //        break;
            //    case TypeQuestion.Matching:
            //        break;
            //}
            XmlElement que = xDoc.CreateElement("question");

            //XmlElement question = xDoc.CreateElement("question");//1
            XmlAttribute questionType = xDoc.CreateAttribute("type");//2
            XmlText QuestionTypeName = xDoc.CreateTextNode("category");//3
            XmlElement questionCategory = xDoc.CreateElement("category");//4
            XmlElement questionCategoryText = xDoc.CreateElement("text");//5
                                                                         //XmlText QuestionCategoryTextText = xDoc.CreateTextNode("$course$/Животные");

            // xRoot.AppendChild(question);//1
            que.Attributes.Append(questionType);//2
            questionType.AppendChild(QuestionTypeName);//3
            que.AppendChild(questionCategory);//4
            questionCategory.AppendChild(questionCategoryText);//5


            return que;
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

        public void Go()
        {
            WriteStartFile();
            DoWriteQuestions();
        }



        public WriterXMLQuestion()
        {

            //подготовка файла для использования
            File.Delete("users.xml");//удаление если файл уже существует(чтобы не было конфликта перезаписи) 
            XmlTextWriter textWritter = new XmlTextWriter("users.xml", null); //создание xml
            textWritter.WriteStartDocument();// Начало чтения документа
            textWritter.WriteStartElement("quiz");// Создание корневого узла (для возможности работы с файлом)
            textWritter.WriteEndElement();// Конец записи
            textWritter.Close();// Закрытие файла







            ////////////////////
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("users.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            XmlElement question = xDoc.CreateElement("question");//1
            XmlAttribute questionType = xDoc.CreateAttribute("type");//2
            XmlText QuestionTypeName = xDoc.CreateTextNode("category");//3
            XmlElement questionCategory = xDoc.CreateElement("category");//4
            XmlElement questionCategoryText = xDoc.CreateElement("text");//5
            //XmlText QuestionCategoryTextText = xDoc.CreateTextNode("$course$/Животные");

            xRoot.AppendChild(question);//1
            question.Attributes.Append(questionType);//2
            questionType.AppendChild(QuestionTypeName);//3
            question.AppendChild(questionCategory);//4
            questionCategory.AppendChild(questionCategoryText);//5

            #region exampleQ

            //questionCategoryText.AppendChild(QuestionCategoryTextText);

            //type category text  $course$/Животные
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.Load("users.xml");
            //XmlElement xRoot = xDoc.DocumentElement;
            //// создаем новый элемент user
            //XmlElement userElem = xDoc.CreateElement("user");
            //// создаем атрибут name
            //XmlAttribute nameAttr = xDoc.CreateAttribute("name");
            //// создаем элементы company и age
            //XmlElement companyElem = xDoc.CreateElement("company");
            //XmlElement ageElem = xDoc.CreateElement("age");
            //// создаем текстовые значения для элементов и атрибута
            //XmlText nameText = xDoc.CreateTextNode("Mark Zuckerberg");
            //XmlText companyText = xDoc.CreateTextNode("Facebook");
            //XmlText ageText = xDoc.CreateTextNode("30");

            ////добавляем узлы
            //nameAttr.AppendChild(nameText);
            //companyElem.AppendChild(companyText);
            //ageElem.AppendChild(ageText);
            //userElem.Attributes.Append(nameAttr);
            //userElem.AppendChild(companyElem);
            //userElem.AppendChild(ageElem);
            //xRoot.AppendChild(userElem);

            #endregion

            xDoc.Save("users.xml");// Закрытие
        }


    }
}
