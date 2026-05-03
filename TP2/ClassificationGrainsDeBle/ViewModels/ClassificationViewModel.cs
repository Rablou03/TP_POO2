// ViewModels/ClassificationViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ClassificationGrainsDeBle;

namespace ClassificationGrainsDeBle_WPF.ViewModels
{
    public class ClassificationViewModel : INotifyPropertyChanged
    {
        private double _accuracy;
        private ObservableCollection<ConfusionMatrixRow> _confusionMatrix = new();
        private string _statusMessage = "Prêt pour la classification";
        private bool _isClassificationRunning = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ClassificationViewModel()
        {
            RunClassificationCommand = new RelayCommand(_ => RunClassification(), 
                _ => ClassifierConfig.IsDataLoaded && !IsClassificationRunning);
            ResetCommand = new RelayCommand(_ => Reset());
        }

        public ICommand RunClassificationCommand { get; }
        public ICommand ResetCommand { get; }

        public double Accuracy
        {
            get => _accuracy;
            set { _accuracy = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ConfusionMatrixRow> ConfusionMatrix
        {
            get => _confusionMatrix;
            set { _confusionMatrix = value; OnPropertyChanged(); }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public bool IsClassificationRunning
        {
            get => _isClassificationRunning;
            set 
            { 
                _isClassificationRunning = value; 
                OnPropertyChanged();
                (RunClassificationCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void RunClassification()
        {
            try
            {
                IsClassificationRunning = true;
                StatusMessage = "Classification en cours...";

                var classifier = new ClassifierKnn(ClassifierConfig.K, ClassifierConfig.Distance);
                classifier.Entrainer(ClassifierConfig.TrainingData!);

                var evaluation = new EvaluationPerformance();
                evaluation.Evaluer(ClassifierConfig.K, ClassifierConfig.Distance, 
                    ClassifierConfig.TrainingData!, ClassifierConfig.TestData!);
                
                Accuracy = evaluation.CalculerAccuracy();
                UpdateConfusionMatrix(evaluation);

                // Sauvegarder l'expérience
                SaveExperience(evaluation);

                StatusMessage = $"Classification terminée! Accuracy: {Accuracy:P2}";
                
                MessageBox.Show($"Classification terminée avec succès!\nAccuracy: {Accuracy:P2}",
                    "Résultat", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erreur lors de la classification: {ex.Message}";
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsClassificationRunning = false;
            }
        }

        private void UpdateConfusionMatrix(EvaluationPerformance evaluation)
        {
            ConfusionMatrix.Clear();
            
            // Utiliser la réflexion ou modifier EvaluationPerformance pour exposer la matrice
            // Pour l'exemple, on crée une structure générique
            var matrix = GetMatrixFromEvaluation(evaluation);
            
            string[] labels = { "Kama", "Rosa", "Canadian" };
            
            for (int i = 0; i < 3; i++)
            {
                ConfusionMatrix.Add(new ConfusionMatrixRow
                {
                    ActualLabel = labels[i],
                    PredictedKama = matrix[i, 0],
                    PredictedRosa = matrix[i, 1],
                    PredictedCanadian = matrix[i, 2]
                });
            }
        }

        private int[,] GetMatrixFromEvaluation(EvaluationPerformance evaluation)
        {
            // À implémenter : extraire la matrice de l'objet evaluation
            // Pour l'instant, retourne une matrice vide
            return new int[3, 3];
        }

        private void SaveExperience(EvaluationPerformance evaluation)
        {
            var experience = new Experience
            {
                Date = DateTime.Now,
                K = ClassifierConfig.K,
                Distance = ClassifierConfig.DistanceName,
                Accuracy = Accuracy,
                TrainingSize = ClassifierConfig.TrainingData?.Taille() ?? 0,
                TestSize = ClassifierConfig.TestData?.Taille() ?? 0
            };
            
            ExperiencesManager.AddExperience(experience);
        }

        private void Reset()
        {
            Accuracy = 0;
            ConfusionMatrix.Clear();
            StatusMessage = "Prêt pour une nouvelle classification";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ConfusionMatrixRow
    {
        public string ActualLabel { get; set; } = string.Empty;
        public int PredictedKama { get; set; }
        public int PredictedRosa { get; set; }
        public int PredictedCanadian { get; set; }
    }

    public class Experience
    {
        public DateTime Date { get; set; }
        public int K { get; set; }
        public string Distance { get; set; } = string.Empty;
        public double Accuracy { get; set; }
        public int TrainingSize { get; set; }
        public int TestSize { get; set; }
    }

    public static class ExperiencesManager
    {
        private static ObservableCollection<Experience> _experiences = new();
        
        public static ObservableCollection<Experience> Experiences => _experiences;
        
        public static void AddExperience(Experience exp)
        {
            _experiences.Insert(0, exp);
        }
        
        public static void ClearExperiences()
        {
            _experiences.Clear();
        }
    }
}