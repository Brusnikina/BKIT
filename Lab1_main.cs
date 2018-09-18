using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Вычисление корней квадратного уравнения\nВведите коэффициенты:");          

            double a, b, c;

            Console.Write("A: ");
            while (!double.TryParse(Console.ReadLine(), out a))
                Console.Write("Некорректный ввод!\nА: ");
            Console.Write("B: ");
            while (!double.TryParse(Console.ReadLine(), out b))
                Console.Write("Некорректный ввод!\nB: ");
            Console.Write("C: ");
            while (!double.TryParse(Console.ReadLine(), out c))
                Console.Write("Некорректный ввод!\nC: ");

            Console.WriteLine("Ваше уравнение: {0}x^2 + {1}x + {2} = 0",a,b,c);

            double d = b * b - 4 * a * c;
            double x;
            if (d < 0.0) Console.WriteLine("Уравнение не имеет действительных корней.");
            else
                if (d == 0.0)
                {
                    x = -b / (2 * a);
                    Console.WriteLine("Уравнение имеет единственный корень х = " + x);
                }
                else
                {
                    Console.WriteLine("Уравнение имеет два корня:");
                    x = (-b + Math.Sqrt(d)) / (2 * a);
                    Console.WriteLine("x_1 = " + x);
                    x = (-b - Math.Sqrt(d)) / (2 * a);
                    Console.WriteLine("x_2 = " + x);
                }
            
            Console.ReadKey();
        }
    }
}
