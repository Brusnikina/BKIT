using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{
    class Worker
    {
        public int id_of_worker;
        public string fio_of_worker;
        public int id_of_office;
        public Worker(int a, string str, int b)
        {
            this.id_of_worker = a;
            this.fio_of_worker = str;
            this.id_of_office = b;
        }
    }
}
