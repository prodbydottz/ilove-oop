using System;

namespace DesignPatternsLab13.Task2
{
    public static class Task2Demo
    {
        public static void Run()
        {
            Console.WriteLine("║  Задание №2: Система управления заявками на билеты           ║");
            Console.WriteLine();

            BookingRequestContext request = null;

            while (true)
            {
                if (request != null)
                {
                    Console.WriteLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                    request.ShowCurrentState();
                }

                Console.WriteLine("\nМеню действий:");
                Console.WriteLine("1. Создать новую заявку");
                Console.WriteLine("2. Отправить заявку клиенту");
                Console.WriteLine("3. Произвести транзакцию");
                Console.WriteLine("4. Подтвердить бронирование");
                Console.WriteLine("5. Отменить заявку");
                Console.WriteLine("6. Показать историю заявки");
                Console.WriteLine("7. Проверить срок оплаты");
                Console.WriteLine("0. Вернуться в главное меню");
                Console.Write("\nВыберите действие: ");

                var choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        request = CreateNewRequest();
                        break;

                    case "2":
                        if (request != null)
                        {
                            request.SendToClient();
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "3":
                        if (request != null)
                        {
                            request.MakePayment();
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "4":
                        if (request != null)
                        {
                            request.ConfirmBooking();
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "5":
                        if (request != null)
                        {
                            request.CancelRequest();
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "6":
                        if (request != null)
                        {
                            request.ShowHistory();
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "7":
                        if (request != null)
                        {
                            CheckPaymentDeadline(request);
                        }
                        else
                        {
                            Console.WriteLine("Создайте заявку.");
                        }
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        private static BookingRequestContext CreateNewRequest()
        {
            Console.Write("Введите имя клиента: ");
            string clientName = Console.ReadLine();

            Console.Write("Введите стоимость билета: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                var request = new BookingRequestContext(clientName, price);
                Console.WriteLine($"\nСоздана новая заявка {request.RequestId}");
                return request;
            }
            else
            {
                Console.WriteLine("Неверная стоимость!");
                return null;
            }
        }

        private static void CheckPaymentDeadline(BookingRequestContext request)
        {
            if (!request.PaymentDeadline.HasValue)
            {
                Console.WriteLine("Срок оплаты не установлен. Отправьте заявку клиенту.");
                return;
            }

            if (request.IsPaymentExpired())
            {
                Console.WriteLine("Attention: Срок оплаты истек!");
                Console.WriteLine("   При попытке оплаты заявка будет автоматически отменена.");
            }
            else
            {
                var timeLeft = request.PaymentDeadline.Value - DateTime.Now;
                Console.WriteLine($"До истечения срока оплаты осталось:");
                Console.WriteLine($"   {timeLeft.Minutes} минут {timeLeft.Seconds} секунд");
            }
        }
    }
}
