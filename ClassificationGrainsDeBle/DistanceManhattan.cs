using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class DistanceManhattan : IDistance
    {
        public double Calculer(double[] a, double[] b)
        {
            double somme = 0;
            for (int i = 0; i < a.Length; i++)
            {
                somme += Math.Abs(a[i] - b[i]);
            }
            return somme;
        }
    }
}
