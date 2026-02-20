using ClassificationGrainsDeBle.ClassificationGrainsDeBle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    //internal class FermeGrain : Ferme
    //{
    //    List<LotDeGrain> lots;

    //    public FermeGrain(List<LotDeGrain> lots)
    //    {
    //        lots = lots;
    //       
    //    }

    //    public List<LotDeGrain> Lots
    //    {
    //        get { return lots; }
    //    }

    //    public override void AjouterLot(LotDeGrain ajoutLotDeGrain)
    //    {
    //        lots.Add(ajoutLotDeGrain);
    //    }


    internal class FermeGrain : Ferme
    {
        private List<LotDeGrain> lots;

        public FermeGrain(string nom, string adresse, List<LotDeGrain> lots)
            : base(nom, adresse)
        {
            this.lots = lots;
        }

        public List<LotDeGrain> GetLots()
        {
            return lots;
        }

        public override void AjouterLot(LotDeGrain ajoutLotDeGrain)
        {
            lots.Add(ajoutLotDeGrain);
        }
    }
}




