using Spectre.Console;
using System.IO;

namespace ClassificationGrainsDeBle
{
    internal class Program
    {
        static int ClasseToIndex(string classe)
        {
            switch (classe.Trim().ToLower())
            {
                case "kama":
                case "1":
                    return 0;

                case "rosa":
                case "2":
                    return 1;

                case "canadian":
                case "3":
                    return 2;

                default:
                    throw new Exception($"Classe inconnue : {classe}");
            }
        }
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

            // Distance par défaut
            IDistance distanceChoisie = new DistanceEuclidienne();

            // KNN initial
            ClassifierKnn knn = new ClassifierKnn(3, distanceChoisie);

            bool continuer = true;

            while (continuer)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(new Rule("[yellow]MENU[/]"));

                string choix = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]FAIRE UN CHOIX[/]")
                        .AddChoices(new[] {
                            "Importer données",
                            "Choisir la distance",
                            "Entraîner modèle",
                            "Tester modèle",
                            "Quitter"
                        }));


                // Importer données

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

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }

              
                // Choixz de la distance
               
                else if (choix == "Choisir la distance")
                {
                    string choixDistance = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Choisir la distance :[/]")
                            .AddChoices(new[] {
                                "Euclidienne",
                                "Manhattan"
                            }));

                    if (choixDistance == "Euclidienne")
                    {
                        distanceChoisie = new DistanceEuclidienne();
                        AnsiConsole.Markup("[green]Distance Euclidienne sélectionnée ![/]\n");
                    }
                    else if (choixDistance == "Manhattan")
                    {
                        distanceChoisie = new DistanceManhattan();
                        AnsiConsole.Markup("[green]Distance Manhattan sélectionnée ![/]\n");
                    }

                    // Recréer le KNN avec la nouvelle distance
                    knn = new ClassifierKnn(3, distanceChoisie);

                    // Réentraîner si les données sont déjà importées
                    if (ensembleTraining.Taille() > 0)
                    {
                        knn.Entrainer(ensembleTraining);
                    }

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }


                // Entrainer le modèle 

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

                        Table recap = new Table();
                        recap.AddColumn("Paramètre");
                        recap.AddColumn("Valeur");

                        recap.AddRow("Nombre d'échantillons", ensembleTraining.Taille().ToString());
                        recap.AddRow("k", "3");
                        recap.AddRow("Distance", distanceChoisie.GetType().Name);

                        AnsiConsole.Write(recap);

                        AnsiConsole.Markup("[green]Modèle entraîné avec succès ![/]\n");
                    }

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }

                
                // Tester le modèle
                
                else if (choix == "Tester modèle")
                {
                    if (ensembleTraining.Taille() == 0)
                    {
                        AnsiConsole.Markup("[red]Erreur : modèle non entraîné ![/]\n");
                        Console.ReadKey();
                        continue;
                    }

                    AnsiConsole.Markup("[blue]Chargement des données de test...[/]\n");

                    List<Grain> grainsTest = dataLoader.conversion_liste(fichierTest);

                    // La matrice de confusion 3x3
                    int[,] matrice = new int[3, 3];
                    int correct = 0;
                    int total = grainsTest.Count;

                    // Tableau de prediction
                    Table tableauPredictions = new Table();
                    tableauPredictions.AddColumn("Réel");
                    tableauPredictions.AddColumn("Prédit");
                    tableauPredictions.AddColumn("Area");
                    tableauPredictions.AddColumn("Perimeter");
                    tableauPredictions.AddColumn("Compactness");

                    foreach (Grain g in grainsTest)
                    {
                        double[] carac = new double[]
                        {
                            g.Area, 
                            g.Perimeter, 
                            g.Compactness,
                            g.LongueurNoyau, 
                            g.LargeurNoyau,
                            g.AsymetryCoefficient, 
                            g.GrooveLength
                        };

                        string prediction = knn.Predire(carac);

                        // Conversion des classes en indices 0–1–2
                        int vrai = ClasseToIndex(g.TypeDeGrain.ToString());
                        int predit = ClasseToIndex(prediction);

                        matrice[vrai, predit]++;

                        if (vrai == predit)
                            correct++;

                        // Ajouter tableau
                        tableauPredictions.AddRow(
                            g.TypeDeGrain.ToString(),
                            prediction,
                            g.Area.ToString(),
                            g.Perimeter.ToString(),
                            g.Compactness.ToString()
                        );
                    }

                    double exactitude = (double)correct / total;

                    // Afficher le tableau de prediction
                    AnsiConsole.Write(new Rule("[yellow]Prédictions détaillées[/]"));
                    AnsiConsole.Write(tableauPredictions);

                    // Afficher la matrice de confusion
                    Table t = new Table();
                    t.AddColumn(" ");
                    t.AddColumn("Prédit 1 (Kama)");
                    t.AddColumn("Prédit 2 (Rosa)");
                    t.AddColumn("Prédit 3 (Canadian)");

                    t.AddRow("Réel 1 (Kama)", matrice[0, 0].ToString(), matrice[0, 1].ToString(), matrice[0, 2].ToString());
                    t.AddRow("Réel 2 (Rosa)", matrice[1, 0].ToString(), matrice[1, 1].ToString(), matrice[1, 2].ToString());
                    t.AddRow("Réel 3 (Canadian)", matrice[2, 0].ToString(), matrice[2, 1].ToString(), matrice[2, 2].ToString());

                    AnsiConsole.Write(new Rule("[yellow]Matrice de confusion[/]"));
                    AnsiConsole.Write(t);

                    AnsiConsole.Markup($"\n[green]Exactitude : {exactitude:P2}[/]\n");

                    // Conversion de la matrice en json
                    int[][] matriceJson = new int[3][];
                    for (int i = 0; i < 3; i++)
                    {
                        matriceJson[i] = new int[3];
                        for (int j = 0; j < 3; j++)
                            matriceJson[i][j] = matrice[i, j];
                    }

                    // Enregistrer fichier json
                    var resultats = new ResultatsEvaluationPerformances
                    {
                        Exactitude = exactitude,
                        MatriceConfusion = matriceJson,
                        Distance = distanceChoisie.GetType().Name,
                        K = 3,
                        DateEvaluation = DateTime.Now
                    };

                    string json = System.Text.Json.JsonSerializer.Serialize(
                        resultats,
                        new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
                    );

                    System.IO.File.WriteAllText("resultats_evaluation.json", json);

                    AnsiConsole.Markup("[blue]Résultats sauvegardés dans resultats_evaluation.json[/]\n");

                    AnsiConsole.Markup("[grey]Appuie sur une touche pour continuer...[/]");
                    Console.ReadKey();
                }

                
                // Quitter le menu
               
                else if (choix == "Quitter")
                {
                    continuer = false;
                }
            }
        }
    }
}
