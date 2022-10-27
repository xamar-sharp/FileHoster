using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using FileHoster.Commands;
using FileHoster.ViewModels;
using FileHoster.Services;
using System.Runtime.CompilerServices;
using System.ComponentModel;
namespace FileHoster.ViewModels
{
    public sealed class PageViewModel:INotifyPropertyChanged
    {
        private static readonly SignInDialog _dialog = new SignInDialog();
        private ObservableCollection<string> _files;
        public ObservableCollection<string> Files { get => _files; set {
                _files = value;
                OnPropertyChanged();
            } }
        public ICommand GetFileCommand { get; set; }
        public ICommand GetFilesCommand { get; set; }
        public ICommand PostFileCommand { get; set; }
        public ICommand SignInCommand { get; set; }
        public ICommand SaveAsCommand { get; set; }
        public ICommand SelectFileCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        private string _remoteFileName;
        private string _localFileName;
        private string _currentFileName;
        private long _currentFileLength;
        public string RemoteFileName { get { return _remoteFileName; } set{ _remoteFileName = value;OnPropertyChanged(); } }
        public string LocalFileName { get { return _localFileName; } set { _localFileName = value;OnPropertyChanged(); } }
        public string CurrentFileName { get { return _currentFileName; } set { _currentFileName = value;OnPropertyChanged(); } }
        public long CurrentFileLength { get { return _currentFileLength; } set { _currentFileLength = value; OnPropertyChanged(); } }
        internal Models.FileModel _currentModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string name=default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public PageViewModel()
        {
            _files = new ObservableCollection<string>();
            GetFileCommand = new Command(async (obj) =>
            {
                _currentModel = new Models.FileModel() { Data = await WebAPI.DownloadFile(obj as string), Extension = Path.GetExtension(obj as string), FileName = obj as string, UserName = App.UserName };
                CurrentFileName = obj as string;
                CurrentFileLength = _currentModel.Data.Length;
            }, (obj) => !string.IsNullOrEmpty(obj as string));
            GetFilesCommand = new Command(async (obj) =>
            {
                var files = (await WebAPI.GetFiles()).Select(ent => ent.FileName).ToList();
                Files = new ObservableCollection<string>(files);
            }, (obj) =>
            {
                return true;
            });
            PostFileCommand = new Command(async (obj) =>
            {
                if (!await WebAPI.PostFile(new Models.FileModel()
                {
                    Data = await System.IO.File.ReadAllBytesAsync(LocalFileName),
                    Extension =
                    Path.GetExtension(LocalFileName),
                    FileName = Path.GetFileNameWithoutExtension(RemoteFileName),
                    UserName = App.UserName
                }))
                {
                    System.Windows.MessageBox.Show(Resource.APIError, Resource.Message, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }, (obj) =>
            {
                return !string.IsNullOrEmpty(LocalFileName) && !string.IsNullOrEmpty(RemoteFileName); 
            });
            SignInCommand = new Command(async (obj) =>
            {
                _dialog.ShowDialog();
            },(obj)=>true);
            SaveAsCommand = new Command(async (obj) =>
            {
                SaveFileDialog saveAs = new SaveFileDialog() { ValidateNames = true, OverwritePrompt = true, AddExtension = true, InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Title = Resource.SaveAs, Filter = Resource.AllFiles + "|*.*" };
                if (saveAs.ShowDialog() == true)
                {
                    await File.WriteAllBytesAsync(saveAs.FileName, _currentModel.Data);
                }
            }, (obj) => _currentModel != null);
            SelectFileCommand = new Command(async (obj) =>
            {
                OpenFileDialog saveAs = new OpenFileDialog() { ValidateNames = true, Multiselect = true, AddExtension = true, InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Title = Resource.SaveAs, Filter = Resource.AllFiles + "|*.*" };
                if (saveAs.ShowDialog() == true)
                {
                    LocalFileName = saveAs.FileName;
                }
            }, (obj)=>true);
            DeleteCommand = new Command(async (obj) =>
            {
                if (!await WebAPI.DeleteFile(CurrentFileName))
                {
                    System.Windows.MessageBox.Show(Resource.APIError, Resource.Message, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }, (obj) => !string.IsNullOrEmpty(CurrentFileName));
        }

    }
}
