using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFClassificationGrainsDeBles.ViewModels
{
    public class HistoriqueViewModel : ViewModelBase
    {
        public ObservableCollection<string> Historique { get; set; }

        public HistoriqueViewModel()
        {
            Historique = new ObservableCollection<string>
            {
                "Exécution 1 : k=3, Euclidienne",
                "Exécution 2 : k=5, Manhattan"
            };
        }
    }
}