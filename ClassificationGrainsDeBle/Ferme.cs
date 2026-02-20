using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal abstract class Ferme
    {
        string nom;
        string adresse;
        List<IClient> clients;
       

        
        public string Nom() { return nom; }
        public string Adresse() { return adresse; }

        public void Ajout(IClient clientAjout)
        {
            clients.Add(clientAjout);    
        }

        public abstract void AjouterLot(LotDeGrain ajoutLotDeGrain);
    }
}
