using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// библиотеки для работы с экселем
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace XmlQuestion
{
    class ParseExel
    {
        _Application excel;
        Workbook wb;
        Worksheet ws;

        public string path = "";
        int Sheet;

        public string categoryName;
        public string questionName;

        public int startRowsQuestion = 3;
        public int endRowsQuestion;

        public List<int[]> rangeOfQuestion = new List<int[]>();
        public List<Question> questions = new List<Question>();

        //в конструкторе сохраняется путь к файлу и номер листа в excel
        public ParseExel(string path, int Sheet)
        {
            this.path = path;
            this.Sheet = Sheet;
        }
        //~ParseExel()
        //{
        //    Close();
        //}

        // функция выполняющая парсинг файла и страницы exel, которые указаны в конструкторе
        public void StartParse()
        {
            excel = new _Excel.Application();
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];

            categoryName = ReadCell(0, 1);
            questionName = ReadCell(1, 1);

            FindAllQuestion();
            CreateQuestion();

            Close();
        }

        //парсинг вопросов на основании информации об их границах 
        public void CreateQuestion()
        {
            foreach (int[] item in rangeOfQuestion)
            {
                Question Q = new Question();

                Q.NumberQuestion = ReadCell(item[0], 0);
                Q.TextQuestion = ReadCell(item[0], 1);

                for (int i = item[0] + 1; i <= item[1]; i++)
                {
                    Q.Answers.Add(new Answer()
                    {
                        Text = ReadCell(i, 1),
                        Result = ReadCell(i, 2)
                    });
                }

                questions.Add(Q);
            }
        }

        //закрытие приложения excel
        public void Close()
        {
            if (excel == null)
            {
                return;
            }

            var workbooks = excel.Workbooks;
            wb.Save();
            wb.Close(true);
            excel.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            excel = null;
            wb = null;
            ws = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        // получить содержание ячейки по высоте и ширине
        public string ReadCell(int i, int j)//функция чтения ячейки i,j
        {
            i++;
            j++;
            if (ws.Cells[i, j].Value2 != null)
                return Convert.ToString(ws.Cells[i, j].Value2);
            else
                return "";
        }

        //поиск границ вопросов по строкам в файле excel  
        public void FindAllQuestion()
        {
            int currentsStartNumberPrevious = startRowsQuestion;
            int currentsEndNumberPrevious = startRowsQuestion;

            //каждая итерация один вопрос
            while (true)
            {
                for (int i = currentsStartNumberPrevious; ; i++)
                {
                    string cellQuestion = ReadCell(i, 1);
                    string cellAnswer = ReadCell(i, 2);

                    if (cellQuestion == "" && cellAnswer == "")
                    {
                        currentsEndNumberPrevious = i - 1;
                        break;
                    }
                }

                rangeOfQuestion.Add(new int[] { currentsStartNumberPrevious, currentsEndNumberPrevious });

                currentsStartNumberPrevious = currentsEndNumberPrevious + 2;
                // проверка на конец файла
                if (ReadCell(currentsStartNumberPrevious, 1) == "")
                {
                    break;
                }
            }

            endRowsQuestion = currentsEndNumberPrevious;
        }

    }


}
