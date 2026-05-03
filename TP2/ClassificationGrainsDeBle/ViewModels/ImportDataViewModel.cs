// ViewModels/ImportDataViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ClassificationGrainsDeBle; // Référence vers les modèles existants

namespace ClassificationGrainsDeBle_WPF.ViewModels
{
    public class ImportDataViewModel : INotifyPropertyChanged
    {
        private string _trainFilePath = string.Empty;
        private string _testFilePath = string.Empty;
        private ObservableCollection<Grain> _trainGrains = new();
        private ObservableCollection<Grain> _testGrains = new();
        private bool _isTrainLoaded = false;
        private bool _isTestLoaded = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ImportDataViewModel()
        {
            LoadTrainCommand = new RelayCommand(_ => LoadTrainFile());
            LoadTestCommand = new RelayCommand(_ => LoadTestFile());
            ValidateDataCommand = new RelayCommand(_ => ValidateData(), _ => IsTrainLoaded && IsTestLoaded);
        }

        public ICommand LoadTrainCommand { get; }
        public ICommand LoadTestCommand { get; }
        public ICommand ValidateDataCommand { get; }

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

        public ObservableCollection<Grain> TrainGrains
        {
            get => _trainGrains;
            set { _trainGrains = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Grain> TestGrains
        {
            get => _testGrains;
            set { _testGrains = value; OnPropertyChanged(); }
        }

        public bool IsTrainLoaded
        {
            get => _isTrainLoaded;
            set { _isTrainLoaded = value; OnPropertyChanged(); }
        }

        public bool IsTestLoaded
        {
            get => _isTestLoaded;
            set { _isTestLoaded = value; OnPropertyChanged(); }
        }

        private void LoadTrainFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Sélectionner le fichier d'entraînement"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    TrainFilePath = dialog.FileName;
                    var grains = Convert.conversion_liste(TrainFilePath);
                    TrainGrains.Clear();
                    foreach (var grain in grains)
                        TrainGrains.Add(grain);
                    
                    IsTrainLoaded = true;
                    MessageBox.Show($"Fichier train chargé avec succès!\n{grains.Count} grains trouvés.", 
                        "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement du fichier train : {ex.Message}", 
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadTestFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Sélectionner le fichier de test"
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    TestFilePath = dialog.FileName;
                    var grains = Convert.conversion_liste(TestFilePath);
                    TestGrains.Clear();
                    foreach (var grain in grains)
                        TestGrains.Add(grain);
                    
                    IsTestLoaded = true;
                    MessageBox.Show($"Fichier test chargé avec succès!\n{grains.Count} grains trouvés.", 
                        "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors du chargement du fichier test : {ex.Message}", 
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ValidateData()
        {
            MessageBox.Show("Données validées avec succès! Vous pouvez maintenant configurer le classifieur.",
                "Validation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}