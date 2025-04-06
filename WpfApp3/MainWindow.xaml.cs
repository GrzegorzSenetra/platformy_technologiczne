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
using System.Windows.Forms;
using System.IO;
using Path = System.IO.Path;
using Application = System.Windows.Application;
using System.Runtime.InteropServices;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.Forms.MessageBox;
using WpfApp3.ViewModels;

enum OSINFO
{
    Windows,
    Unix,
    Unknown
}

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public System.Windows.Controls.TreeView folderTreeEl;

        public MainWindow()
        {
            InitializeComponent();
            this.folderTreeEl = FolderTree;
            this.folderTreeEl.SelectedItemChanged += FolderTree_SelectedItemChanged;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dlg = new FolderBrowserDialog() { Description = "Select directory to open" })
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var path = dlg.SelectedPath;
                    var fileExplorer = new FileExplorer();
                    fileExplorer.OpenRoot(path);
                    DataContext = fileExplorer;
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = folderTreeEl.SelectedItem as TreeViewItem;

            if (selectedItem != null && selectedItem.Tag != null)
            {
                string? path = selectedItem.Tag.ToString();

                if (IsTextFile(path))
                {
                    try
                    {
                        using (var textReader = System.IO.File.OpenText(path))
                        {
                            FileTextContentEl.Text = textReader.ReadToEnd();
                        }
                    }
                    catch (Exception ex)
                    {
                        FileTextContentEl.Text = $"Error reading file: {ex.Message}";
                    }
                }
                else
                {
                    FileTextContentEl.Text = "Selected item is not a text file.";
                }
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = FolderTree.SelectedItem as TreeViewItem;

            if (selectedItem == null)
            {
                MessageBox.Show("Please select a directory to create in.", "Warning", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Warning);
                return;
            }

            string? selectedPath = selectedItem.Tag.ToString();
            if (!Directory.Exists(selectedPath))
            {
                MessageBox.Show("Selected item is not a valid directory.", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                return;
            }

            CreateDialog dialog = new CreateDialog(selectedPath);
            if (dialog.ShowDialog() == true)
            {
                string newPath = dialog.FilePath;
                bool isDirectory = dialog.IsDirectory;

                try
                {
                    if (isDirectory)
                    {

                        DirectoryInfo dir = Directory.CreateDirectory(newPath);
                        
                        if (dialog.IsReadOnly)
                        {
                            dir.Attributes |= FileAttributes.ReadOnly;
                        }
                        if (dialog.IsArchive)
                        {
                            dir.Attributes |= FileAttributes.Archive;
                        }
                        if (dialog.IsHidden)
                        {
                            dir.Attributes |= FileAttributes.Hidden;
                        }
                        if (dialog.IsSystem)
                        {
                            dir.Attributes |= FileAttributes.System;
                        }
                    }
                    else
                    {
                        File.Create(newPath).Close();
                        FileInfo newFileInfo = new FileInfo(newPath);
                        if (dialog.IsReadOnly)
                        {
                            newFileInfo.Attributes |= FileAttributes.ReadOnly;
                        }
                        if (dialog.IsArchive)
                        {
                            newFileInfo.Attributes |= FileAttributes.Archive;
                        }
                        if (dialog.IsHidden)
                        {
                            newFileInfo.Attributes |= FileAttributes.Hidden;
                        }
                        if (dialog.IsSystem)
                        {
                            newFileInfo.Attributes |= FileAttributes.System;
                        }
                    }

                    TreeViewItem newItem = new TreeViewItem
                    {
                        Header = Path.GetFileName(newPath),
                        Tag = newPath
                    };
                    selectedItem.Items.Add(newItem);
                    selectedItem.IsExpanded = true;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error creating: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = FolderTree.SelectedItem as TreeViewItem;

            if (selectedItem != null && selectedItem.Tag != null)
            {
                string? path = selectedItem.Tag.ToString();

                if (File.Exists(path))
                {
                    if (IsReadOnlyOrProtected(path))
                    {
                        MessageBox.Show("The selected file is read-only or protected and cannot be deleted.", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                        return;
                    }
                    try
                    {
                        File.Delete(path);

                        var parent = selectedItem.Parent as TreeViewItem;
                        if (parent != null)
                        {
                            parent.Items.Remove(selectedItem);
                        }
                        else
                        {
                            FolderTree.Items.Remove(selectedItem);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting file: {ex.Message}", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(path + " is not a file or does not exist", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"No item is selected.", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
            }
        }

        private bool IsReadOnlyOrProtected(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.IsReadOnly || (fileInfo.Attributes & FileAttributes.System) == FileAttributes.System;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking file attributes: {ex.Message}");
            }
        }

        private void DeleteDirectory_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = FolderTree.SelectedItem as TreeViewItem;

            if (selectedItem != null && selectedItem.Tag != null)
            {
                string? path = selectedItem.Tag.ToString();

                if (Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path, true);

                        var parent = selectedItem.Parent as TreeViewItem;
                        if (parent != null)
                        {
                            parent.Items.Remove(selectedItem);
                        }
                        else
                        {
                            FolderTree.Items.Remove(selectedItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting directory: {ex.Message}", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(path + " is not a directory or does not exist", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"No item is selected.", "Error", (MessageBoxButtons)MessageBoxButton.OK, (MessageBoxIcon)MessageBoxImage.Error);
            }
        }

        //private void LoadDirectory(string dir, TreeViewItem? parentItem)
        //{
        //    var directoryInfo = new DirectoryInfo(dir);
        //    var directoryItem = new TreeViewItem
        //    {
        //        Header = directoryInfo.Name,
        //        Tag = directoryInfo.FullName
        //    };

        //    if (parentItem == null)
        //    {
        //        this.folderTreeEl.Items.Add(directoryItem);
        //    }
        //    else
        //    {
        //        parentItem.Items.Add(directoryItem);
        //    }

        //    foreach (var file in directoryInfo.GetFiles())
        //    {
        //        directoryItem.Items.Add(new TreeViewItem
        //        {
        //            Header = file.Name,
        //            Tag = file.FullName
        //        });
        //    }

        //    foreach (var subDir in directoryInfo.GetDirectories())
        //    {
        //        LoadDirectory(subDir.FullName, directoryItem);
        //    }
        //}

        private void SortTreeViewItems(TreeViewItem parentItem)
        {
            var itemsList = new List<TreeViewItem>();

            foreach (TreeViewItem item in parentItem.Items)
            {
                itemsList.Add(item);
            }

            foreach (var item in itemsList)
            {
                if (item.Items.Count > 0)
                    SortTreeViewItems(item);
            }

            var sorted = itemsList
                .OrderByDescending(i => i.Items.Count > 0) 
                .ThenBy(i => i.Header.ToString(), StringComparer.InvariantCultureIgnoreCase)
                .ToList();

            parentItem.Items.Clear();
            foreach (var item in sorted)
            {
                parentItem.Items.Add(item);
            }
        }

        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem != null && selectedItem.Tag != null)
            {
                string? path = selectedItem.Tag.ToString();

                OSINFO osInfo = GetOperatingSystemInfo();

                UpdateAccessibilityInfo(path, osInfo);

                if (IsTextFile(path))
                {
                    selectedItem.ContextMenu = (ContextMenu)FolderTree.Resources["TextFileContextMenu"];
                }
                else if (Directory.Exists(path))
                {
                    selectedItem.ContextMenu = (ContextMenu)FolderTree.Resources["FolderContextMenu"];
                }

            }
        }

        private bool IsTextFile(string? filePath)
        {
            try
            {
                // Open the file in binary mode
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Read a small portion of the file to analyze its content
                    byte[] buffer = new byte[512];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    // Check for non-text characters in the buffer
                    for (int i = 0; i < bytesRead; i++)
                    {
                        // If the byte is outside the printable ASCII range and not whitespace,
                        // it is likely a binary file
                        if (buffer[i] < 32 && buffer[i] != 9 && buffer[i] != 10 && buffer[i] != 13)
                        {
                            return false;
                        }
                    }
                }

                // If no binary characters were found, assume it's a text file
                return true;
            }
            catch
            {
                // If an error occurs (e.g., access denied), assume it's not a text file
                return false;
            }
        }

        private OSINFO GetOperatingSystemInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSINFO.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) 
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX) 
                || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            {
                return OSINFO.Unix;
            }

            return OSINFO.Unknown;
        }

        private void UpdateAccessibilityInfo(string path, OSINFO osInfo)
        {
            try
            {
                char[] accessInfo = new char[4] { '-', '-', '-', '-' };

                if (osInfo == OSINFO.Windows)
                {
                    FileAttributes attributes = File.GetAttributes(path);

                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        accessInfo[0] = 'r';  // ReadOnly
                    }

                    if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                    {
                        accessInfo[1] = 'a';  // Archive
                    }

                    if ((attributes & FileAttributes.System) == FileAttributes.System)
                    {
                        accessInfo[2] = 's';  // System
                    }

                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        accessInfo[3] = 'h';  // Hidden
                    }


                    // if its directory
                    if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        accessInfo[0] = 'd';
                    }

                }
                else if (osInfo == OSINFO.Unix)
                {
                    FileInfo fileInfo = new FileInfo(path);

                    if (!fileInfo.IsReadOnly)
                    {
                        accessInfo[0] = 'r';  // Readable
                    }

                    // archive
                    if ((fileInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
                    {
                        accessInfo[1] = 'a'; // Archive
                    }

                    if (Path.GetFileName(path).StartsWith("."))
                    {
                        accessInfo[1] = 'h'; // Hidden
                    }

                    if ((fileInfo.Attributes & FileAttributes.Directory) == 0 &&
                        (fileInfo.Attributes & FileAttributes.Normal) == FileAttributes.Normal)
                    {
                        if ((fileInfo.Exists && (fileInfo.Attributes & FileAttributes.ReadOnly) == 0))
                        {
                            accessInfo[2] = 'x'; // Executable
                        }
                    }
                }
                else
                {
                    throw new Exception("Error, system type not recognized!");
                }

                string attrString = new string(accessInfo);

                AccessibilityNote.Text = $"{attrString}";

            }
            catch (Exception ex)
            {
                AccessibilityNote.Text = $"Error checking attributes: {ex.Message}";
            }
        }


    }
}