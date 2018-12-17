using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    delegate int Calc(string s, int i1, int i2, bool absolute);
    class Program
    {
        static int Mult(string str, int a, int b, bool absolute)
        {
            Console.Write(str);
            if (absolute) return Math.Abs(a * b);
            else return a * b;
        }
        static int Plus(string str, int a, int b, bool absolute)
        {
            Console.Write(str);
            if (absolute) return Math.Abs(a + b);
            else return a + b;
        }
        static void Print(int a, int b, bool ab, string str, Calc func)
        {            
            Console.WriteLine("Параметры: a = " + a.ToString() + ", b = " + b.ToString());          
            Console.WriteLine("\nРезультат:" + (func(str, a, b, ab)).ToString());
        }
        static void Print2(int a, int b, bool ab, string str, Action<string, int, int, bool> act_param)
        {
            Console.WriteLine("Параметры: a = " + a.ToString() + ", b = " + b.ToString());
            act_param(str, a, b, true);
        }
        static void Main(string[] args)
        {
            Print(5, -10, true, "Умножение", Mult);
            Print(-10, 5, false, "Вычитание", 
                (str, x, y, absolute) => 
                {
                    Console.Write(str);
                    if (absolute) return Math.Abs(x - y);
                    else return x - y;
                }
                );

            Action<string, int, int, bool> act1 = (str, x, y, ab) =>
                                                {
                                                    Console.Write(str);
                                                    if (ab) Console.WriteLine(Math.Abs(x*y));
                                                    else Console.WriteLine(x*y);
                                                };           
            Action<string, int, int, bool> act2 = (str, x, y, ab) =>
                                                {
                                                    Console.Write(str);
                                                    if (ab) Console.WriteLine(Math.Abs(x + y));
                                                    else Console.WriteLine(x + y);
                                                };
            Action<string, int, int, bool> chain = act1 + act2;
            chain("Цепочка ", 3, -8, true);
            Print2(5, -10, true, "Умножение ", act1);

            Console.ReadKey();
        }
    }
}
