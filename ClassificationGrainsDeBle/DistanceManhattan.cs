using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class DistanceManhattan : Idistance
    {
        /* Lien 1 : https://codereview.stackexchange.com/questions/120933/calculating-distance-with-euclidean-manhattan-and-chebyshev-in-c
         * Lien 2 : https://www.geeksforgeeks.org/dsa/sum-manhattan-distances-pairs-points/
         * */
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
