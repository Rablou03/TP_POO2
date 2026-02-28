using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ClassificationGrainsDeBle;
using CsvHelper;
using Spectre.Console;

namespace ClassificationGrainsDeBle
{

    internal class Convert
    {

        public static List<Grain> conversion_liste(string nom_fichier)

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
                    Enum.Parse<TypeDeGrain>(colonnes[idxVariety]),
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

        public static void ConstuireTableauDeGrain(List<Grain> grains)
        {
            var table = new Table();

            table.AddColumn("Type");
            table.AddColumn("Area");
            table.AddColumn("Perimeter");
            table.AddColumn("Compactness");
            table.AddColumn("Kernel Length");
            table.AddColumn("Kernel Width");
            table.AddColumn("Asymmetry");
            table.AddColumn("Groove Length");

            foreach (var g in grains)
            {
                table.AddRow(
                    g.TypeDeGrain.ToString(),
                    g.Area.ToString(),
                    g.Perimeter.ToString(),
                    g.Compactness.ToString(),
                    g.LongueurNoyau.ToString(),
                    g.LargeurNoyau.ToString(),
                    g.AsymetryCoefficient.ToString(),
                    g.GrooveLength.ToString()
                );

            }
            AnsiConsole.Write(table);
        }

        public static void saveEchantillon(List<Grain> grains, EnsembleDonnees gainsTraining)
        {
            List<Echantillon> echantillons = new List<Echantillon>();
            foreach (Grain g in grains)
            {
                double[] carac = new double[]
                {
                    g.Area, g.Perimeter, g.Compactness,
                            g.LongueurNoyau, g.LargeurNoyau,
                            g.AsymetryCoefficient, g.GrooveLength
                };
                gainsTraining.AjouterUnEchantillon(new Echantillon(carac, g.TypeDeGrain));
            }
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
