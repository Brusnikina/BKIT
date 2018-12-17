using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7
{ 
    class Program
    {
        static List<Worker> workers = new List<Worker>()
        {
            new Worker(1,"Алексеев Алексей Алексеевич",1),
            new Worker(2,"Фёдоров Фёдор Фёдорович",1),
            new Worker(3,"Андреев Андрей Андреевич",2),
            new Worker(4,"Александров Александр Александрович",2),
            new Worker(5,"Васильев Василий Васильевич",1),
            new Worker(6,"Петров Пётр Петрович",3),
            new Worker(7,"Иванов Иван Иванович",3)
        };
        static List<Office> offices = new List<Office>()
        {
            new Office(1,"Отдел кадров"),
            new Office(2,"Отдел маркетинга"),
            new Office(3,"Бухгалтерия")
        };
        static List<WorkersOfOffice> records = new List<WorkersOfOffice>()
        {
            new WorkersOfOffice(1,1),            
            new WorkersOfOffice(1,3),
            new WorkersOfOffice(2,1),
            new WorkersOfOffice(3,2),
            new WorkersOfOffice(3,3),
            new WorkersOfOffice(4,2),
            new WorkersOfOffice(5,1),
            new WorkersOfOffice(5,2),
            new WorkersOfOffice(5,3),
            new WorkersOfOffice(6,2),
            new WorkersOfOffice(6,3),
            new WorkersOfOffice(7,1),
            new WorkersOfOffice(7,3)
        };
        static void Main(string[] args)
        {
            //Список всех сотрудников и отделов, отсортированный по отделам
            Console.WriteLine("Список всех сотрудников и отделов, отсортированный по отделам");
            var q1 = from x in workers                    
                     orderby x.id_of_office
                     select x;
            foreach (var x in q1)
                Console.WriteLine("ID отдела: {0}, ID сотрудника: {1}, ФИО отрудника: {2}",
                x.id_of_office, x.id_of_worker, x.fio_of_worker);

            //Список всех сотрудников, у которых фамилия начинается с буквы «А»
            Console.WriteLine("\nСписок всех сотрудников, у которых фамилия начинается с буквы «А»");
            var q2 = from x in workers
                     where (x.fio_of_worker)[0] == 'А'
                     select x;
            foreach (var x in q2)
                Console.WriteLine("ID сотрудника: {0}, ФИО сотрудника: {1}, ID отдела: {2}",
               x.id_of_worker, x.fio_of_worker, x.id_of_office);

            //Список всех отделов и количество сотрудников в каждом отделе
            //Console.WriteLine("Список всех отделов и количество сотрудников в каждом отделе");
            //var q3 = from x in offices                         
            //         select x;
            //foreach (var x in q3)
            //{
            //    Console.Write("ID отдела: {0}, название отдела: {1}, количество сотрудников: ", x.id_of_office, x.name_of_office);
            //    Console.WriteLine(workers.Count(w => w.id_of_office == x.id_of_office));
            //}

            Console.WriteLine("\nСписок всех отделов и количество сотрудников в каждом отделе");
            var q33 = from x in offices
                      join y in workers on x.id_of_office equals y.id_of_office
                      group x by x.id_of_office into g
                      select new { id = g.Key, count = g.Count() };
            foreach (var g in q33) Console.WriteLine("ID отдела: {0} , кол-во сотрудников: {1}", g.id, g.count);        

            //Список отделов, в которых у всех сотрудников фамилия начинается с буквы «А»

            //Console.WriteLine("Список отделов, в которых у всех сотрудников фамилия начинается с буквы «А»");
            //var q4 = from x in offices
            //         join y in workers on x.id_of_office equals y.id_of_office
            //         group y by y.id_of_office;

            Console.WriteLine("\nСписок отделов, в которых у всех сотрудников фамилия начинается с буквы «А»");
            var q4 = from x in workers
                     group x by x.id_of_office into g
                     where g.All(x => x.fio_of_worker[0] == 'А')
                     select new { id = g.Key };
            foreach (var x in q4) Console.WriteLine("ID отдела: " + x.id);


            //var q4 = from x in workers
            //         group x by x.id_of_office into g
            //         select new { ggg = g.All(t => t.fio_of_worker[0] == 'А') };
            //foreach (var x in q4) Console.WriteLine("ID отдела: " + x.ggg);


            //foreach (var x in q4)
            //{
            //    Console.WriteLine("ID отдела: " + x.Key);
            //    foreach (var y in x) Console.WriteLine(y.fio_of_worker);
            //}


            //Список отделов, в которых хотя бы у одного сотрудника фамилия начинается с буквы «А»
            Console.WriteLine("\nСписок отделов, в которых хотя бы у одного сотрудника фамилия начинается с буквы «А»");
            var q5 = from x in workers
                     group x by x.id_of_office into g
                     where g.Any(x => x.fio_of_worker[0] == 'А')
                     select new { id = g.Key };
            foreach (var x in q5) Console.WriteLine("ID отдела: " + x.id);


            //МНОГИЕ КО МНОГИМ
            //Список всех отделов и список сотрудников в каждом отделе 
            //Console.WriteLine("Список всех отделов и список сотрудников в каждом отделе");
            //var m_to_m1 = from x in offices
            //              join l in records on x.id_of_office equals l.id_of_office_data into temp
            //              from t1 in temp
            //              join y in workers on t1.id_of_worker_data equals y.id_of_worker into temp2
            //              from t2 in temp2
            //              orderby t2.id_of_office
            //              select new { id1 = x.id_of_office, id2 = t2.id_of_worker };
            //foreach (var x in m_to_m1) Console.WriteLine(x.id1 + "   " + x.id2);

            Console.WriteLine("\nМНОГИЕ КО МНОГИМ");
            Console.WriteLine("\nСписок всех отделов и список сотрудников в каждом отделе");
            var m_to_m1 = from x in offices
                          join l in records on x.id_of_office equals l.id_of_office_data into temp
                          from t1 in temp
                          join y in workers on t1.id_of_worker_data equals y.id_of_worker into temp2
                          from t2 in temp2
                          group t2 by x.id_of_office;
            //select new { id1 = x.id_of_office, id2 = t2.id_of_worker };
            //foreach (var x in m_to_m1) Console.WriteLine(x);
            foreach (var x in m_to_m1)
            {
                Console.WriteLine("ID отдела: " + x.Key);
                foreach (var y in x) Console.WriteLine("ID сотрудника: {0}, ФИО сотрудника: {1}", y.id_of_worker, y.fio_of_worker);
            }
           
            //Console.WriteLine("Список всех отделов и количество сотрудников в каждом отделе");
            //var m_to_m2 = from x in offices
            //              join l in records on x.id_of_office equals l.id_of_office_data into temp
            //              from t1 in temp
            //              join y in workers on t1.id_of_worker_data equals y.id_of_worker into temp2
            //              from t2 in temp2
            //              group t2 by t2.id_of_office into g
            //              select new { id = g.Key, count = g.Count() };
            //foreach (var x in m_to_m2) Console.WriteLine("ID отдела: {0}, кол-во сотрудников: {1}", x.id, x.count);

            //Список всех отделов и количество сотрудников в каждом отделе
            Console.WriteLine("\nСписок всех отделов и количество сотрудников в каждом отделе");
            var m_to_m2 = from x in m_to_m1
                          select new { id = x.Key, count = x.Count() };
            foreach (var x in m_to_m2) Console.WriteLine("ID отдела: {0}, кол-во сотрудников: {1}", x.id, x.count);


            Console.ReadKey();
        }
    }
}
