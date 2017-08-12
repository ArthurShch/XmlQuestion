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
            ParseExel = new ParseExel(path,1);
        }

        public void Go(string pathToSave)
        {
            ParseExel.StartParse();
            IoTQ = new IdentificationOfTypeQuestion(ParseExel.questions);

            WXMLQ = new WriterXMLQuestion(IoTQ.ListQuestionWithType, ParseExel.categoryName, ParseExel.questionName, pathToSave);

        }
    }
}
