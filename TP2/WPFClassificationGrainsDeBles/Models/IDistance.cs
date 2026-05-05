using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClassificationGrainsDeBles.Models
{
    public interface IDistance
    {
        double Calculer(double[] a, double[] b);
    }
}
