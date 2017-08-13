using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
//using System.Windows.Forms;

namespace XmlQuestion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ProcessingElement> ListProcessingElement = new List<ProcessingElement>();
        string pathForSaveXMLFiles;

        bool FileOrPathIsSelect = false;
        bool PathSaveIsSelect = false;


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

            if (ModeOfSelectMultiChoise.IsChecked == true)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (openFileDialog.ShowDialog() == true)
                {
                    FileOrPathIsSelect = true;
                    //pathForSaveXMLFiles = null;
                    ListProcessingElement.Clear();

                    int start = openFileDialog.FileName.LastIndexOf("\\") + 1;

                    string nameOfFile 
                        = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1,
                        openFileDialog.FileName.Length - start);
                    
                    ListOfSelectFile.Items.Add(nameOfFile);
                    PathToFileExcel.Content = openFileDialog.FileName;
                    ListProcessingElement.Add(new ProcessingElement(openFileDialog.FileName));
                    //openFileDialog.filed
                    //pathForSaveXMLFiles =  openFileDialog.FileName + ".Parse.xml";

                }
            }
            else
            {
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        FileOrPathIsSelect = true;
                        ListProcessingElement.Clear();
                        ListOfSelectFile.Items.Clear();

                        string[] files = Directory.GetFiles(fbd.SelectedPath);
                        PathToFileExcel.Content = fbd.SelectedPath;
                        foreach (var el in files)
                        {
                            if (el.Contains("xlsx"))
                            {
                                int start = el.LastIndexOf("\\") + 1;
                                string nameOfFile = el.Substring(el.LastIndexOf("\\") + 1, el.Length - start);

                                ListOfSelectFile.Items.Add(nameOfFile);
                                ListProcessingElement.Add(new ProcessingElement(el));
                            }                            
                        }
                       // System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
                    }
                }
            }
        }

        private void StartParsing(object sender, RoutedEventArgs e)
        {
            //обязательные условия начала парсинга
            //выбрать файл или папку 
            //если пользователь выбрал что нужно сохранить в определённую папку то проследить чтобы эта папка была выбрана

            //выбрана ли папка или файл для парсинга
            if (FileOrPathIsSelect)
            {
                //выбран режим иной папки для сохранения
                if (ModeOfSelectSavePath.IsChecked == false)
                {
                    //выбрана папака для сохранени
                    if (PathSaveIsSelect)
                    {
                        foreach (var el in ListProcessingElement)
                        {
                            el.Go(pathForSaveXMLFiles);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Не выбрана папка для сохранения");
                        //не выбрана папка 
                    }
                }
                else
                {
                    //сохранить в туже папку
                    foreach (var el in ListProcessingElement)
                    {
                        el.Go();
                    }
                }
            }
            else
            {
                //ошибка файл или папка не выбраны
                System.Windows.Forms.MessageBox.Show("Файл или папка не выбраны");
            }
        }

        private void ModeOfSelectMultiChoise_Click(object sender, RoutedEventArgs e)
        {
            if (ModeOfSelectMultiChoise.IsChecked == true)
            {
                PathSaveXMLFiles.IsEnabled = false;
                ButtonSelectSavePath.IsEnabled = false;                
            }
            else
            {
                PathSaveXMLFiles.IsEnabled = true;
                ButtonSelectSavePath.IsEnabled = true;
            }
        }

        private void ModeOfSelectSavePath_Click(object sender, RoutedEventArgs e)
        {
            if (ModeOfSelectSavePath.IsChecked == true)
            {
                PathSaveXMLFiles.IsEnabled = false;
                ButtonSelectSavePath.IsEnabled = false;                
            }
            else
            {
                PathSaveXMLFiles.IsEnabled = true;
                ButtonSelectSavePath.IsEnabled = true;
            }
        }

        private void ButtonSelectSavePath_Click(object sender, RoutedEventArgs e)
        {

            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    PathSaveXMLFiles.Content = fbd.SelectedPath + "\\";
                    pathForSaveXMLFiles= fbd.SelectedPath + "\\";
                    PathSaveIsSelect = true;
                }
            }
        }
    }
}
