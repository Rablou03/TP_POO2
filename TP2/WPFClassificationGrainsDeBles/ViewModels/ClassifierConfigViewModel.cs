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
    public class ClassifierConfigViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainVM;
        private int _kValue;
        private string _selectedDistance;
        private string _configStatus;

        public int KValue
        {
            get => _kValue;
            set
            {
                _kValue = value;
                _mainVM.K = value;
                OnPropertyChanged();
                UpdateConfigStatus();
            }
        }

        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                _selectedDistance = value;
                _mainVM.SelectedDistance = value;
                OnPropertyChanged();
                UpdateConfigStatus();
            }
        }

        public string ConfigStatus
        {
            get => _configStatus;
            set { _configStatus = value; OnPropertyChanged(); }
        }

        public List<string> DistanceOptions { get; } = new List<string> { "Euclidienne", "Manhattan" };

        public ICommand ApplyConfigCommand { get; }

        // Constructeur avec paramètre
        public ClassifierConfigViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            _kValue = _mainVM.K;
            _selectedDistance = _mainVM.SelectedDistance;
            UpdateConfigStatus();
            ApplyConfigCommand = new RelayCommand(ApplyConfig);
        }

        private void UpdateConfigStatus()
        {
            ConfigStatus = $"Configuration: k={KValue}, Distance={SelectedDistance}";
        }

        private void ApplyConfig()
        {
            ConfigStatus = $"✅ Configuration appliquée!";
        }
    }
}