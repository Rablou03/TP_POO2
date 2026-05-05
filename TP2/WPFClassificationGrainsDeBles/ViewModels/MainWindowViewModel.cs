using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFClassificationGrainsDeBles.Models;
using System.IO;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // ========== VARIABLES PRIVEES ==========
        private object _currentView;
        private string _currentViewTitle;
        private readonly SharedModel _sharedModel;
        private string _selectedDistance = "Euclidienne";
        private string _importStatus = "Aucun fichier importé";
        private string _classificationResult;

        // ========== PROPRIETES ==========
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public string CurrentViewTitle
        {
            get => _currentViewTitle;
            set { _currentViewTitle = value; OnPropertyChanged(); }
        }

        public int K
        {
            get => _sharedModel.K;
            set
            {
                _sharedModel.K = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                _selectedDistance = value;
                _sharedModel.SetDistance(value);
                OnPropertyChanged();
            }
        }

        public string ImportStatus
        {
            get => _importStatus;
            set { _importStatus = value; OnPropertyChanged(); }
        }

        public string ClassificationResult
        {
            get => _classificationResult;
            set { _classificationResult = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> Historique { get; set; }

        // ========== COMMANDES ==========
        public ICommand NavigateToImportCommand { get; }
        public ICommand NavigateToConfigCommand { get; }
        public ICommand NavigateToClassificationCommand { get; }
        public ICommand NavigateToHistoriqueCommand { get; }
        public ICommand QuitCommand { get; }

        // ========== CONSTRUCTEUR ==========
        public MainWindowViewModel()
        {
            _sharedModel = new SharedModel();
            Historique = new ObservableCollection<string>();

            NavigateToImportCommand = new RelayCommand(() => NavigateToImport());
            NavigateToConfigCommand = new RelayCommand(() => NavigateToConfig());
            NavigateToClassificationCommand = new RelayCommand(() => NavigateToClassification());
            NavigateToHistoriqueCommand = new RelayCommand(() => NavigateToHistorique());
            QuitCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());

            NavigateToImport();
        }

        // ========== METHODES DE NAVIGATION ==========
        private void NavigateToImport()
        {
            var importVM = new ImportDataViewModel(this);
            CurrentView = importVM;
            CurrentViewTitle = "1. Importer les données";
        }

        private void NavigateToConfig()
        {
            var configVM = new ClassifierConfigViewModel(this);
            CurrentView = configVM;
            CurrentViewTitle = "2. Choisir k et 3. Choisir la distance";
        }

        private void NavigateToClassification()
        {
            if (!_sharedModel.IsDataLoaded)
            {
                ClassificationResult = "Erreur: Veuillez d'abord importer les données!";
                return;
            }

            var classifyVM = new ClassificationViewModel(this);
            CurrentView = classifyVM;
            CurrentViewTitle = "4. Entraîner et 5. Tester le modèle";
        }

        private void NavigateToHistorique()
        {
            var historiqueVM = new HistoriqueViewModel(this);  // ← AJOUTÉ : passer "this"
            CurrentView = historiqueVM;
            CurrentViewTitle = "Historique des expériences";
        }

        // ========== METHODES PRINCIPALES ==========
        public async Task ImportDataAsync(string trainPath, string testPath)
        {
            try
            {
                // ← MODIFIÉ : utiliser DataConverter au lieu de Convert
                var trainGrains = DataConverter.ConversionListe(trainPath);
                _sharedModel.TrainingData = new EnsembleDonnees();
                DataConverter.SaveEchantillon(trainGrains, _sharedModel.TrainingData);

                var testGrains = DataConverter.ConversionListe(testPath);
                _sharedModel.TestData = new EnsembleDonnees();
                DataConverter.SaveEchantillon(testGrains, _sharedModel.TestData);

                _sharedModel.IsDataLoaded = true;
                _sharedModel.InitializeClassifier();

                int trainCount = _sharedModel.TrainingData.Taille();
                int testCount = _sharedModel.TestData.Taille();
                ImportStatus = $"Succès: {trainCount} échantillons train, {testCount} échantillons test";
            }
            catch (Exception ex)
            {
                ImportStatus = $"Erreur: {ex.Message}";
                _sharedModel.IsDataLoaded = false;
            }
        }

        public string TrainAndTest()
        {
            if (!_sharedModel.IsDataLoaded)
                return "Erreur: Aucune donnée chargée";

            try
            {
                _sharedModel.InitializeClassifier();

                if (_sharedModel.Classifier == null)
                    return "Erreur: Impossible d'initialiser le classifieur";

                var evaluation = new EvaluationPerformance();
                evaluation.Evaluer(_sharedModel.K, _sharedModel.DistanceStrategy,
                                   _sharedModel.TrainingData, _sharedModel.TestData);

                double accuracy = evaluation.CalculerAccuracy() * 100;
                string distanceName = _sharedModel.GetDistanceName();

                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "historique.json");
                evaluation.SauvegarderJsonGlobal(jsonPath, _sharedModel.K, _sharedModel.DistanceStrategy,
                                                  _sharedModel.TrainingData, _sharedModel.TestData);

                string dateStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string historiqueEntry = $"{dateStr} | k={_sharedModel.K} | {distanceName} | Accuracy={accuracy:F2}%";
                Historique.Insert(0, historiqueEntry);

                string resultat = $"===== RÉSULTAT DE LA CLASSIFICATION =====\n\n" +
                                 $"Paramètres:\n" +
                                 $"  - k = {_sharedModel.K}\n" +
                                 $"  - Distance = {distanceName}\n" +
                                 $"  - Données train = {_sharedModel.TrainingData.Taille()}\n" +
                                 $"  - Données test = {_sharedModel.TestData.Taille()}\n\n" +
                                 $"Précision (Accuracy) = {accuracy:F2}%\n\n" +
                                 $"Résultat sauvegardé dans l'historique!";

                ClassificationResult = resultat;
                return resultat;
            }
            catch (Exception ex)
            {
                string erreur = $"Erreur: {ex.Message}";
                ClassificationResult = erreur;
                return erreur;
            }
        }
    }
}