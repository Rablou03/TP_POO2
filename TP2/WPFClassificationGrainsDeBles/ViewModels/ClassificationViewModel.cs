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
    public class ClassificationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainVM;
        private string _resultats;
        private bool _isTraining;

        public string Resultats
        {
            get => _resultats;
            set { _resultats = value; OnPropertyChanged(); }
        }

        public bool IsTraining
        {
            get => _isTraining;
            set { _isTraining = value; OnPropertyChanged(); }
        }

        public ICommand TrainAndTestCommand { get; }

        // Constructeur avec paramètre
        public ClassificationViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            TrainAndTestCommand = new RelayCommand(async () => await ExecuteTrainAndTestAsync());
            Resultats = "Prêt à entraîner et tester...";
        }

        private async Task ExecuteTrainAndTestAsync()
        {
            IsTraining = true;
            Resultats = "🔄 Entraînement en cours...";

            await Task.Delay(100);

            string result = _mainVM.TrainAndTest();
            Resultats = result;

            IsTraining = false;
        }
    }
}