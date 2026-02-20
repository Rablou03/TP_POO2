using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class Echantillon
    {
        public double[] Caracteristiques { get; set; }
        public string Etiquette { get; set; }

        public Echantillon(double[] caracteristiques, string etiquette)
        {
            Caracteristiques = caracteristiques;
            Etiquette = etiquette;
        }
    }
}