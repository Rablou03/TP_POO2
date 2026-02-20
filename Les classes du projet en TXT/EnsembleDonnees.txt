using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class EnsembleDonnees
    {
        // Pour les données aussi
        private List<Echantillon> echantillons = new List<Echantillon>();  
        public void Ajouter(Echantillon e)
        {
            echantillons.Add(e);
        }
        public List<Echantillon> ObtenirEchantillon()
        { 
            return echantillons; 
        }
        public int Taille()
        {
            return echantillons.Count;
        }

    }
}
