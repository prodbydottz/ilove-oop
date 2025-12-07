using System;

namespace DesignPatternsLab13.Task1
{
    /// <summary>
    /// Демонстрация работы системы бронирования гостиницы
    /// </summary>
    public static class Task1Demo
    {
        public static void Run()
        {
            Console.WriteLine("║  Задание 1: Система бронирования номеров в гостинице        ║");
            Console.WriteLine();

            var booking = new HotelBookingContext();

            while (true)
            {
                Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                booking.ShowCurrentState();
                Console.WriteLine("\nМеню действий:");
                Console.WriteLine("1. Выбрать номер");
                Console.WriteLine("2. Подтвердить бронирование");
                Console.WriteLine("3. Произвести транзакцию");
                Console.WriteLine("4. Отменить бронирование");
                Console.WriteLine("5. Изменить номер");
                Console.WriteLine("6. Применить скидочный купон");
                Console.WriteLine("7. Показать историю");
                Console.WriteLine("8. Начать новое бронирование");
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("\nВыберите действие: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите номер комнаты (например, 101, 205, 301): ");
                        string room = Console.ReadLine();
                        booking.SelectRoom(room);
                        break;

                    case "2":
                        booking.ConfirmBooking();
                        break;

                    case "3":
                        Console.Write("Введите сумму оплаты: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                        {
                            booking.MakePayment(amount);
                        }
                        else
                        {
                            Console.WriteLine("Неверная сумма.");
                        }
                        break;

                    case "4":
                        booking.CancelBooking();
                        break;

                    case "5":
                        Console.Write("Введите новый номер комнаты: ");
                        string newRoom = Console.ReadLine();
                        booking.ChangeRoom(newRoom);
                        break;

                    case "6":
                        ApplyDiscount(booking);
                        break;

                    case "7":
                        booking.ShowHistory();
                        break;

                    case "8":
                        booking = new HotelBookingContext();
                        Console.WriteLine("Создано новое бронирование.");
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        private static void ApplyDiscount(HotelBookingContext booking)
        {
            if (booking.TotalAmount == 0)
            {
                Console.WriteLine("❌ Скидка применяется только после подтверждения бронирования.");
                return;
            }

            Console.WriteLine("\n🎁 Выберите скидку:");
            Console.WriteLine("1. 5% - Первый раз у нас?");
            Console.WriteLine("2. 10% - Постоянный клиент");
            Console.WriteLine("3. 15% - VIP клиент");
            Console.WriteLine("4. 20% - Новогоднее предложение");
            Console.Write("\nВыберите вариант: ");

            var choice = Console.ReadLine();
            decimal discount = 0;

            switch (choice)
            {
                case "1":
                    discount = 0.05m;
                    Console.WriteLine("Применена скидка 5% - Первый раз у нас?");
                    break;
                case "2":
                    discount = 0.10m;
                    Console.WriteLine("Применена скидка 10% - Постоянный клиент");
                    break;
                case "3":
                    discount = 0.15m;
                    Console.WriteLine("Применена скидка 15% - VIP клиент");
                    break;
                case "4":
                    discount = 0.20m;
                    Console.WriteLine("Применена скидка 20% - Новогоднее предложение");
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    return;
            }

            booking.Discount = discount;
            booking.AddToHistory($"Применена скидка: {discount * 100}%");
            
            decimal originalAmount = booking.TotalAmount;
            decimal finalAmount = booking.CalculateFinalAmount();
            Console.WriteLine($"Исходная сумма: {originalAmount:C}");
            Console.WriteLine($"Сумма со скидкой: {finalAmount:C}");
            Console.WriteLine($"Сэкономлено: {(originalAmount - finalAmount):C}");
        }
    }
}
