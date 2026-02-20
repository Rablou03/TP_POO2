using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;

namespace ClassificationGrainsDeBle
{

    internal class Data
    {

        string nom_fichier = "seeds_dataset_training.csv";

        public List<Grain> conversion_liste(string nom_fichier)
        {
            List<Grain> grains = new List<Grain>();
            string[] lignes = File.ReadAllLines(nom_fichier);


            string[] headers = lignes[0].Split(';');


            int idxVariety = Array.IndexOf(headers, "variety");
            int idxArea = Array.IndexOf(headers, "Area");
            int idxPerimeter = Array.IndexOf(headers, "Perimeter");
            int idxCompactness = Array.IndexOf(headers, "Compactness");
            int idxKernelLength = Array.IndexOf(headers, "Kernel_Length");
            int idxKernelWidth = Array.IndexOf(headers, "Kernel_Width");
            int idxAsymmetry = Array.IndexOf(headers, "Asymmetry_Coefficient");
            int idxGroove = Array.IndexOf(headers, "Groove_Length");


            for (int i = 1; i < lignes.Length; i++)
            {
                string[] colonnes = lignes[i].Split(';');

                Grain g = new Grain(
                    colonnes[idxVariety],
                    double.Parse(colonnes[idxArea]),
                    double.Parse(colonnes[idxPerimeter]),
                    double.Parse(colonnes[idxCompactness]),
                    double.Parse(colonnes[idxKernelLength]),
                    double.Parse(colonnes[idxKernelWidth]),
                    double.Parse(colonnes[idxAsymmetry]),
                    double.Parse(colonnes[idxGroove])
                );

                grains.Add(g);
            }

            return grains;
        }
        public static void Afficher(List<Grain> grains)
        {
            foreach (Grain g in grains)
            {
                Console.WriteLine($"{g.TypeDeGrain} | {g.Area} | {g.Perimeter} | {g.Compactness} | " + $"{g.LongueurNoyau} | {g.LargeurNoyau} | {g.AsymetryCoefficient} | {g.GrooveLength}");
            }
        }


    }
}
