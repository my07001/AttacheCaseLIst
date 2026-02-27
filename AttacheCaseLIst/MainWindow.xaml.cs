using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WpfTutorialSamples.ListView_control;

namespace AttacheCaseLIst
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ObservableCollection<string> FileList;

        public class AttacheFile
        {
            public string name { get; set; } = string.Empty;

            public string lastWriteTime { get; set; } = string.Empty;
        }

        public MainWindow()
        {
            InitializeComponent();
            FileList = new ObservableCollection<string>();
            //FileListBox.ItemsSource = FileList;
            FileListBox.ItemsSource = System.IO.Directory.GetFiles(@"C:\_User\atc", "*.atc");


            List<AttacheFile> attacheFiles = new();
            string[] getFiles = System.IO.Directory.GetFiles(@"C:\_User\atc", "*.atc");
            foreach (string item in getFiles)
            {
                var filedata = new AttacheFile();
                filedata.name = item;
                filedata.lastWriteTime = File.GetLastWriteTime(item).ToString();
                attacheFiles.Add(filedata);
            }


        }

        private void FileListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var name in fileNames)
                {
                    FileList.Add(name);
                }
            }
        }

        private void FileListBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.All;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void FileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listBoxItem = sender as ListBoxItem;

            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.UseShellExecute = true;
            pInfo.FileName = "C:\\Program Files (x86)\\AttacheCase3\\AttacheCase.exe";

            var path1 = (string)FileListBox.Items[FileListBox.SelectedIndex];
            //System.Diagnostics.Debug.WriteLine(path1);

            pInfo.Arguments = path1;

            Process.Start(pInfo);
        }

        private void SearchTextBox_ChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if(args.Changes.Count > 0)
            {
                var textbox = sender as TextBox;
                //System.Diagnostics.Debug.WriteLine(textbox);

                FileList = new ObservableCollection<string>();
                FileListBox.ItemsSource = System.IO.Directory.GetFiles(@"C:\_User\atc", "*.atc").Where(x => x.Contains(textbox.Text)).ToList();
            }
        }
    }
}
