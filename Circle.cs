using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
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
        public void Print()
        {
            Console.WriteLine(this.ToString());
        }
    }
}
