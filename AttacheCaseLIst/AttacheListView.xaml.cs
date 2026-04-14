using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Input;

namespace WpfTutorialSamples.ListView_control
{
    public partial class ListViewColumnSortingSample : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public class AttacheFile
        {
            public string name { get; set; } = string.Empty;

            public string lastWriteTime { get; set; } = string.Empty;
        }

        public ListViewColumnSortingSample()
        {
            InitializeComponent();
            string[] getFiles = System.IO.Directory.GetFiles(@"C:\_User\atc", "*.atc");
            attaItems.ItemsSource = makeListAttacheFiles(getFiles).OrderByDescending(x => x.lastWriteTime);
            itemCount.Content = attaItems.Items.Count + " items";
        }

        private AttacheFile makeListAttache(string item)
        {
            var filedata = new AttacheFile();
            filedata.name = item;
            filedata.lastWriteTime = File.GetLastWriteTime(item).ToString();
 
            return filedata;
        }

        private List<AttacheFile> makeListAttacheFiles(string[] getFiles)
        {
            List<AttacheFile> attacheFiles = new();

            foreach (string item in getFiles)
            {
                attacheFiles.Add(makeListAttache(item));
            }

            return attacheFiles;
        }

        private List<AttacheFile> makeListAttacheFiles(List<string> getFiles)
        {
            List<AttacheFile> attacheFiles = new();

            foreach (string item in getFiles)
            {
                attacheFiles.Add(makeListAttache(item));
            }

            return attacheFiles;
        }

        private void attaItemsColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                attaItems.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            attaItems.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void SearchTextBox_ChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            if (args.Changes.Count > 0)
            {
                var textbox = sender as TextBox;
                //System.Diagnostics.Debug.WriteLine(textbox);

                List<string> getFiles = System.IO.Directory.GetFiles(@"C:\_User\atc", "*.atc").Where(x => x.Contains(textbox.Text)).ToList();
                attaItems.ItemsSource = makeListAttacheFiles(getFiles);
                itemCount.Content = attaItems.Items.Count + " items";
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // sender がダブルクリックされた項目
            ListView targetItem = (ListView)sender;

            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.UseShellExecute = true;
            pInfo.FileName = "C:\\Program Files (x86)\\AttacheCase4\\AttacheCase.exe";

            AttacheFile ta = (AttacheFile)targetItem.SelectedValue;
            pInfo.Arguments = '\"' + ta.name + '\"';

            Process.Start(pInfo);
        }
    }

    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}