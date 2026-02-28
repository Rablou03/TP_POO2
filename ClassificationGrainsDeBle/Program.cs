using Spectre.Console;
using System.IO;

namespace ClassificationGrainsDeBle
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string fichierTrain = "seeds_dataset_training.csv";
            string fichierTest = "seeds_dataset_test.csv";

            EnsembleDonnees training = new EnsembleDonnees();
            EnsembleDonnees testing = new EnsembleDonnees();

            IDistance distanceChoisie = new DistanceEuclidienne();
            int k = 3;

            ClassifierKnn knn = new ClassifierKnn(k, distanceChoisie);

            bool continuer = true;

            while (continuer)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[yellow]MENU PRINCIPAL[/]"));

                string choix = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Choisissez les options dans l'ordre: [/]")
                        .AddChoices(new[]
                        {
                            "1 - Importer données",
                            "2 - Choisir k",
                            "3 - Choisir distance",
                            "4 - Entraîner modèle",
                            "5 - Tester modèle",
                            "6 - Quitter"
                        }));

                switch (choix)
                {

                    case "1 - Importer données":

                        //importation de Train
                        List<Grain> grainsTrain = Convert.conversion_liste(fichierTrain);
                        Convert.saveEchantillon(grainsTrain, training);
                        AnsiConsole.Markup("[green]Les données en Grain du fichier train: [/]\n");
                        Convert.ConstuireTableauDeGrain(grainsTrain);




                        //importation de Test
                        List<Grain> grainsTest = Convert.conversion_liste(fichierTest);
                        AnsiConsole.Markup("[green]Les données en Grain du fichier test: [/]\n");
                        Convert.ConstuireTableauDeGrain(grainsTest);
                        Convert.saveEchantillon(grainsTest, testing);

                        testing = new EnsembleDonnees();

                        Convert.saveEchantillon(grainsTest, testing);

                        AnsiConsole.Markup("[green]Données importées [/]\n");
                        break;


                    //choix de cas
                    case "2 - Choisir k":

                        k = AnsiConsole.Ask<int>("[yellow]Entrez la valeur de k :[/]");
                        knn = new ClassifierKnn(k, distanceChoisie);

                        AnsiConsole.Markup($"[green]k mis à jour : {k} [/]\n");
                        break;


                    //choix du méthode de calcul de distance
                    case "3 - Choisir distance":

                        string choixDistance = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[yellow]Choisir distance[/]")
                                .AddChoices("Euclidienne", "Manhattan"));

                        if (choixDistance == "Euclidienne")
                            distanceChoisie = new DistanceEuclidienne();
                        else
                            distanceChoisie = new DistanceManhattan();

                        knn = new ClassifierKnn(k, distanceChoisie);

                        AnsiConsole.Markup("[green]Distance mise à jour [/]\n");
                        break;


                    //entrainer le modéle
                    case "4 - Entraîner modèle":

                        if (training.Taille() == 0)
                        {
                            AnsiConsole.Markup("[red]Importer les données d'abord ![/]\n");
                        }
                        else
                        {
                            knn.Entrainer(training);
                            AnsiConsole.Markup("[green]Modèle entraîné [/]\n");
                        }
                        break;


                    //tester
                    case "5 - Tester modèle":

                        if (training.Taille() == 0)
                        {
                            AnsiConsole.Markup("[red]Modèle non entraîné ![/]\n");
                        }
                        else
                        {
                            EvaluationPerformance eval =
                                new EvaluationPerformance();

                            eval.Evaluer(k, distanceChoisie, training, testing);

                            eval.AfficherTableSpectre();
                            
                            eval.SauvegarderJsonGlobal("resultats.json", k, distanceChoisie, training, testing);

                            AnsiConsole.Markup(
                                "\n[blue]Résultats sauvegardés dans resultats.json [/]\n");
                            Console.WriteLine($"Fichier JSON créé ici : {Path.GetFullPath("resultat.json")}");
                        }
                        break;


                    case "6 - Quitter":
                        continuer = false;
                        break;
                }

                if (continuer)
                {
                    AnsiConsole.Markup(
                        "\n[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }
            }
        }
    }
}
