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
    public class MainWindowViewModel : ViewModelBase
    {
        private object _currentView;
        private string _currentViewTitle;

        public object CurrentView
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

        public ICommand NavigateToImportCommand { get; }
        public ICommand NavigateToConfigCommand { get; }
        public ICommand NavigateToClassificationCommand { get; }
        public ICommand NavigateToHistoriqueCommand { get; }
        public ICommand QuitCommand { get; }

        public MainWindowViewModel()
        {
            NavigateToImportCommand = new RelayCommand(() => NavigateToImport());
            NavigateToConfigCommand = new RelayCommand(() => NavigateToConfig());
            NavigateToClassificationCommand = new RelayCommand(() => NavigateToClassification());
            NavigateToHistoriqueCommand = new RelayCommand(() => NavigateToHistorique());
            QuitCommand = new RelayCommand(() => System.Windows.Application.Current.Shutdown());

            NavigateToImport();
        }

        private void NavigateToImport()
        {
            CurrentView = new ImportDataViewModel();
            CurrentViewTitle = "Importation des données";
        }

        private void NavigateToConfig()
        {
            CurrentView = new ClassifierConfigViewModel();
            CurrentViewTitle = "Configuration du classifieur";
        }

        private void NavigateToClassification()
        {
            CurrentView = new ClassificationViewModel();
            CurrentViewTitle = "Classification";
        }

        private void NavigateToHistorique()
        {
            CurrentView = new HistoriqueViewModel();
            CurrentViewTitle = "Historique des expériences";
        }
    }
}