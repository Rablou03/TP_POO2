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
    internal class Convert
    {
        public static List<Grain> ConversionListe(string nom_fichier)
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

            return grains;
        }

        public static DataTable ConstruireTableauDeGrains(List<Grain> grains)
        {
            DataTable table = new DataTable();

            table.Columns.Add("Type");
            table.Columns.Add("Area");
            table.Columns.Add("Perimeter");
            table.Columns.Add("Compactness");
            table.Columns.Add("Kernel Length");
            table.Columns.Add("Kernel Width");
            table.Columns.Add("Asymmetry");
            table.Columns.Add("Groove Length");

            foreach (var g in grains)
            {
                table.Rows.Add(
                    g.TypeDeGrain.ToString(),
                    g.Area,
                    g.Perimeter,
                    g.Compactness,
                    g.LongueurNoyau,
                    g.LargeurNoyau,
                    g.AsymetryCoefficient,
                    g.GrooveLength
                );
            }

            return table;
        }

        public static void SaveEchantillon(List<Grain> grains, EnsembleDonnees trainingSet)
        {
            foreach (var g in grains)
            {
                double[] carac = new double[]
                {
                    g.Area, g.Perimeter, g.Compactness,
                    g.LongueurNoyau, g.LargeurNoyau,
                    g.AsymetryCoefficient, g.GrooveLength
                };

                trainingSet.AjouterUnEchantillon(new Echantillon(carac, g.TypeDeGrain));
            }
        }
    }
}