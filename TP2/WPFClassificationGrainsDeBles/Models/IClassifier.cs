using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClassificationGrainsDeBles.Models
{
    internal interface IClassifier
    {
        void Entrainer(EnsembleDonnees data);
        TypeDeGrain Predire(Echantillon e);
    }
}
