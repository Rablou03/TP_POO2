using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;

namespace WPFClassificationGrainsDeBles.Models
{
    public class DataConverter  // Changé de Convert à DataConverter
    {
        public static List<Grain> ConversionListe(string nom_fichier)
        {
            List<Grain> grains = new List<Grain>();

            if (!File.Exists(nom_fichier))
            {
                throw new FileNotFoundException($"Fichier non trouvé: {nom_fichier}");
            }

            string[] lignes = File.ReadAllLines(nom_fichier);
            if (lignes.Length == 0) return grains;

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
                if (colonnes.Length < 8) continue;

                try
                {
                    var g = new Grain(
                        (TypeDeGrain)Enum.Parse(typeof(TypeDeGrain), colonnes[idxVariety]),
                        double.Parse(colonnes[idxArea], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxPerimeter], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxCompactness], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxKernelLength], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxKernelWidth], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxAsymmetry], CultureInfo.InvariantCulture),
                        double.Parse(colonnes[idxGroove], CultureInfo.InvariantCulture)
                    );
                    grains.Add(g);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur ligne {i}: {ex.Message}");
                }
            }

            return grains;
        }

        public static void SaveEchantillon(List<Grain> grains, EnsembleDonnees ensemble)
        {
            if (grains == null || ensemble == null) return;

            foreach (var g in grains)
            {
                double[] carac = new double[]
                {
                    g.Area, g.Perimeter, g.Compactness,
                    g.LongueurNoyau, g.LargeurNoyau,
                    g.AsymetryCoefficient, g.GrooveLength
                };

                ensemble.AjouterUnEchantillon(new Echantillon(carac, g.TypeDeGrain));
            }
        }
    }
}