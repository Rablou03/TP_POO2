using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class EnsembleDonnees
    {
        private List<Echantillon> echantillons = new List<Echantillon>();
        public void AjouterUnEchantillon(Echantillon e)
        {
            echantillons.Add(e);
        }

        public void AjouterListEchantillon(List<Echantillon> echantillons)
        {
            foreach (Echantillon e in echantillons)
            {
                echantillons.Add(e);
            }
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
