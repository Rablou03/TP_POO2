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
        private int _kValue = 3;
        private string _selectedDistance = "Euclidienne";

        public int KValue
        {
            get => _kValue;
            set
            {
                _kValue = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDistance
        {
            get => _selectedDistance;
            set
            {
                _selectedDistance = value;
                OnPropertyChanged();
            }
        }

        public ICommand ApplyConfigCommand { get; }

        public ClassifierConfigViewModel()
        {
            ApplyConfigCommand = new RelayCommand(ApplyConfig);
        }

        private void ApplyConfig()
        {
            // Ici tu appliqueras la config réelle
        }
    }
}
