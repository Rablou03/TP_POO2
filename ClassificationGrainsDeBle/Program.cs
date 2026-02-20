namespace ClassificationGrainsDeBle
{
    internal class Program
    {
        static void Main(string[] args)
        {

            
            // Charger les données d'entraînement
           

            Data dataLoader = new Data();

            string fichierTraining = "seeds_dataset_training.csv";
            string fichierTest = "seeds_dataset_test.csv";

            Console.WriteLine("Chargement des données d'entraînement");
            List<Grain> grainsTraining = dataLoader.conversion_liste(fichierTraining);

            EnsembleDonnees ensembleTraining = new EnsembleDonnees();

            foreach (var g in grainsTraining)
            {
                double[] caracteristiques = new double[]
                {
                    g.Area,
                    g.Perimeter,
                    g.Compactness,
                    g.LongueurNoyau,
                    g.LargeurNoyau,
                    g.AsymetryCoefficient,
                    g.GrooveLength
                };

                ensembleTraining.Ajouter(new Echantillon(caracteristiques, g.TypeDeGrain.ToString()));
            }

            
            // Entraîner le classifieur KNN
            

            Console.WriteLine("Entraînement du modèle KNN");
            ClassifierKnn knn = new ClassifierKnn(3, new DistanceEuclidienne());
            knn.Entrainer(ensembleTraining);

           
            // Charger les données de test
           

            Console.WriteLine("Chargement des données de test");
            List<Grain> grainsTest = dataLoader.conversion_liste(fichierTest);

           
            // Prédire pour chaque grain de test
            

            Console.WriteLine("PRÉDICTIONS");

            foreach (var g in grainsTest)
            {
                double[] caracteristiques = new double[]
                {
                    g.Area,
                    g.Perimeter,
                    g.Compactness,
                    g.LongueurNoyau,
                    g.LargeurNoyau,
                    g.AsymetryCoefficient,
                    g.GrooveLength
                };

                string prediction = knn.Predire(caracteristiques);

                Console.WriteLine(
                    $"Réel : {g.TypeDeGrain} | Prédit : {prediction} | " +
                    $"Area={g.Area}, Perimeter={g.Perimeter}, Compactness={g.Compactness}"
                );
            }

            Console.WriteLine("La lassification est terminée.");
        }
    }
}    




    

