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
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  Модуль 13: Диаграммы состояний и вариантов использования  ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
                Console.WriteLine("\nВыберите задание:");
                Console.WriteLine("1. Задание №1 - Система бронирования номеров в гостинице");
                Console.WriteLine("2. Задание №2 - Система управления заявками на билеты");
                Console.WriteLine("3. Задание №3 - Система управления онлайн-курсами");
                Console.WriteLine("0. Выход");
                Console.Write("\nВаш выбор: ");

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
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
                
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
