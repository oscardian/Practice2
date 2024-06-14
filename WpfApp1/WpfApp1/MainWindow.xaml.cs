using ClassLibrary1.Domain.Files;
using ClassLibrary1.Service.Files;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tools;
using static System.Net.Mime.MediaTypeNames;
using File = ClassLibrary1.Domain.Files.File;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowFiles();
        }

        List<String> Files = new List<String>();

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
               
                Files.AddRange(files);
            }
            LoadedFiles();
        }

        #region Dictionary

        Dictionary<string, FileType> FileTypesDictionary = new Dictionary<string, FileType>
            {
                { ".rar", FileType.Arhived },
                { ".zip", FileType.Arhived },

                { ".avi", FileType.Video},
                { ".mpeg1", FileType.Video },
                { ".mpeg2", FileType.Video },
                { ".mpeg4", FileType.Video },
                { ".swf", FileType.Video},

                { ".gif", FileType.Graphic},
                { ".jpg", FileType.Graphic},
                { ".psd", FileType.Graphic},
                { ".tif",FileType.Graphic},

                { ".doc",FileType.Text},
                { ".pdf",FileType.Text},
                { ".rtf",FileType.Text},
                { ".txt",FileType.Text},

                { ".midi", FileType.Sound },
                { ".mp3", FileType.Sound },
                { ".wav", FileType.Sound },
                { ".wma", FileType.Sound },
                { ".au", FileType.Sound },
                { ".mod", FileType.Sound },
            };
        #endregion

        BackgroundWorker bw;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bw = new BackgroundWorker();
            bw.DoWork += (obj, ea) =>
            {
                List<string> files = Files;

                List<Task> tasks = new List<Task>();

                foreach (string file in files)
                {
                    Task task = Task.Run(() => ProcessFile(file));
                    tasks.Add(task);
                }

                Task.WhenAll(tasks).ContinueWith((t) =>
                {
                    MessageBox.Show("Хеш сумма всех файлов почитана успешно.");
                    
                });
            };

            bw.RunWorkerAsync();           
        }

        private void ProcessFile(string file)
        {
            FileBlank fileBlank = new FileBlank();
            FileInfo fileInfo = new FileInfo(file);
            fileBlank.Name = fileInfo.Name;
            fileBlank.FileType = FileTypesDictionary.ContainsKey(fileInfo.Extension) ? FileTypesDictionary[fileInfo.Extension] : FileType.Unknown;
            fileBlank.HashSum = CalculateHash.CalculateSHA256Hash(file);

            FileService fileService = new FileService();
            fileService.SaveFile(fileBlank);
        }

        public void LoadedFiles()
        {
            loadedFiles.Children.Clear();
            foreach (string file in Files)
            {
                Grid grid = new Grid() { Height = 20, Width = 180 };
   
                FileInfo fileInfo = new FileInfo(file);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = $"{fileInfo.Name}";

                grid.Children.Add(textBlock);


                loadedFiles.Children.Add(grid);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Files.Clear();
            LoadedFiles();
        }

        public class DataItem<T>
        {
            public String HashSum { get; set; }
            public String FileName { get; set; }
            public FileType FileType { get; set; }
        }

        private File[] LoadFiles()
        {
            FileService fileService = new FileService();

            return fileService.GetFiles();
        }

        private void ShowFiles()
        {
            List<File> files = LoadFiles().ToList();

            List<DataItem<Double>> dataItems = new List<DataItem<Double>>();

            for (int i = 0; i < files.Count; i++)
            {
                dataItems.Add(new DataItem<Double> { HashSum = files[i].HashSum , FileName = files[i].Name, FileType = files[i].FileType });
            }

            listViewArrayB.ItemsSource = dataItems;
        }

        private void dataBaseCheched(object sender, MouseButtonEventArgs e)
        {
            ShowFiles();
        }
    }
}