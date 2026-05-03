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
    public class ImportDataViewModel : ViewModelBase
    {
        private string _status = "Aucun fichier importé.";

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public ICommand ImportCommand { get; }

        public ImportDataViewModel()
        {
            ImportCommand = new RelayCommand(ImportFile);
        }

        private void ImportFile()
        {
            // Simulation d'import
            Status = "Fichier importé avec succès.";
        }
    }
}
