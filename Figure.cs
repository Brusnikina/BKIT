using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    abstract class Figure : IComparable
    {
        public string Type { get; protected set; }
        public abstract double Area();
        public override string ToString()
        {
            return this.Type + " площадью " + this.Area().ToString();
        }
        public int CompareTo (object obj)
        {
            Figure f = (Figure)obj;
            if (this.Area() < f.Area()) return -1;
            else if (this.Area() == f.Area()) return 0;
                 else return 1;
        }

    }
}
