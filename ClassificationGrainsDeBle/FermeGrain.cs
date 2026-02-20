using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class FermeGrain: Ferme
    {
        List<LotDeGrain> lots;

        public FermeGrain(List<LotDeGrain> lots)
        {
            lots = lots;
        }

        public List<LotDeGrain> Lots { get { return lots; }}
        public override void AjouterLot(LotDeGrain ajoutLotDeGrain)
        {
            lots.Add(ajoutLotDeGrain);    
        }
    }
}
