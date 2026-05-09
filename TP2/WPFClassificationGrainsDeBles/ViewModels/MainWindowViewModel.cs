using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WPFClassificationGrainsDeBles.Models;
using WPFClassificationGrainsDeBles.Services;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SharedModel _sharedModel = new SharedModel();
        private readonly UtilisateurService _utilisateurService = new UtilisateurService();
        private Utilisateur _utilisateurSelectionne;

        public ObservableCollection<Utilisateur> Utilisateurs { get; set; } = new ObservableCollection<Utilisateur>();

        public Utilisateur UtilisateurSelectionne
        {
            get => _utilisateurSelectionne;
            set { _utilisateurSelectionne = value; OnPropertyChanged(); }
        }

        // Affichage de notre programme
        private string _currentTitle = "Bienvenue sur l'application de Classification des grains de blé - k-NN";
        private Visibility _showWelcome = Visibility.Visible;
        private Visibility _showImport = Visibility.Collapsed;
        private Visibility _showKConfig = Visibility.Collapsed;
        private Visibility _showDistanceConfig = Visibility.Collapsed;
        private Visibility _showTrain = Visibility.Collapsed;
        private Visibility _showTest = Visibility.Collapsed;
        private Visibility _showResults = Visibility.Collapsed;
        private Visibility _showHistory = Visibility.Collapsed;
        private string _importStatus = "Aucun fichier importé";
        private string _resultText = "Les résultats s'afficheront dans cette section.";
        private string _currentConfigText = "Aucune configuration";

        // Déclaration des fichiers
        private string _trainPath = "";
        private string _testPath = "";

        // Configuration distance avec k par defaut = 3
        private int _kValue = 3;
        private bool _isEuclidienne = true;
        private bool _isManhattan = false;
        private bool _kValidated = false;
        private bool _distanceValidated = false;

        // Historique
        public ObservableCollection<string> HistoriqueList { get; set; } = new ObservableCollection<string>();

        // Propriétés
        public string CurrentTitle
        {
            get => _currentTitle;
            set { _currentTitle = value; OnPropertyChanged(); }
        }

        public Visibility ShowWelcome
        {
            get => _showWelcome;
            set { _showWelcome = value; OnPropertyChanged(); }
        }

        public Visibility ShowImport
        {
            get => _showImport;
            set { _showImport = value; OnPropertyChanged(); }
        }

        public Visibility ShowKConfig
        {
            get => _showKConfig;
            set { _showKConfig = value; OnPropertyChanged(); }
        }

        public Visibility ShowDistanceConfig
        {
            get => _showDistanceConfig;
            set { _showDistanceConfig = value; OnPropertyChanged(); }
        }

        public Visibility ShowTrain
        {
            get => _showTrain;
            set { _showTrain = value; OnPropertyChanged(); }
        }

        public Visibility ShowTest
        {
            get => _showTest;
            set { _showTest = value; OnPropertyChanged(); }
        }

        public Visibility ShowResults
        {
            get => _showResults;
            set { _showResults = value; OnPropertyChanged(); }
        }

        public Visibility ShowHistory
        {
            get => _showHistory;
            set { _showHistory = value; OnPropertyChanged(); }
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

        public string CurrentConfigText
        {
            get => _currentConfigText;
            set { _currentConfigText = value; OnPropertyChanged(); }
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

        // Commandes
        public ICommand BrowseTrainCommand { get; }
        public ICommand BrowseTestCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand ValidateKAndContinueCommand { get; }
        public ICommand ValidateDistanceAndContinueCommand { get; }
        public ICommand TrainCommand { get; }
        public ICommand TestCommand { get; }
        public ICommand QuitCommand { get; }
        public ICommand ChargerUtilisateursCommand { get; }

        // Commandes de navigation
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateImportCommand { get; }
        public ICommand NavigateKConfigCommand { get; }
        public ICommand NavigateDistanceConfigCommand { get; }
        public ICommand NavigateResultsCommand { get; }
        public ICommand NavigateHistoryCommand { get; }

        // Constructeur
        public MainWindowViewModel()
        {
            // Initialisation des commandes
            BrowseTrainCommand = new RelayCommand(() => BrowseTrain());
            BrowseTestCommand = new RelayCommand(() => BrowseTest());
            ImportCommand = new RelayCommand(async () => await ImportData());
            ValidateKAndContinueCommand = new RelayCommand(() => ValidateKAndContinue());
            ValidateDistanceAndContinueCommand = new RelayCommand(() => ValidateDistanceAndContinue());
            TrainCommand = new RelayCommand(() => Train());
            TestCommand = new RelayCommand(() => Test());
            QuitCommand = new RelayCommand(() => Application.Current.Shutdown());
            ChargerUtilisateursCommand = new RelayCommand(async () => await ChargerUtilisateurs());

            // Commandes de navigation
            NavigateHomeCommand = new RelayCommand(() => ShowHome());
            NavigateImportCommand = new RelayCommand(() => ShowImportSection());
            NavigateKConfigCommand = new RelayCommand(() => ShowKConfigSection());
            NavigateDistanceConfigCommand = new RelayCommand(() => ShowDistanceConfigSection());
            NavigateResultsCommand = new RelayCommand(() => ShowResultsSection());
            NavigateHistoryCommand = new RelayCommand(() => ShowHistorySection());
        }

        // Méthode chargement utilisateurs API
        private async Task ChargerUtilisateurs()
        {
            try
            {
                var liste = await _utilisateurService.GetUtilisateursAsync();
                Utilisateurs.Clear();
                foreach (var u in liste)
                    Utilisateurs.Add(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur chargement utilisateurs: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Méthodes de navigation
        private void ShowHome()
        {
            ShowWelcome = Visibility.Visible;
            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "Bienvenue sur l'application de Classification des grains de blé - k-NN";
        }

        private void ShowImportSection()
        {
            ShowWelcome = Visibility.Collapsed;
            ShowImport = Visibility.Visible;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "1. Importer les données";
        }

        private void ShowKConfigSection()
        {
            ShowWelcome = Visibility.Collapsed;

            if (!_sharedModel.IsDataLoaded)
            {
                MessageBox.Show("Veuillez d'abord importer les données.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ShowImportSection();
                return;
            }

            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Visible;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "2. Configurer la valeur de k";
        }

        private void ShowDistanceConfigSection()
        {
            ShowWelcome = Visibility.Collapsed;

            if (!_kValidated)
            {
                MessageBox.Show("Veuillez d'abord valider la valeur de k.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                ShowKConfigSection();
                return;
            }

            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Visible;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "3. Choisir la distance";
        }

        private void ShowTrainTestSections()
        {
            ShowWelcome = Visibility.Collapsed;
            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Visible;
            ShowTest = Visibility.Visible;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "4. Entraîner et 5. Tester le modèle";
        }

        private void ShowResultsSection()
        {
            ShowWelcome = Visibility.Collapsed;
            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Visible;
            ShowHistory = Visibility.Collapsed;
            CurrentTitle = "Résultats de classification";
        }

        private void ShowHistorySection()
        {
            ShowWelcome = Visibility.Collapsed;
            ShowImport = Visibility.Collapsed;
            ShowKConfig = Visibility.Collapsed;
            ShowDistanceConfig = Visibility.Collapsed;
            ShowTrain = Visibility.Collapsed;
            ShowTest = Visibility.Collapsed;
            ShowResults = Visibility.Collapsed;
            ShowHistory = Visibility.Visible;
            CurrentTitle = "Historique des expériences";
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
                ImportStatus = "Veuillez sélectionner les deux fichiers!";
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

                ImportStatus = $"Succès: {_sharedModel.TrainingData.Taille()} train, {_sharedModel.TestData.Taille()} test";

                _kValidated = false;
                _distanceValidated = false;

                ShowKConfigSection();
            }
            catch (Exception ex)
            {
                ImportStatus = $"Erreur: {ex.Message}";
                _sharedModel.IsDataLoaded = false;
            }
        }

        private void ValidateKAndContinue()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                MessageBox.Show("Veuillez d'abord importer les données!", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowImportSection();
                return;
            }

            _sharedModel.K = KValue;
            _kValidated = true;
            ResultText = $"k = {KValue} validé. Choisissez maintenant la distance.";
            ShowDistanceConfigSection();
        }

        private void ValidateDistanceAndContinue()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                MessageBox.Show("Veuillez d'abord importer les données!", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowImportSection();
                return;
            }

            if (!_kValidated)
            {
                MessageBox.Show("Veuillez d'abord valider la valeur de k!", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowKConfigSection();
                return;
            }

            string distance = IsEuclidienne ? "Euclidienne" : "Manhattan";
            _sharedModel.SetDistance(distance);
            _distanceValidated = true;

            UpdateCurrentConfigText();
            ShowTrainTestSections();
            ResultText = $"Distance {distance} validée. Vous pouvez maintenant entraîner et tester.";
        }

        private void UpdateCurrentConfigText()
        {
            string distance = IsEuclidienne ? "Euclidienne" : "Manhattan";
            CurrentConfigText = $"Configuration actuelle :\n" +
                               $"   - k = {_sharedModel.K}\n" +
                               $"   - Distance = {distance}\n" +
                               $"   - Données d'entraînement = {_sharedModel.TrainingData?.Taille() ?? 0} échantillons\n" +
                               $"   - Données de test = {_sharedModel.TestData?.Taille() ?? 0} échantillons";
        }

        private void Train()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                MessageBox.Show("Veuillez d'abord importer les données!", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowImportSection();
                return;
            }

            if (!_kValidated || !_distanceValidated)
            {
                MessageBox.Show("Veuillez d'abord configurer k et la distance!", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowKConfigSection();
                return;
            }

            _sharedModel.InitializeClassifier();
            ResultText = $"Modèle entraîné avec succès\n\n" +
                        $"   - k = {_sharedModel.K}\n" +
                        $"   - Distance = {_sharedModel.GetDistanceName()}\n" +
                        $"   - Données d'entraînement = {_sharedModel.TrainingData.Taille()} échantillons\n\n" +
                        $"Vous pouvez maintenant tester le modèle.";

            MessageBox.Show("Entraînement terminé avec succès.", "Succès",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Test()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                MessageBox.Show("Veuillez d'abord importer les données.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ShowImportSection();
                return;
            }

            if (_sharedModel.Classifier == null)
            {
                MessageBox.Show("Veuillez d'abord entraîner le modèle.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (UtilisateurSelectionne == null)
            {
                MessageBox.Show("Veuillez sélectionner un auteur avant de tester.", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var evaluation = new EvaluationPerformance();
                evaluation.Evaluer(_sharedModel.K, _sharedModel.DistanceStrategy,
                                   _sharedModel.TrainingData, _sharedModel.TestData);

                double accuracy = evaluation.CalculerAccuracy() * 100;
                string distanceName = _sharedModel.GetDistanceName();

                SauvegarderDonnee(accuracy, distanceName);

                string resultat = $"RÉSULTAT DU TEST\n" +
                                 $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n" +
                                 $"Paramètres du classifieur :\n" +
                                 $"   • k = {_sharedModel.K}\n" +
                                 $"   • Distance = {distanceName}\n" +
                                 $"   • Données test = {_sharedModel.TestData.Taille()} échantillons\n\n" +
                                 $"Précision (Accuracy) = {accuracy:F2}%\n" +
                                 $"Auteur = {UtilisateurSelectionne.NomComplet}\n" +
                                 $"Données sauvegardées dans la base\n" +
                                 $"━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";

                ResultText = resultat;

                string dateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string historiqueEntry = $"{dateStr} | k={_sharedModel.K} | {distanceName} | Accuracy={accuracy:F2}% | Auteur={UtilisateurSelectionne.NomComplet}";
                HistoriqueList.Insert(0, historiqueEntry);

                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "historique.json");
                evaluation.SauvegarderJsonGlobal(jsonPath, _sharedModel.K, _sharedModel.DistanceStrategy,
                                                  _sharedModel.TrainingData, _sharedModel.TestData);

                ShowResultsSection();

                MessageBox.Show($"Test terminé! Précision: {accuracy:F2}%", "Résultats",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ResultText = $"Erreur lors du test: {ex.Message}";
                MessageBox.Show($"Erreur lors du test: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SauvegarderDonnee(double accuracy, string distanceName)
        {
            using (var context = new ClassificationGrainDeBlesContext())
            {
                var donnee = new Models.Donnee
                {
                    k = _sharedModel.K,
                    Distance = accuracy,
                    donnee_Tester = Path.GetFileName(TestPath),
                    precision = $"{accuracy:F2}%",
                    AuteurNom = UtilisateurSelectionne?.NomComplet ?? "Anonyme"
                };

                context.donnees.Add(donnee);
                context.SaveChanges();
            }
        }

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}