using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Entities;

namespace WpfApp3.ViewModels
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
        public DateTime LastWriteTime
        {
            get { return _lastWriteTime; }
            set
            {
                if (_lastWriteTime != value)
                {
                    _lastWriteTime = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public FileSystemInfo Model
        {
            get { return _fileSystemInfo; }
            set
            {
                if (_fileSystemInfo != value)
                {
                    _fileSystemInfo = value;
                    this.LastWriteTime = value.LastWriteTime;
                    this.Caption = value.Name;
                    NotifyPropertyChanged();
                }
            }
        }

        

        private DateTime _lastWriteTime;
        private FileSystemInfo _fileSystemInfo;
        private string _caption;
    }
}
