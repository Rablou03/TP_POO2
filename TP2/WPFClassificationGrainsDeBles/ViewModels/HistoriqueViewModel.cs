using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class HistoriqueViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel _mainVM;
        private ObservableCollection<string> _historique;
        private string _detailsHistorique;

        public ObservableCollection<string> Historique
        {
            get => _historique;
            set { _historique = value; OnPropertyChanged(); }
        }

        public string DetailsHistorique
        {
            get => _detailsHistorique;
            set { _detailsHistorique = value; OnPropertyChanged(); }
        }

        // CONSTRUCTEUR SANS PARAMÈTRE (existant)
        public HistoriqueViewModel()
        {
            _historique = new ObservableCollection<string>();
            LoadFullHistory();
        }

        // CONSTRUCTEUR AVEC PARAMÈTRE (AJOUTER CECI)
        public HistoriqueViewModel(MainWindowViewModel mainVM)
        {
            _mainVM = mainVM;
            _historique = _mainVM.Historique;
            LoadFullHistory();
        }

        private void LoadFullHistory()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "historique.json");
            if (File.Exists(jsonPath))
            {
                try
                {
                    DetailsHistorique = File.ReadAllText(jsonPath);
                }
                catch
                {
                    DetailsHistorique = "Impossible de charger l'historique";
                }
            }
            else
            {
                DetailsHistorique = "Aucun historique disponible. Lancez d'abord une classification.";
            }
        }
    }
}