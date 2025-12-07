using System;
using DesignPatternsLab13.Task1;
using DesignPatternsLab13.Task2;
using DesignPatternsLab13.Task3;

namespace DesignPatternsLab13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            while (true)
            {
                Console.WriteLine("Диаграммы состояний и вариантов применения");
                Console.WriteLine("\nВыберите необходимое вам задание:");
                Console.WriteLine("1. Задание 1 — Система бронирования гостиничных номеров");
                Console.WriteLine("2. Задание 2 — Управление заявками на билеты");
                Console.WriteLine("3. Задание 3 — Управление системой онлайн-курсов");
                Console.WriteLine("0. Завершить работу");
                Console.Write("\nВведите пункт меню: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Task1Demo.Run();
                        break;
                    case "2":
                        Task2Demo.Run();
                        break;
                    case "3":
                        Task3Demo.Run();
                        break;
                    case "0":
                        Console.WriteLine("Работа программы завершена.");
                        return;
                    default:
                        Console.WriteLine("Некорректный пункт меню. Повторите попытку.");
                        break;
                }
                
                Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
