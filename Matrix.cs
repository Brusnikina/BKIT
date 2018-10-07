using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    class Matrix<T>
    {
        Dictionary<string, T> _matrix = new Dictionary<string, T>();
        int maxX;
        int maxY;
        int maxZ;
        IMatrixCheckEmpty<T> сheckEmpty;
        public Matrix(int px, int py, int pz, IMatrixCheckEmpty<T> сheckEmptyParam)
        {
            this.maxX = px;
            this.maxY = py;
            this.maxZ = pz;
            this.сheckEmpty = сheckEmptyParam;
        }
        public T this[int x, int y, int z]
        {
            set
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                this._matrix.Add(key, value);
            }
            get
            {
                CheckBounds(x, y, z);
                string key = DictKey(x, y, z);
                if (this._matrix.ContainsKey(key)) { return this._matrix[key]; }
                else { return this.сheckEmpty.getEmptyElement(); }
            }
        }
        void CheckBounds(int x, int y, int z)
        {
            
            if (x < 0 || x >= this.maxX) { throw new ArgumentOutOfRangeException("x", "x=" + x + " выходит за границы"); }
            if (y < 0 || y >= this.maxY) { throw new ArgumentOutOfRangeException("y", "y=" + y + " выходит за границы"); }
            if (z < 0 || z >= this.maxZ) { throw new ArgumentOutOfRangeException("z", "z=" + z + " выходит за границы"); }
        }
        string DictKey(int x, int y, int z) { return x.ToString() + "_" + y.ToString() + "_" + z.ToString(); }
        // i- слой(номер таблицы), j - строка, k - столбец 
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i<this.maxZ; i++)
            {                
                for (int j = 0; j < this.maxY; j++)
                {
                    b.Append("[");
                    for (int k = 0; k < this.maxX; k++)
                    {
                        if (k > 0) { b.Append("\t"); }
                        if (!this.сheckEmpty.checkEmptyElement(this[i, j, k])) { b.Append(this[i, j, k].ToString()); }
                        else { b.Append(" - "); }
                    }
                    b.Append("]\n");
                }
                b.Append("\n***\n\n");                
            }
            return b.ToString();
        }
    }

}

