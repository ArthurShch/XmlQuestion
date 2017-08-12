using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XmlQuestion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ProcessingElement> ListProcessingElement = new List<ProcessingElement>();
        string pathForSaveXMLFiles;


        public MainWindow()
        {
            InitializeComponent();

            //ParseExel excel = new ParseExel(@"D:\ExampleExcel.xlsx", 1);

            //excel.StartParse();

            //IdentificationOfTypeQuestion uhhjujuh = new IdentificationOfTypeQuestion(excel.questions);

            //WriterXMLQuestion dwdwdsawd = new WriterXMLQuestion(uhhjujuh.ListQuestionWithType, excel.categoryName, excel.questionName, "users.xml");

           
            
            //ParseExel excel1232 = new ParseExel(@"D:\ExampleExcel.xlsx", 1);
            


            //string sdasdawdwad = excel.ReadCell(1, 1);


        }

        private void SelectFileForParse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            

            if (openFileDialog.ShowDialog() == true)
            {
                ListProcessingElement.Clear();
                ListProcessingElement.Add(new ProcessingElement(openFileDialog.FileName));
                //openFileDialog.filed

            }
            
            //using (var fbd = new FolderBrowserDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        string[] files = Directory.GetFiles(fbd.SelectedPath);

            //        System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
            //    }
            //}
        }

        private void StartParsing(object sender, RoutedEventArgs e)
        {
            foreach (var el in ListProcessingElement)
            {
                el.Go("user.xml");
            }
        }

        //Start Parsing
    }
}
