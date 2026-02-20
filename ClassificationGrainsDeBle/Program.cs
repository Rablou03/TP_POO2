using Spectre.Console;

namespace ClassificationGrainsDeBle
{
    internal class Program
    {
        static void Main(string[] args)
        {


            //// Charger les données d'entraînement


            //Data dataLoader = new Data();

            //string fichierTraining = "seeds_dataset_training.csv";
            //string fichierTest = "seeds_dataset_test.csv";

            //Console.WriteLine("Chargement des données d'entraînement");
            //List<Grain> grainsTraining = dataLoader.conversion_liste(fichierTraining);

            //EnsembleDonnees ensembleTraining = new EnsembleDonnees();

            //foreach (var g in grainsTraining)
            //{
            //    double[] caracteristiques = new double[]
            //    {
            //        g.Area,
            //        g.Perimeter,
            //        g.Compactness,
            //        g.LongueurNoyau,
            //        g.LargeurNoyau,
            //        g.AsymetryCoefficient,
            //        g.GrooveLength
            //    };

            //    ensembleTraining.Ajouter(new Echantillon(caracteristiques, g.TypeDeGrain.ToString()));
            //}


            //// Entraîner le classifieur KNN


            //Console.WriteLine("Entraînement du modèle KNN");
            //ClassifierKnn knn = new ClassifierKnn(3, new DistanceEuclidienne());
            //knn.Entrainer(ensembleTraining);


            //// Charger les données de test


            //Console.WriteLine("Chargement des données de test");
            //List<Grain> grainsTest = dataLoader.conversion_liste(fichierTest);


            //// Prédire pour chaque grain de test


            //Console.WriteLine("PRÉDICTIONS");

            //foreach (var g in grainsTest)
            //{
            //    double[] caracteristiques = new double[]
            //    {
            //        g.Area,
            //        g.Perimeter,
            //        g.Compactness,
            //        g.LongueurNoyau,
            //        g.LargeurNoyau,
            //        g.AsymetryCoefficient,
            //        g.GrooveLength
            //    };

            //    string prediction = knn.Predire(caracteristiques);

            //    Console.WriteLine(
            //        $"Réel : {g.TypeDeGrain} | Prédit : {prediction} | " +
            //        $"Area={g.Area}, Perimeter={g.Perimeter}, Compactness={g.Compactness}"
            //    );
            //}

            //Console.WriteLine("La lassification est terminée.");

            //////////////////////////////////////////////////////////////////
            /// spectre /////////////////////////////////////////
            /// 
            Data dataLoader = new Data();
            string fichierTraining = "seeds_dataset_training.csv";
            string fichierTest = "seeds_dataset_test.csv";

            EnsembleDonnees ensembleTraining = new EnsembleDonnees();
            ClassifierKnn knn = new ClassifierKnn(3, new DistanceEuclidienne());

            bool continuer = true;

            while (continuer)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[yellow]Menu Principal[/]"));

                string choix = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Que veux-tu faire ?[/]")
                        .AddChoices(new[] {
                            "Importer données",
                            "Entraîner modèle",
                            "Tester modèle",
                            "Quitter"
                        }));

                // ---------------------------------------------------------
                // IMPORTATION DES DONNÉES
                // ---------------------------------------------------------
                if (choix == "Importer données")
                {
                    AnsiConsole.Markup("[blue]Importation des données d'entraînement...[/]\n");

                    List<Grain> grainsTraining = dataLoader.conversion_liste(fichierTraining);

                    ensembleTraining = new EnsembleDonnees();

                    foreach (Grain g in grainsTraining)
                    {
                        double[] carac = new double[]
                        {
                            g.Area, g.Perimeter, g.Compactness,
                            g.LongueurNoyau, g.LargeurNoyau,
                            g.AsymetryCoefficient, g.GrooveLength
                        };

                        ensembleTraining.Ajouter(new Echantillon(carac, g.TypeDeGrain.ToString()));
                    }

                    AnsiConsole.Markup("[green]Importation terminée ![/]\n");

                    // Affichage tableau
                    Table table = new Table();
                    table.AddColumn("Type");
                    table.AddColumn("Area");
                    table.AddColumn("Perimeter");
                    table.AddColumn("Compactness");

                    foreach (Grain g in grainsTraining)
                    {
                        table.AddRow(
                            g.TypeDeGrain.ToString(),
                            g.Area.ToString(),
                            g.Perimeter.ToString(),
                            g.Compactness.ToString()
                        );
                    }

                    AnsiConsole.Write(table);

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }

                // ---------------------------------------------------------
                // ENTRAÎNEMENT DU MODÈLE
                // ---------------------------------------------------------
                else if (choix == "Entraîner modèle")
                {
                    if (ensembleTraining.Taille() == 0)
                    {
                        AnsiConsole.Markup("[red]Erreur : aucune donnée importée ![/]\n");
                    }
                    else
                    {
                        AnsiConsole.Markup("[yellow]Entraînement du modèle KNN...[/]\n");

                        knn.Entrainer(ensembleTraining);

                        // Tableau récapitulatif
                        Table recap = new Table();
                        recap.AddColumn("Paramètre");
                        recap.AddColumn("Valeur");

                        recap.AddRow("Nombre d'échantillons", ensembleTraining.Taille().ToString());
                        recap.AddRow("k", "3");
                        recap.AddRow("Distance", "Euclidienne");

                        AnsiConsole.Write(recap);

                        AnsiConsole.Markup("[green]Modèle entraîné avec succès ![/]\n");
                    }

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }



                // ---------------------------------------------------------
                // TEST DU MODÈLE
                // ---------------------------------------------------------
                else if (choix == "Tester modèle")
                {
                    AnsiConsole.Markup("[blue]Chargement des données de test...[/]\n");

                    List<Grain> grainsTest = dataLoader.conversion_liste(fichierTest);

                    Table table = new Table();
                    table.AddColumn("Réel");
                    table.AddColumn("Prédit");
                    table.AddColumn("Area");
                    table.AddColumn("Perimeter");
                    table.AddColumn("Compactness");

                    foreach (Grain g in grainsTest)
                    {
                        double[] carac = new double[]
                        {
                            g.Area, g.Perimeter, g.Compactness,
                            g.LongueurNoyau, g.LargeurNoyau,
                            g.AsymetryCoefficient, g.GrooveLength
                        };

                        string prediction = knn.Predire(carac);

                        table.AddRow(
                            g.TypeDeGrain.ToString(),
                            prediction,
                            g.Area.ToString(),
                            g.Perimeter.ToString(),
                            g.Compactness.ToString()
                        );
                    }

                    AnsiConsole.Write(table);

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }

                // ---------------------------------------------------------
                // QUITTER
                // ---------------------------------------------------------
                else if (choix == "Quitter")
                {
                    continuer = false;
                }
            }
        }
    }
}






