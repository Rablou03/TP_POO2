namespace ClassificationGrainsDeBle
{
    internal class Grain
    {
        TypeDeGrain typeDeGrain;
        double area;
        double perimeter;
        double compactness;
        double longueurNoyau;
        double largeurNoyau;
        double asymetryCoefficient;
        double grooveLength;

        public Grain(TypeDeGrain typeDeGrain, double area, double perimeter, double compactness, double longueurNoyau, double largeurNoyau, double asymetryCoefficient, double grooveLength)
        {
            this.typeDeGrain = typeDeGrain;
            this.area = area;
            this.perimeter = perimeter;
            this.compactness = compactness;
            this.longueurNoyau = longueurNoyau;
            this.largeurNoyau = largeurNoyau;
            this.asymetryCoefficient = asymetryCoefficient;
            this.grooveLength = grooveLength;
        }

        public TypeDeGrain TypeDeGrain { get { return typeDeGrain; } set { typeDeGrain = value; } }
        public double Area { get { return area; } set { area = value; } }
        public double Perimeter { get { return perimeter; } set { perimeter = value; } }
        public double Compactness { get { return compactness; } set { compactness = value; } }
        public double LongueurNoyau { get { return longueurNoyau; } set { longueurNoyau = value; } }
        public double LargeurNoyau { get { return largeurNoyau; } set { largeurNoyau = value; } }
        public double AsymetryCoefficient { get { return asymetryCoefficient; } set { asymetryCoefficient = value; } }
        public double GrooveLength { get { return grooveLength; } set { grooveLength = value; } }


    }


}

