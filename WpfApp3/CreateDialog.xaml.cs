using System;
using System.IO;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp3
{
    public partial class CreateDialog : System.Windows.Window
    {
        public string FilePath { get; set; }
        public bool IsDirectory { get; set; }
        //public bool IsFile => !IsDirectory;
        public bool IsReadOnly { get; set; }
        public bool IsArchive { get; set; }
        public bool IsHidden { get; set; }
        public bool IsSystem { get; set; }
        private string basePath;

        public CreateDialog(string basePath)
        {
            InitializeComponent();
            this.basePath = basePath;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name cannot be empty.", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return;
            }

            this.FilePath = Path.Combine(basePath, name);
            this.IsDirectory = DirectoryRadio.IsChecked == true;

            this.IsReadOnly = ReadOnlyCheck.IsChecked == true;
            this.IsArchive = ArchiveCheck.IsChecked == true;
            this.IsHidden = HiddenCheck.IsChecked == true;
            this.IsSystem = SystemCheck.IsChecked == true;

            DialogResult = true;
            Close();
        }

    }
}
