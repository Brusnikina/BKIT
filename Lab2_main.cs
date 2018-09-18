using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            Rectangle rec = new Rectangle(4, 5);
            Square sc = new Square(5);
            Circle cir = new Circle(5);

            rec.Print();
            sc.Print();
            cir.Print();

            Console.ReadKey();
        }
    }
}
