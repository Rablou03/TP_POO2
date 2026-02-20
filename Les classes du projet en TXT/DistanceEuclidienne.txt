using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class DistanceEuclidienne : IDistance
    {
        /* Lien 1 : https://stackoverflow.com/questions/34698649/calculate-the-euclidean-distance-between-an-array-in-c-sharp-with-function
         * Lien 2 : https://learn.microsoft.com/fr-fr/archive/msdn-magazine/2019/may/test-run-weighted-k-nn-classification-using-csharp
         */
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
