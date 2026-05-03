// ViewModels/MainWindowViewModel.cs
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ClassificationGrainsDeBle_WPF.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object? _currentView;
        private string _currentViewTitle;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            // Initialisation des commandes
            NavigateToImportCommand = new RelayCommand(_ => NavigateToImport());
            NavigateToConfigCommand = new RelayCommand(_ => NavigateToConfig());
            NavigateToClassificationCommand = new RelayCommand(_ => NavigateToClassification());
            NavigateToHistoriqueCommand = new RelayCommand(_ => NavigateToHistorique());
            QuitCommand = new RelayCommand(_ => Quit());

            // Vue par défaut
            NavigateToImport();
        }

        public ICommand NavigateToImportCommand { get; }
        public ICommand NavigateToConfigCommand { get; }
        public ICommand NavigateToClassificationCommand { get; }
        public ICommand NavigateToHistoriqueCommand { get; }
        public ICommand QuitCommand { get; }

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public string CurrentViewTitle
        {
            get => _currentViewTitle;
            set
            {
                _currentViewTitle = value;
                OnPropertyChanged();
            }
        }

        private void NavigateToImport()
        {
            CurrentView = new ImportDataViewModel();
            CurrentViewTitle = "📁 Chargement des données";
        }

        private void NavigateToConfig()
        {
            CurrentView = new ClassifierConfigViewModel();
            CurrentViewTitle = "⚙️ Configuration du classifieur";
        }

        private void NavigateToClassification()
        {
            CurrentView = new ClassificationViewModel();
            CurrentViewTitle = "🏷️ Classification";
        }

        private void NavigateToHistorique()
        {
            CurrentView = new HistoriqueViewModel();
            CurrentViewTitle = "📜 Historique des expériences";
        }

        private void Quit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}