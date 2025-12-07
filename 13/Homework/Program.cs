using DesignPatternsModuleHomework13.Classes.Models;
using DesignPatternsModuleHomework13.Classes.Services;
using DesignPatternsModuleHomework13.Classes.StateMachine;

namespace DesignPatternsModuleHomework13.Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new TicketCatalogService();
            catalog.Seed(
                new TicketType("Child", "Детский", 50m),
                new TicketType("Standard", "Стандартный", 100m),
                new TicketType("Student", "Студентческий", 90m)
            );

            var inventory = new InventoryService();
            inventory.SeedStock(new[]
            {
                (catalog.Find("Standard")!, 10),
                (catalog.Find("Student")!, 5),
                (catalog.Find("Child")!, 3)
            });

            var payment = new PaymentService();
            var machine = new TicketMachine(payment, inventory);

            Console.WriteLine("=== Билетная Касса ===");
            Console.WriteLine("Доступные позиции:");
            foreach (var ticket in catalog.All())
            {
                Console.WriteLine($"{ticket.Code}: {ticket.Name} ({ticket.Price:C})");
            }

            var script = new (string command, string? argument)[]
            {
                ("select", "Standard"),
                ("insert", "100"),
                ("dispense", null),
                ("select", "Student"),
                ("insert", "50"),
                ("cancel", null),
                ("select", "Child"),
                ("insert", "20"),
                ("insert", "30"),
                ("dispense", null)
            };

            foreach (var (command, argument) in script)
            {
                Console.WriteLine($"\n> {command} {argument}".Trim());

                try
                {
                    switch (command)
                    {
                        case "select":
                            if (string.IsNullOrWhiteSpace(argument))
                            {
                                Console.WriteLine("Номер вашего билета:");
                                continue;
                            }

                            var ticket = catalog.Find(argument);
                            if (ticket is null)
                            {
                                Console.WriteLine("Данный билет отсутствует.");
                                continue;
                            }

                            machine.SelectTicket(ticket);
                            Console.WriteLine($"Выбранный вами билет {ticket.Name}. Цена {ticket.Price:C}");
                            break;

                        case "insert":
                            if (string.IsNullOrWhiteSpace(argument) || !decimal.TryParse(argument, out var amount))
                            {
                                Console.WriteLine("Некорректная сумма.");
                                continue;
                            }

                            machine.InsertMoney(amount);
                            break;

                        case "dispense":
                            machine.DispenseTicket();
                            break;

                        case "cancel":
                            machine.Cancel();
                            break;

                        default:
                            Console.WriteLine("Ошибка! Неизвестная команда.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.ReadKey();
        }
    }
}
