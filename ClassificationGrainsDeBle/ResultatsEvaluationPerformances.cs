using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassificationGrainsDeBle
{
    internal class ResultatsEvaluationPerformances
    {
        public double Exactitude { get; set; }
        public int[][] MatriceConfusion { get; set; }
        public string Distance { get; set; }
        public int K { get; set; }
        public DateTime DateEvaluation { get; set; }

    }
}
