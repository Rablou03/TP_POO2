using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.IO;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WPFClassificationGrainsDeBles.Models
{
    public class EvaluationPerformance
    {
        private readonly int[,] matrice;
        private int total;
        private int correct;

        public EvaluationPerformance()
        {
            matrice = new int[3, 3];
            total = 0;
            correct = 0;
        }

        public void Evaluer(int k, IDistance distance, EnsembleDonnees train, EnsembleDonnees test)
        {
            ClassifierKnn classifier = new ClassifierKnn(k, distance);
            classifier.Entrainer(train);

            foreach (var e in test.ObtenirEchantillon())
            {
                TypeDeGrain reel = e.Etiquette;
                TypeDeGrain predit = classifier.Predire(e);

                if (reel == predit)
                    correct++;

                int ligne = GetIndex(reel);
                int colonne = GetIndex(predit);

                matrice[ligne, colonne]++;
                total++;
            }
        }

        public int GetIndex(TypeDeGrain type)
        {
            return type switch
            {
                TypeDeGrain.Kama => 0,
                TypeDeGrain.Rosa => 1,
                _ => 2 // Canadian
            };
        }

        public double CalculerAccuracy()
        {
            if (total == 0) return 0;
            return (double)correct / total;
        }

        // Version WPF : retourne un DataTable pour affichage dans un DataGrid
        public DataTable ConstruireTableauWpf()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Réel \\ Prédit");
            table.Columns.Add("Kama");
            table.Columns.Add("Rosa");
            table.Columns.Add("Canadian");

            string[] labels = { "Kama", "Rosa", "Canadian" };

            for (int i = 0; i < 3; i++)
            {
                table.Rows.Add(
                    labels[i],
                    matrice[i, 0],
                    matrice[i, 1],
                    matrice[i, 2]
                );
            }

            return table;
        }

        public void SauvegarderJsonGlobal(string chemin, int k, IDistance typeDistance, EnsembleDonnees train, EnsembleDonnees test)
        {
            var matriceListe = new List<List<int>>();
            for (int i = 0; i < 3; i++)
            {
                var ligne = new List<int>();
                for (int j = 0; j < 3; j++)
                    ligne.Add(matrice[i, j]);

                matriceListe.Add(ligne);
            }

            var nouveauResultat = new
            {
                ParametresExecution = new
                {
                    k = k,
                    distance = typeDistance.ToString(),
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                JeuxDeDonnees = new
                {
                    taille_train = train.Taille(),
                    taille_test = test.Taille()
                },
                Evaluation = new
                {
                    accuracy = CalculerAccuracy(),
                    matrice_confusion = matriceListe
                }
            };

            List<object> listeResultats;

            if (File.Exists(chemin))
            {
                string ancienJson = File.ReadAllText(chemin);
                try
                {
                    listeResultats = JsonSerializer.Deserialize<List<object>>(ancienJson) ?? new List<object>();
                }
                catch
                {
                    listeResultats = new List<object>();
                }
            }
            else
            {
                listeResultats = new List<object>();
            }

            listeResultats.Add(nouveauResultat);

            string jsonFinal = JsonSerializer.Serialize(listeResultats,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(chemin, jsonFinal);

            Console.WriteLine($"Fichier JSON mis à jour ici : {Path.GetFullPath(chemin)}");
        }
    }
}