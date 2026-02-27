using Spectre.Console;
using System;
using System.Collections.Generic;

namespace ClassificationGrainsDeBle
{
    internal class ClassifierKnn : IClassifier
    {
        IDistance distance;
        EnsembleDonnees training;
        int k;

        public ClassifierKnn(int k, IDistance distance)
        {
            this.k = k;
            this.distance = distance;
        }

        public void Entrainer(EnsembleDonnees donnees)
        {
            training = donnees;
        }

        public List<Voisin> ListeTousVoisin(double[] caracteristiques)
        {
            List<Voisin> liste = new List<Voisin>();

            foreach (Echantillon e in training.ObtenirEchantillon())
            {
                double d = distance.Calculer(caracteristiques, e.Caracteristiques);
                liste.Add(new Voisin(e, d));
            }

            return liste;
        }

        public List<Voisin> TrierListVoisinParDistance(List<Voisin> voisins)
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

            resultat.AddRange(TrierListVoisinParDistance(plusPetits));
            resultat.Add(pivot);
            resultat.AddRange(TrierListVoisinParDistance(plusGrands));

            return resultat;
        }

        public TypeDeGrain VoteMajoritaire(List<Voisin> voisins)
        {
            TypeDeGrain typeDuGrain = new TypeDeGrain();
            int compteurKama = 0;
            int compteurRosa = 0;
            int compteurCanadian = 0;

            foreach (var voisin in voisins)
            {
                if (voisin.Echantillon.Etiquette.ToString() == "Kama")
                {
                    compteurKama++;
                }
                if (voisin.Echantillon.Etiquette.ToString() == "Rosa")
                {
                    compteurRosa++;
                }
                if (voisin.Echantillon.Etiquette.ToString() == "Canadian")
                {
                    compteurCanadian++;
                }
            }
            if (compteurKama >= compteurRosa && compteurKama >= compteurCanadian)
                typeDuGrain = TypeDeGrain.Kama;

            else if (compteurRosa >= compteurKama && compteurRosa >= compteurCanadian)
                typeDuGrain = TypeDeGrain.Rosa;

            else
                typeDuGrain = TypeDeGrain.Canadian;

            return typeDuGrain;

        }

        public TypeDeGrain Predire(Echantillon e)
        {
            List<Voisin> voisins = ListeTousVoisin(e.Caracteristiques);
            List<Voisin> voisinsTries = TrierListVoisinParDistance(voisins);

            List<Voisin> kVoisins = new List<Voisin>();
            for (int i = 0; i < k; i++)
            {
                kVoisins.Add(voisinsTries[i]);
            }

            return VoteMajoritaire(kVoisins);

        }
    }
}