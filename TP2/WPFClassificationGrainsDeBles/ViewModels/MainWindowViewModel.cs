using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using WPFClassificationGrainsDeBles.Models;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SharedModel _sharedModel = new SharedModel();

        // ========== AFFICHAGE ==========
        private string _currentTitle = "Bienvenue";
        private bool _showImport = true;
        private bool _showKConfig = false;
        private bool _showDistanceConfig = false;
        private bool _showTrainTest = false;
        private string _importStatus = "Aucun fichier importé";
        private string _resultText = "Les résultats apparaîtront ici...";

        // ========== FICHIERS ==========
        private string _trainPath = "";
        private string _testPath = "";

        // ========== CONFIGURATION ==========
        private int _kValue = 3;
        private bool _isEuclidienne = true;
        private bool _isManhattan = false;

        // ========== HISTORIQUE ==========
        public ObservableCollection<string> HistoriqueList { get; set; } = new ObservableCollection<string>();

        // ========== PROPRIETES ==========
        public string CurrentTitle
        {
            get => _currentTitle;
            set { _currentTitle = value; OnPropertyChanged(); }
        }

        public bool ShowImport
        {
            get => _showImport;
            set { _showImport = value; OnPropertyChanged(); }
        }

        public bool ShowKConfig
        {
            get => _showKConfig;
            set { _showKConfig = value; OnPropertyChanged(); }
        }

        public bool ShowDistanceConfig
        {
            get => _showDistanceConfig;
            set { _showDistanceConfig = value; OnPropertyChanged(); }
        }

        public bool ShowTrainTest
        {
            get => _showTrainTest;
            set { _showTrainTest = value; OnPropertyChanged(); }
        }

        public string ImportStatus
        {
            get => _importStatus;
            set { _importStatus = value; OnPropertyChanged(); }
        }

        public string ResultText
        {
            get => _resultText;
            set { _resultText = value; OnPropertyChanged(); }
        }

        public string TrainPath
        {
            get => _trainPath;
            set { _trainPath = value; OnPropertyChanged(); }
        }

        public string TestPath
        {
            get => _testPath;
            set { _testPath = value; OnPropertyChanged(); }
        }

        public int KValue
        {
            get => _kValue;
            set
            {
                if (value < 1) value = 1;
                if (value > 30) value = 30;
                _kValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsEuclidienne
        {
            get => _isEuclidienne;
            set
            {
                _isEuclidienne = value;
                if (value) _isManhattan = false;
                OnPropertyChanged();
            }
        }

        public bool IsManhattan
        {
            get => _isManhattan;
            set
            {
                _isManhattan = value;
                if (value) _isEuclidienne = false;
                OnPropertyChanged();
            }
        }

        // ========== COMMANDES ==========
        public ICommand BrowseTrainCommand { get; }
        public ICommand BrowseTestCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ShowKConfigCommand { get; }
        public ICommand ShowDistanceConfigCommand { get; }
        public ICommand ValidateKCommand { get; }
        public ICommand ValidateDistanceCommand { get; }
        public ICommand TrainCommand { get; }
        public ICommand TestCommand { get; }
        public ICommand QuitCommand { get; }

        // ========== CONSTRUCTEUR ==========
        public MainWindowViewModel()
        {
            // Initialisation des commandes
            BrowseTrainCommand = new RelayCommand(BrowseTrain);
            BrowseTestCommand = new RelayCommand(BrowseTest);
            ImportCommand = new RelayCommand(async () => await ImportData());
            ShowKConfigCommand = new RelayCommand(ShowKConfigSection);
            ShowDistanceConfigCommand = new RelayCommand(ShowDistanceConfigSection);
            ValidateKCommand = new RelayCommand(ValidateK);
            ValidateDistanceCommand = new RelayCommand(ValidateDistance);
            TrainCommand = new RelayCommand(Train);
            TestCommand = new RelayCommand(Test);
            QuitCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());
        }

        // ========== METHODES ==========
        private void ShowOnly(string section)
        {
            ShowImport = (section == "import");
            ShowKConfig = (section == "k");
            ShowDistanceConfig = (section == "distance");
            ShowTrainTest = (section == "traintest");
        }

        private void BrowseTrain()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Sélectionner le fichier train.csv",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
                TrainPath = dialog.FileName;
        }

        private void BrowseTest()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Sélectionner le fichier test.csv",
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
                TestPath = dialog.FileName;
        }

        private async System.Threading.Tasks.Task ImportData()
        {
            if (string.IsNullOrEmpty(TrainPath) || string.IsNullOrEmpty(TestPath))
            {
                ImportStatus = "❌ Veuillez sélectionner les deux fichiers!";
                return;
            }

            try
            {
                var trainGrains = DataConverter.ConversionListe(TrainPath);
                _sharedModel.TrainingData = new EnsembleDonnees();
                DataConverter.SaveEchantillon(trainGrains, _sharedModel.TrainingData);

                var testGrains = DataConverter.ConversionListe(TestPath);
                _sharedModel.TestData = new EnsembleDonnees();
                DataConverter.SaveEchantillon(testGrains, _sharedModel.TestData);

                _sharedModel.IsDataLoaded = true;
                _sharedModel.InitializeClassifier();

                ImportStatus = $"✅ Succès: {_sharedModel.TrainingData.Taille()} train, {_sharedModel.TestData.Taille()} test";
                ShowOnly("traintest");
                CurrentTitle = "📊 Données chargées - Configurez k et distance";
            }
            catch (Exception ex)
            {
                ImportStatus = $"❌ Erreur: {ex.Message}";
                _sharedModel.IsDataLoaded = false;
            }
        }

        private void ShowKConfigSection()
        {
            ShowOnly("k");
            CurrentTitle = "🔢 2. Choisir la valeur de k (1 à 30)";
        }

        private void ShowDistanceConfigSection()
        {
            ShowOnly("distance");
            CurrentTitle = "📏 3. Choisir la distance";
        }

        private void ValidateK()
        {
            _sharedModel.K = KValue;
            ShowOnly("distance");
            CurrentTitle = "📏 3. Choisir la distance";
            ResultText = $"✅ k = {KValue} validé. Choisissez maintenant la distance.";
        }

        private void ValidateDistance()
        {
            string distance = IsEuclidienne ? "Euclidienne" : "Manhattan";
            _sharedModel.SetDistance(distance);
            ShowOnly("traintest");
            ResultText = $"✅ Distance {distance} validée. Vous pouvez maintenant entraîner et tester.";
            CurrentTitle = "🚂 4. Entraîner et 5. Tester le modèle";
        }

        private void Train()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                ResultText = "❌ Erreur: Veuillez d'abord importer les données!";
                return;
            }

            _sharedModel.InitializeClassifier();
            ResultText = $"✅ Modèle entraîné avec succès!\n" +
                        $"   📊 k = {_sharedModel.K}\n" +
                        $"   📏 Distance = {_sharedModel.GetDistanceName()}\n" +
                        $"   📈 Données d'entraînement = {_sharedModel.TrainingData.Taille()} échantillons";
        }

        private void Test()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                ResultText = "❌ Erreur: Veuillez d'abord importer les données!";
                return;
            }

            if (_sharedModel.Classifier == null)
            {
                ResultText = "❌ Erreur: Veuillez d'abord entraîner le modèle!";
                return;
            }

            try
            {
                var evaluation = new EvaluationPerformance();
                evaluation.Evaluer(_sharedModel.K, _sharedModel.DistanceStrategy,
                                   _sharedModel.TrainingData, _sharedModel.TestData);

                double accuracy = evaluation.CalculerAccuracy() * 100;
                string distanceName = _sharedModel.GetDistanceName();

                string resultat = $"╔════════════════════════════════════════════════════╗\n" +
                                 $"║                 RÉSULTAT DU TEST                   ║\n" +
                                 $"╚════════════════════════════════════════════════════╝\n\n" +
                                 $"📊 Paramètres du classifieur :\n" +
                                 $"   • k = {_sharedModel.K}\n" +
                                 $"   • Distance = {distanceName}\n" +
                                 $"   • Données test = {_sharedModel.TestData.Taille()} échantillons\n\n" +
                                 $"🎯 Précision (Accuracy) = {accuracy:F2}%\n";

                ResultText = resultat;

                // Sauvegarde dans l'historique
                string dateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string historiqueEntry = $"{dateStr} | k={_sharedModel.K} | {distanceName} | Accuracy={accuracy:F2}%";
                HistoriqueList.Insert(0, historiqueEntry);

                // Sauvegarde dans le fichier JSON
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "historique.json");
                evaluation.SauvegarderJsonGlobal(jsonPath, _sharedModel.K, _sharedModel.DistanceStrategy,
                                                  _sharedModel.TrainingData, _sharedModel.TestData);
            }
            catch (Exception ex)
            {
                ResultText = $"❌ Erreur lors du test: {ex.Message}";
            }
        }

        // ========== INotifyPropertyChanged ==========
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}