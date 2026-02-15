namespace ClassificationGrainsDeBle
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // code non terminé !!!

            var data = new EnsembleDonnees();
            data.Ajouter(new Echantillon(new double[] { 1, 2 }, "A"));
            data.Ajouter(new Echantillon(new double[] { 2, 3 }, "A"));
            data.Ajouter(new Echantillon(new double[] { 8, 9 }, "B"));

            var knn = new ClassifierKnn(3, new DistanceEuclidienne());
            knn.Entrainer(data);

            string resultat = knn.Predire(new double[] { 1.5, 2 });
            Console.WriteLine("Classe prédite : " + resultat);

        }
    }
}
