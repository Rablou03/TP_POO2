// ViewModels/ClassifierConfigViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ClassificationGrainsDeBle;

namespace ClassificationGrainsDeBle_WPF.ViewModels
{
    public class ClassifierConfigViewModel : INotifyPropertyChanged
    {
        private int _kValue = 3;
        private string _selectedDistance = "Euclidienne";
        private readonly string[] _distanceOptions = { "Euclidienne", "Manhattan" };

        public event PropertyChangedEventHandler? PropertyChanged;

        public ClassifierConfigViewModel()
        {
            ApplyConfigCommand = new RelayCommand(_ => ApplyConfiguration());
        }

        public ICommand ApplyConfigCommand { get; }

        public int KValue
        {
            get => _kValue;
            set
            {
                if (value >= 1 && value <= 50)
                {
                    _kValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string[] DistanceOptions => _distanceOptions;

        public string SelectedDistance
        {
            get => _selectedDistance;
            set { _selectedDistance = value; OnPropertyChanged(); }
        }

        public IDistance GetDistanceInstance()
        {
            return SelectedDistance == "Euclidienne" 
                ? new DistanceEuclidienne() 
                : new DistanceManhattan();
        }

        private void ApplyConfiguration()
        {
            var distance = GetDistanceInstance();
            // Stocker la configuration dans une classe static ou un service
            ClassifierConfig.Current = new ClassifierConfig
            {
                K = KValue,
                Distance = distance,
                DistanceName = SelectedDistance
            };
            
            System.Windows.MessageBox.Show(
                $"Configuration appliquée :\nK = {KValue}\nDistance = {SelectedDistance}",
                "Configuration", 
                System.Windows.MessageBoxButton.OK, 
                System.Windows.MessageBoxImage.Information);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Classe statique pour partager la configuration entre ViewModels
    public static class ClassifierConfig
    {
        public static int K { get; set; } = 3;
        public static IDistance Distance { get; set; } = new DistanceEuclidienne();
        public static string DistanceName { get; set; } = "Euclidienne";
        public static EnsembleDonnees? TrainingData { get; set; }
        public static EnsembleDonnees? TestData { get; set; }
        public static bool IsDataLoaded => TrainingData?.Taille() > 0 && TestData?.Taille() > 0;
    }
}