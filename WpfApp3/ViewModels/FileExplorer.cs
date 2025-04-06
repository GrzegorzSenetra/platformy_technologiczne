using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Entities;

namespace WpfApp3.ViewModels
{
    public class FileExplorer : ViewModelBase
    {
        public DirectoryInfoViewModel Root { get; set; }

        //public ObservableCollection<FileSystemInfoViewModel> Items { get; } = new();

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path, null);
            //Root.FileSystemChanged += OnFileSystemChanged;
        }

        //private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        //{
        //    App.Current.Dispatcher.Invoke(() =>
        //    {
        //        switch (e.ChangeType)
        //        {
        //            case WatcherChangeTypes.Created:
        //                if (File.Exists(e.FullPath))
        //                {
        //                    var fileInfo = new FileInfo(e.FullPath);
        //                    Root.Items.Add(new FileInfoViewModel { Model = fileInfo });
        //                }
        //                else if (Directory.Exists(e.FullPath))
        //                {
        //                    var dirInfo = new DirectoryInfo(e.FullPath);
        //                    Root.Items.Add(new DirectoryInfoViewModel { Model = dirInfo });
        //                }
        //                break;

        //            case WatcherChangeTypes.Deleted:
        //                var itemToRemove = Root.Items.FirstOrDefault(i => i.Model.FullName == e.FullPath);
        //                if (itemToRemove != null)
        //                    Root.Items.Remove(itemToRemove);
        //                break;

        //            case WatcherChangeTypes.Changed:
        //            case WatcherChangeTypes.Renamed:
        //                // Można rozbudować w razie potrzeby
        //                break;
        //        }
        //    });
        //}


    }
}
