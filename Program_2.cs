using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_v2
{
    class Program
    {
        public static bool GetAtr(PropertyInfo check_type, Type atr_type, out object atr)
        {
            bool result = false;
            atr = null;
            var is_atr = check_type.GetCustomAttributes(atr_type, false);
            if (is_atr.Length > 0)
            {
                result = true;
                atr = is_atr[0];
            }
            return result;
        }
        static void InvokeMemberInfo()
        {
            Type t = typeof(Test);
            Console.WriteLine("\nВызов метода через рефлексию:");
            
            //Создание объекта через рефлексию
            Test test = (Test)t.InvokeMember(null, BindingFlags.CreateInstance, null, null, new object[] { });
            
            object[] parameter = new object[] { 5 };            
            //Вызов метода
            object result = t.InvokeMember("sq", BindingFlags.InvokeMethod, null, test, parameter);
            Console.WriteLine("5 в степени 2 равно {0}", result);
        }
        static void AtrInfo()
        {
            Type t = typeof(Test);
            Console.WriteLine("\nСвойства, помеченные атрибутом:");
            foreach (var x in t.GetProperties())
            {
                object atr_obj;
                if (GetAtr(x, typeof(Atr), out atr_obj))
                {
                    Atr atr1 = atr_obj as Atr;
                    Console.WriteLine(x.Name + " - " + atr1.name);
                }
            }
        }
        static void Main(string[] args)
        {
            Test obj = new Test();
            Type t = obj.GetType();

            Console.WriteLine("Тип: " + t.FullName);
            Console.WriteLine("Пространство имён: " + t.Namespace);
            Console.WriteLine("Информация о сборке: " + t.AssemblyQualifiedName);

            Console.WriteLine("Конструкторы: ");
            foreach (var x in t.GetConstructors()) Console.WriteLine(x);
            Console.WriteLine("Методы: ");
            foreach (var x in t.GetMethods()) Console.WriteLine(x);
            Console.WriteLine("Свойства: ");
            foreach (var x in t.GetProperties()) Console.WriteLine(x);
            Console.WriteLine("Поля данных: ");
            foreach (var x in t.GetFields()) Console.WriteLine(x);

            AtrInfo();
            InvokeMemberInfo();

            Console.ReadKey();
        }
    }
}
