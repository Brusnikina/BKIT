using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class FigureMatrixCheckEmpty : IMatrixCheckEmpty<Figure>
    {
        public Figure getEmptyElement() { return null; }
        public bool checkEmptyElement(Figure element)
        {
            bool Result = false;
            if (element == null) { Result = true; }
            return Result;
        }
    }
}
