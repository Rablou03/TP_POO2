using System;
using System.Collections.Generic;

namespace ClassificationGrainsDeBle
{
    internal class ClassifierKnn : IClassifier
    {
        private IDistance distance;
        private EnsembleDonnees donneesApprentissage;
        private int k;

        public ClassifierKnn(int k, IDistance distance)
        {
            this.k = k;
            this.distance = distance;
        }

        public void Entrainer(EnsembleDonnees data)
        {
            donneesApprentissage = data;
        }

        public string Predire(double[] caracteristiques)
        {
            List<Voisin> voisins = CalculerToutesDistances(caracteristiques);
            List<Voisin> voisinsTries = TrierVoisinsParDistance(voisins);

            List<Voisin> kVoisins = new List<Voisin>();
            for (int i = 0; i < k; i++)
            {
                kVoisins.Add(voisinsTries[i]);
            }

            return VoteMajoritaire(kVoisins);
        }

        private List<Voisin> CalculerToutesDistances(double[] caracteristiques)
        {
            List<Voisin> liste = new List<Voisin>();

            foreach (Echantillon e in donneesApprentissage.ObtenirEchantillon())
            {
                double d = distance.Calculer(caracteristiques, e.Caracteristiques);
                liste.Add(new Voisin(e, d));
            }

            return liste;
        }

        private List<Voisin> TrierVoisinsParDistance(List<Voisin> voisins)
        {
            if (voisins.Count <= 1)
                return voisins;

            Voisin pivot = voisins[0];

            List<Voisin> plusPetits = new List<Voisin>();
            List<Voisin> plusGrands = new List<Voisin>();

            for (int i = 1; i < voisins.Count; i++)
            {
                if (voisins[i].Distance < pivot.Distance)
                    plusPetits.Add(voisins[i]);
                else
                    plusGrands.Add(voisins[i]);
            }

            List<Voisin> resultat = new List<Voisin>();

            resultat.AddRange(TrierVoisinsParDistance(plusPetits));
            resultat.Add(pivot);
            resultat.AddRange(TrierVoisinsParDistance(plusGrands));

            return resultat;
        }

        private string VoteMajoritaire(List<Voisin> voisins)
        {
            Dictionary<string, int> compteur = new Dictionary<string, int>();

            foreach (Voisin v in voisins)
            {
                string etiquette = v.Echantillon.Etiquette;

                if (!compteur.ContainsKey(etiquette))
                    compteur[etiquette] = 0;

                compteur[etiquette]++;
            }

            string meilleur = null;
            int max = -1;

            foreach (var pair in compteur)
            {
                if (pair.Value > max)
                {
                    max = pair.Value;
                    meilleur = pair.Key;
                }
            }

            return meilleur;
        }
    }
}