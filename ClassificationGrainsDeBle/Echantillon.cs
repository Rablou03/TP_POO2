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
        public TypeDeGrain Etiquette { get; set; }

        public Echantillon(double[] caracteristiques, TypeDeGrain etiquette)
        {
            Caracteristiques = caracteristiques;
            Etiquette = etiquette;
        }
    }
}