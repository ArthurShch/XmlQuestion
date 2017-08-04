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
        _Application excel = new _Excel.Application();
        Workbook wb;
        Worksheet ws;

        string path = "";
        int Sheet;

        public string categoryName;
        public string questionName;

        public int startRowsQuestion = 3;
        public int endRowsQuestion;

        public List<int[]> rangeOfQuestion = new List<int[]>();
        public List<Question> questions = new List<Question>();

        public ParseExel(string path, int Sheet)
        {
            this.path = path;
            this.Sheet = Sheet;
        }

        public void StartParse()
        {
            wb = excel.Workbooks.Open(path);
            ws = wb.Worksheets[Sheet];

            categoryName = ReadCell(0, 1);
            questionName = ReadCell(1, 1);

            endRowsQuestion = GetNumberLastRows();

            CreateQuestion();
            Close();
        }

        public void CreateQuestion()
        {
            foreach (int[] item in rangeOfQuestion)
            {
                Question Q = new Question();

                Q.numberQuestion = ReadCell(item[0], 0);
                Q.textQuestion = ReadCell(item[0], 1);

                for (int i = item[0]; i <= item[1]; i++)
                {
                    Q.answers.Add(new Answer()
                    {
                        text = ReadCell(i,1),
                        result = ReadCell(i, 2)
                    });
                }

                questions.Add(Q);
            }
        }

            public void Close()
            {
                wb.Save();
                wb.Close(true);
                excel.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
                excel = null;
            }

            public string ReadCell(int i, int j)//функция чтения ячейки i,j
            {
                //Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
                //Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@"C:\1.xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing); //открыть файл
                //Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1]; //получить 1 лист
                //var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейку
                //string[,] list = new string[lastCell.Column, lastCell.Row];
                i++;
                j++;
                if (ws.Cells[i, j].Value2 != null)
                    return Convert.ToString(ws.Cells[i, j].Value2);
                else
                    return "";
            }

            public int GetNumberLastRows()
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



                return currentsEndNumberPrevious;
            }

        }
    }
