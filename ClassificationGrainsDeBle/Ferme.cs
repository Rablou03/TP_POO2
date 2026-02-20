using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    //internal abstract class Ferme
    //{
    //    string nom;
    //    string adresse;
    //    List<IClient> clients;

    //    // J'au ajouté constructeurs 
    //    public Ferme(string nom, string adresse)
    //    {
    //        this.nom = nom;
    //        this.adresse = adresse;
    //        this.clients = new List<IClient>();
    //    }

    //    public string Nom() { return nom; }
    //    public string Adresse() { return adresse; }

    //    public void Ajout(IClient clientAjout)
    //    {
    //        clients.Add(clientAjout);    
    //    }

    //    public abstract void AjouterLot(LotDeGrain ajoutLotDeGrain);


    namespace ClassificationGrainsDeBle
    {
        internal abstract class Ferme
        {
            private string nom;
            private string adresse;
            private List<IClient> clients = new List<IClient>();

            public Ferme(string nom, string adresse)
            {
                this.nom = nom;
                this.adresse = adresse;
                this.clients = new List<IClient>();
            }

            public string Nom()
            {
                return nom;
            }

            public string Adresse()
            {
                return adresse;
            }

            public void Ajout(IClient clientAjout)
            {
                clients.Add(clientAjout);
            }

            public abstract void AjouterLot(LotDeGrain ajoutLotDeGrain);
        }
    }
}

