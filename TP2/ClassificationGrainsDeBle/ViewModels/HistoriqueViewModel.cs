// ViewModels/HistoriqueViewModel.cs
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassificationGrainsDeBle_WPF.ViewModels
{
    public class HistoriqueViewModel : INotifyPropertyChanged
    {
        private Experience? _selectedExperience;

        public event PropertyChangedEventHandler? PropertyChanged;

        public HistoriqueViewModel()
        {
            Experiences = ExperiencesManager.Experiences;
            ClearHistoryCommand = new RelayCommand(_ => ClearHistory());
        }

        public ObservableCollection<Experience> Experiences { get; }

        public ICommand ClearHistoryCommand { get; }

        public Experience? SelectedExperience
        {
            get => _selectedExperience;
            set 
            { 
                _selectedExperience = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelectedExperience));
            }
        }

        public bool HasSelectedExperience => SelectedExperience != null;

        private void ClearHistory()
        {
            ExperiencesManager.ClearExperiences();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}