using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClassificationGrainsDeBles.Models
{
    public class Voisin
    {
        public Echantillon Echantillon { get; set; }
        public double Distance { get; set; }

        public Voisin(Echantillon e, double d)
        {
            Echantillon = e;
            Distance = d;
        }
    }
}
