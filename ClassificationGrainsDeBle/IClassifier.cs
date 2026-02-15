using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    
    internal interface IClassifier
    {
        void Entrainer(EnsembleDonnees data);
        string Predire(double[] caracteristiques);
    }
}
