using System.Collections.ObjectModel;
using System.Windows.Input;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class ClassificationViewModel : ViewModelBase
    {
        public ObservableCollection<string> Resultats { get; set; }

        public ICommand ClassifyCommand { get; }

        public ClassificationViewModel()
        {
            Resultats = new ObservableCollection<string>();
            ClassifyCommand = new RelayCommand(Classify);
        }

        private void Classify()
        {
            Resultats.Clear();
            Resultats.Add("Exemple : Classe A");
            Resultats.Add("Exemple : Classe B");
        }
    }
}