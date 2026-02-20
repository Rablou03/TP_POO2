using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class Personne : IClient,IFermier
    {
        string nom;

        //public string Id()
        //{ }

        public string Nom(string nom)
        {
            return nom;
        }

        public void PasserComande()
        {

        }

        public void GererFerme()
        {

        }
    }
}
