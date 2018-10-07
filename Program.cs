using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            Rectangle rec = new Rectangle(4, 5);
            Square sc = new Square(5);
            Circle cir = new Circle(5);
            Circle cir2 = new Circle(2);

            //*** Test 1: ArrayList ***
            ArrayList al = new ArrayList();
            al.Add(cir);
            al.Add(rec);
            al.Add(sc);
            al.Add(cir2);

            Console.WriteLine("Test 1: ArrayList");
            Console.WriteLine("\nДо сортировки:");
            foreach (Figure f in al) Console.WriteLine(f);
            al.Sort();
            Console.WriteLine("\nПосле сортировки:");
            foreach (Figure f in al) Console.WriteLine(f);

            //*** Test 2: List<T> ***
            List<Figure> list = new List<Figure>();
            list.Add(sc);
            list.Add(rec);    
            list.Add(cir);
            list.Add(cir2);

            Console.WriteLine("\n\nTest 2: List<T>");
            Console.WriteLine("\nДо сортировки:");
            foreach (Figure f in list) Console.WriteLine(f);
            list.Sort();
            Console.WriteLine("\nПосле сортировки:");
            foreach (Figure f in list) Console.WriteLine(f);

            //*** Test 3: Matrix<T> ***
            Matrix<Figure> matrix = new Matrix<Figure>(3, 3, 3, new FigureMatrixCheckEmpty());
            matrix[0, 0, 0] = rec;
            matrix[1, 1, 1] = sc;
            matrix[2, 2, 2] = cir;
            matrix[2, 0, 2] = cir2;
            Console.WriteLine("\n\nTest 3: Matrix<T>\n");
            Console.WriteLine(matrix.ToString());

            //*** Test 4: SimpleList<T> ***
            SimpleList<Figure> simple_list = new SimpleList<Figure>();
            simple_list.Add(cir);
            simple_list.Add(rec);
            simple_list.Add(cir2);
            simple_list.Add(sc);
            Console.WriteLine("Test 4: SimpleList<T>");
            Console.WriteLine("\nДо сортировки:");
            foreach (var x in simple_list) Console.WriteLine(x); 
            simple_list.Sort();
            Console.WriteLine("\nПосле сортировки:");
            foreach (var x in simple_list) Console.WriteLine(x);

            //*** Test 5: SimpleStack<T> ***
            SimpleStack<Figure> stack = new SimpleStack<Figure>();           
            stack.Push(rec);
            stack.Push(sc);
            stack.Push(cir2);
            stack.Push(cir);
            Console.WriteLine("\nTest 5: SimpleStack<T>\n");
            while (stack.Count > 0)
            {
                Figure f = stack.Pop();
                Console.WriteLine(f);
            }

            Console.ReadKey();
        }
    }
}
