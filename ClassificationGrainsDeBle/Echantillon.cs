using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class Echantillon
    {
        // Pour les données
        public double[] Caracterisques { get; set; }    
        public string Etiquette { get; set; }

        public Echantillon(double[] caracterisques, string etiquette)
        {
            Caracterisques = caracterisques;
            Etiquette = etiquette;
        }
    }
}
