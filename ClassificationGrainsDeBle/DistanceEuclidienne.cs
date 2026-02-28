using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class DistanceEuclidienne : IDistance
    {
        public double Calculer(double[] a, double[] b)
        {
            double somme = 0;
            for (int i = 0; i < a.Length; i++)
            {
                somme += Math.Pow(a[i] - b[i], 2);
            }
            return Math.Sqrt(somme);
        }
    }
}
