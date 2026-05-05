using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class ImportDataViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainVM;
        private string _trainFilePath = "";
        private string _testFilePath = "";
        private string _status = "Sélectionnez les fichiers Train.csv et Test.csv";
        private bool _isImporting = false;

        public string TrainFilePath
        {
            get => _trainFilePath;
            set { _trainFilePath = value; OnPropertyChanged(); }
        }

        public string TestFilePath
        {
            get => _testFilePath;
            set { _testFilePath = value; OnPropertyChanged(); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public bool IsImporting
        {
            get => _isImporting;
            set { _isImporting = value; OnPropertyChanged(); }
        }

        public ICommand SelectTrainCommand { get; }
        public ICommand SelectTestCommand { get; }
        public ICommand ImportCommand { get; }

        // Constructeur avec paramètre
        public ImportDataViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            SelectTrainCommand = new RelayCommand(SelectTrainFile);
            SelectTestCommand = new RelayCommand(SelectTestFile);
            ImportCommand = new RelayCommand(async () => await ImportFilesAsync());
        }

        private void SelectTrainFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Sélectionner train.csv",
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                TrainFilePath = dialog.FileName;
                UpdateStatus();
            }
        }

        private void SelectTestFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Sélectionner test.csv",
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                TestFilePath = dialog.FileName;
                UpdateStatus();
            }
        }

        private void UpdateStatus()
        {
            if (!string.IsNullOrEmpty(TrainFilePath) && !string.IsNullOrEmpty(TestFilePath))
                Status = "✅ Fichiers sélectionnés. Cliquez sur Importer.";
            else
                Status = "Sélectionnez les deux fichiers CSV";
        }

        private async Task ImportFilesAsync()
        {
            if (string.IsNullOrEmpty(TrainFilePath) || string.IsNullOrEmpty(TestFilePath))
            {
                Status = "❌ Veuillez sélectionner les deux fichiers!";
                return;
            }

            IsImporting = true;
            await _mainVM.ImportDataAsync(TrainFilePath, TestFilePath);
            Status = _mainVM.ImportStatus;
            IsImporting = false;
        }
    }
}