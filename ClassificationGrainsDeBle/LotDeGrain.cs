using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class LotDeGrain
    {
        string idLot;
        DateTime dateRecolte;
        float poidsTotalKg;
        List<Grain> lotDeGrains;

        public LotDeGrain(string idLot, DateTime dateRecolte, float poidsTotalKg, List<Grain> lotDeGrains)
        {
            idLot = idLot;
            DateRecolte = dateRecolte;
            PoidsTotalKg = poidsTotalKg;
            LotDeGrains = lotDeGrains;
            IdLot = idLot;
            DateRecolte = dateRecolte;
            PoidsTotalKg = poidsTotalKg;
            LotDeGrains = lotDeGrains;
        }

        public string IdLot
        {
            get { return idLot; }
            set { idLot = value; }
        }

        public DateTime DateRecolte
        {
            get { return dateRecolte; }
            set { dateRecolte = value; }
        }

        public float PoidsTotalKg
        {
            get { return poidsTotalKg; }
            set { poidsTotalKg = value; }
        }

        public List<Grain> LotDeGrains
        {
            get { return lotDeGrains; }
            set { lotDeGrains = value; }
        }
    }
}
