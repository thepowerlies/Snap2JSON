using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Snap2Json.Commands;
using Snap2Json.Json;
using Snap2Json.Log;
using Snap2Json.NotifyProperty;

namespace Snap2Json.ViewModel
{
    public class MainViewModel: NotifyPropertyChanged
    {
        private string _rootFolderPath;
        private string _outputFileName;
        private bool _isIncludeHiddenFile;
        private bool _isIncludeSystemFile;
        private bool _isOpenFileWhenReady;

        private bool _isLoading;

        private string _message;
        private bool _isMessageError;
        
        
        public MainViewModel()
        {
            SelectRootPathCommand = new CommandHandler(SelectRootPath);
            CreateSnapshotCommand = new CommandHandler(CreateSnapshot);

            IsOpenFileWhenReady = true;
        }
        
        public ICommand SelectRootPathCommand { get; }
        public ICommand CreateSnapshotCommand { get; }
        
        public string OutputFilePath { get; set; }

        public string RootFolderPath
        {
            get => _rootFolderPath;
            set
            {
                _rootFolderPath = value;
                OnPropertyChanged(nameof(RootFolderPath));
            }
        }

        public string OutputFileName
        {
            get => _outputFileName;
            set
            {
                _outputFileName = value;
                OnPropertyChanged(nameof(OutputFileName));
            }
        }

        public bool IsIncludeHiddenFile
        {
            get => _isIncludeHiddenFile;
            set
            {
                _isIncludeHiddenFile = value;
                OnPropertyChanged(nameof(IsIncludeHiddenFile));
            }
        }

        public bool IsIncludeSystemFile
        {
            get => _isIncludeSystemFile;
            set
            {
                _isIncludeSystemFile = value;
                OnPropertyChanged(nameof(IsIncludeSystemFile));
            }
        }

        public bool IsOpenFileWhenReady
        {
            get => _isOpenFileWhenReady;
            set
            {
                _isOpenFileWhenReady = value;
                OnPropertyChanged(nameof(IsOpenFileWhenReady));
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public bool IsMessageError
        {
            get => _isMessageError;
            set
            {
                _isMessageError = value;
                OnPropertyChanged(nameof(IsMessageError));
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            } 
        }

        private void SelectRootPath()
        {
            using var folderBrowser = new FolderBrowserDialog();
            var result = folderBrowser.ShowDialog();
            
            if (result != DialogResult.OK) return;
            RootFolderPath = folderBrowser.SelectedPath;
            
            var outputFolderName = GetFolderNameFromPath(RootFolderPath);
            OutputFileName = $"{outputFolderName}.json";
        }

        private string GetFolderNameFromPath(string path)
        {
            var pathArray = path.Split('\\');
            return pathArray[^1];
        }

        private void CreateSnapshot()
        {
            if (!CheckSelectedPath())
            {
                ShowError("please select root folder.");
                return;
            }
            
            using var saveFolderDialog = new SaveFileDialog
            {
                Filter = "json files (*.json)|*.json",
                FileName = OutputFileName
            };

            var dialogResult = saveFolderDialog.ShowDialog();
            if(dialogResult != DialogResult.OK) return;

            OutputFilePath = saveFolderDialog.FileName;
            IsLoading = true;
            Task.Run(async ()=>
            {
                try
                {
                    var outputJson = JsonGenerator.GenerateOutput(RootFolderPath, IsIncludeHiddenFile, IsIncludeSystemFile, ShowMessage);
                    ShowMessage($"saving to {OutputFileName} ...");
                    await File.WriteAllTextAsync(OutputFilePath, outputJson);
                    ShowMessage($"Result saved to {OutputFileName}.");

                    IsLoading = false;
                    if(IsOpenFileWhenReady) OpenOutputFile();
                }
                catch (Exception e)
                {
                    IsLoading = false;
                    await Logger.LogExceptionAsync(e);
                    ShowError($"{e.Message}\nplease check log file.");
                }
            });
           
        }
        
        private bool CheckSelectedPath()
        {
            return !string.IsNullOrEmpty(RootFolderPath) && Directory.Exists(RootFolderPath);
        }

        private void OpenOutputFile()
        {
            Process.Start(@"cmd.exe",$@"/c {OutputFilePath}");
        }

        private void ShowError(string message)
        {
            IsMessageError = true;
            Message = message;
        }

        private void ShowMessage(string message)
        {
            IsMessageError = false;
            Message = message;
        }
    }
}