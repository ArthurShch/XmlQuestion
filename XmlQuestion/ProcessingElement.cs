using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlQuestion
{
    class ProcessingElement
    {
        ParseExel ParseExel;
        IdentificationOfTypeQuestion IoTQ;
        WriterXMLQuestion WXMLQ;

        public ProcessingElement(string path)
        {
            ParseExel = new ParseExel(path, 1);
        }

        public void Go(string pathToSave = null)
        {
            

            //если путь сохранения не указан то создаётся файл в тойже папке что и исходный 
            if (pathToSave == null)
            {
                pathToSave = ParseExel.path.Substring(0, ParseExel.path.IndexOf(".xlsx")) + ".Parse.xml";
            }
            //иначе создаётся в выбранной папке
            else
            {
                int start = ParseExel.path.LastIndexOf("\\") + 1;

                string nameOfFile = ParseExel.path.Substring(ParseExel.path.LastIndexOf("\\") + 1, ParseExel.path.Length - start);


                //ParseExel.path.Substring(ParseExel.path.LastIndexOf("\\"));
                pathToSave += nameOfFile + ".Parse.xml";
            }

            ParseExel.StartParse();
            IoTQ = new IdentificationOfTypeQuestion(ParseExel.questions);
            WXMLQ = new WriterXMLQuestion(IoTQ.ListQuestionWithType, ParseExel.categoryName, ParseExel.questionName, pathToSave);
        }
    }
}
