using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Circle :Figure, IPrint
    {
       public double radius { get; protected set; }

        public Circle(double r)
        {
            this.radius = r;
            this.Type = "Круг";
        }
        public override double Area()
        {
            return Math.PI * this.radius * this.radius;
        }
        public override string ToString()
        {
            return this.Type + " площадью " + this.Area()
                + " с радиусом " + this.radius.ToString();
        }
        public void Print()
        {
            Console.WriteLine(this.ToString());
        }
    }
}
