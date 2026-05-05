using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClassificationGrainsDeBles.Models
{
    public class SharedModel
    {
        public EnsembleDonnees TrainingData { get; set; } = new EnsembleDonnees();
        public EnsembleDonnees TestData { get; set; } = new EnsembleDonnees();
        public int K { get; set; } = 3;
        public IDistance DistanceStrategy { get; set; }
        public ClassifierKnn Classifier { get; set; }
        public bool IsDataLoaded { get; set; } = false;

        public SharedModel()
        {
            DistanceStrategy = new DistanceEuclidienne();
        }

        public void SetDistance(string distanceName)
        {
            if (distanceName == "Manhattan")
                DistanceStrategy = new DistanceManhattan();
            else
                DistanceStrategy = new DistanceEuclidienne();
        }

        public string GetDistanceName()
        {
            return DistanceStrategy is DistanceManhattan ? "Manhattan" : "Euclidienne";
        }

        public void InitializeClassifier()
        {
            if (TrainingData != null && TrainingData.Taille() > 0)
            {
                Classifier = new ClassifierKnn(K, DistanceStrategy);
                Classifier.Entrainer(TrainingData);
            }
        }
    }
}

