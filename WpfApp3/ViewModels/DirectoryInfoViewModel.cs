using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Runtime;

namespace WpfApp3.ViewModels
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; } = new ObservableCollection<FileSystemInfoViewModel>();

        public Exception? Exception { get; private set; }

        public event FileSystemEventHandler? FileSystemChanged;

        private FileSystemWatcher? _watcher;


        public bool Open(string path, DirectoryInfoViewModel? parentInfo)
        {
            bool result = false;

            try
            {
                var dirInfo = new DirectoryInfo(path);
                DirectoryInfoViewModel itemViewModel = new DirectoryInfoViewModel();
                itemViewModel.Model = dirInfo;

                if (parentInfo == null)
                {
                    Items.Add(itemViewModel);
                }
                else
                {
                    parentInfo.Items.Add(itemViewModel);
                }

                foreach (var file in dirInfo.GetFiles())
                {
                    FileInfoViewModel fileViewModel = new FileInfoViewModel();
                    fileViewModel.Model = file;
                    itemViewModel.Items.Add(fileViewModel);
                }

                foreach (var subDir in dirInfo.GetDirectories())
                {
                    Open(subDir.FullName, itemViewModel);
                }

                InitializeFileWatcher(path);

                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }

            return result;
        }

        private void InitializeFileWatcher(string path)
        {
            _watcher = new FileSystemWatcher(path);
            _watcher.IncludeSubdirectories = false;

            _watcher.Created += OnFileSystemChanged;
            _watcher.Deleted += OnFileSystemChanged;
            _watcher.Changed += OnFileSystemChanged;
            _watcher.Renamed += OnFileSystemChanged;
            _watcher.Error += Watcher_Error;
            _watcher.EnableRaisingEvents = true;
            //_watcher.IncludeSubdirectories = true;
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Exception = e.GetException();
        }
        //private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        //{
        //    FileSystemChanged?.Invoke(this, e);
        //}

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
        //                    Items.Add(new FileInfoViewModel { Model = fileInfo });
        //                }
        //                else if (Directory.Exists(e.FullPath))
        //                {
        //                    var dirInfo = new DirectoryInfo(e.FullPath);
        //                    Items.Add(new DirectoryInfoViewModel { Model = dirInfo });
        //                }
        //                break;

        //            case WatcherChangeTypes.Deleted:
        //                var itemToRemove = Items.FirstOrDefault(i => i.Model.FullName == e.FullPath);
        //                if (itemToRemove != null)
        //                    Items.Remove(itemToRemove);
        //                break;

        //            case WatcherChangeTypes.Changed:
        //            case WatcherChangeTypes.Renamed:
        //                // Można rozbudować w razie potrzeby
        //                break;
        //        }
        //    });
        //}

        private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                // Znajdź katalog, w którym nastąpiła zmiana
                string parentDir = Path.GetDirectoryName(e.FullPath);
                DirectoryInfoViewModel parentViewModel = FindDirectoryViewModel(Items, parentDir);

                if (parentViewModel != null)
                {
                    switch (e.ChangeType)
                    {
                        case WatcherChangeTypes.Created:
                            if (File.Exists(e.FullPath))
                            {
                                var fileInfo = new FileInfo(e.FullPath);
                                parentViewModel.Items.Add(new FileInfoViewModel { Model = fileInfo });
                            }
                            else if (Directory.Exists(e.FullPath))
                            {
                                var dirInfo = new DirectoryInfo(e.FullPath);
                                parentViewModel.Items.Add(new DirectoryInfoViewModel { Model = dirInfo });
                            }
                            break;

                        case WatcherChangeTypes.Deleted:
                            var itemToRemove = parentViewModel.Items.FirstOrDefault(i => i.Model.FullName == e.FullPath);
                            if (itemToRemove != null)
                                parentViewModel.Items.Remove(itemToRemove);
                            break;

                        case WatcherChangeTypes.Changed:
                        case WatcherChangeTypes.Renamed:
                            // Można rozbudować w razie potrzeby
                            break;
                    }
                }
            });
        }

        // Pomocnicza metoda do wyszukiwania katalogu w drzewiastej strukturze
        private DirectoryInfoViewModel FindDirectoryViewModel(ObservableCollection<FileSystemInfoViewModel> items, string path)
        {
            foreach (var item in items)
            {
                if (item is DirectoryInfoViewModel dirViewModel)
                {
                    if (dirViewModel.Model.FullName.Equals(path, StringComparison.OrdinalIgnoreCase))
                        return dirViewModel;

                    var foundInChild = FindDirectoryViewModel(dirViewModel.Items, path);
                    if (foundInChild != null)
                        return foundInChild;
                }
            }
            return null;
        }

    }
}
