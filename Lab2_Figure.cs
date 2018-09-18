using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    abstract class Figure
    {
        public string Type { get; protected set; }
        public abstract double Area();

        //public override string ToString()
        //{
        //    return this.Type + " площадью " + this.Area().ToString();
        //}
    }
}
