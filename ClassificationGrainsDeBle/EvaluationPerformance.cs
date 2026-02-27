using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spectre.Console;

namespace ClassificationGrainsDeBle
{
    internal class EvaluationPerformance
    {
        int[,] matrice;
        int total;
        int correct;

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
            if (type == TypeDeGrain.Kama) return 0;
            if (type == TypeDeGrain.Rosa) return 1;
            return 2; // Canadian
        }

        public double CalculerAccuracy()
        {
            if (total == 0) return 0;
            return (double)correct / total;
        }

        public void AfficherTableSpectre()
        {
            var table = new Table();

            table.Border(TableBorder.Rounded);
            table.AddColumn("[yellow]Réel \\ Prédit[/]");
            table.AddColumn("[blue]Kama[/]");
            table.AddColumn("[green]Rosa[/]");
            table.AddColumn("[red]Canadian[/]");

            string[] labels = { "Kama", "Rosa", "Canadian" };

            for (int i = 0; i < 3; i++)
            {
                table.AddRow(
                    $"[bold]{labels[i]}[/]",
                    matrice[i, 0].ToString(),
                    matrice[i, 1].ToString(),
                    matrice[i, 2].ToString()
                );
            }

            AnsiConsole.Write(table);
            double accuracy = (double)correct / total;

            AnsiConsole.MarkupLine($"\n[bold green]Accuracy : {accuracy:P2}[/]");
        }

        public void SauvegarderJsonGlobal(string chemin, int k, IDistance typeDistance, EnsembleDonnees train, EnsembleDonnees test)
        {
            // Conversion matrice 2D -> liste de listes
            var matriceListe = new List<List<int>>();
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                var ligne = new List<int>();
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    ligne.Add(matrice[i, j]);
                }
                matriceListe.Add(ligne);
            }

            // Crée l'objet résultat
            var nouveauResultat = new
            {
                ParametresExecution = new
                {
                    k = k,
                    distance = typeDistance,
                    date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                },
                JeuxDeDonnees = new
                {
                    taille_train = train.Taille(),
                    taille_test = test.Taille()
                },
                Evaluation = new
                {
                    accuracy = (double)correct / total,
                    matrice_confusion = matriceListe
                }
            };

            List<object> listeResultats;

            // Si le fichier existe déjà, on le lit et on désérialise
            if (File.Exists(chemin))
            {
                string ancienJson = File.ReadAllText(chemin);
                try
                {
                    listeResultats = JsonSerializer.Deserialize<List<object>>(ancienJson) ?? new List<object>();
                }
                catch
                {
                    // Si le fichier existant n'est pas un tableau, on recrée une liste
                    listeResultats = new List<object>();
                }
            }
            else
            {
                listeResultats = new List<object>();
            }

            // On ajoute le nouveau résultat
            listeResultats.Add(nouveauResultat);

            // On réécrit le fichier
            string jsonFinal = JsonSerializer.Serialize(listeResultats,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(chemin, jsonFinal);

            Console.WriteLine($"Fichier JSON mis à jour ici : {Path.GetFullPath(chemin)}");
        }

    }
}
