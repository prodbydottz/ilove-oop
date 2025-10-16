using System;
using System.Collections.Generic;

public interface ICostCalculationStrategy
{
    decimal CalculateCost(TravelBooking booking);
}

public class TravelBooking
{
    public string TransportationType { get; set; }
    public decimal Distance { get; set; }
    public int PassengerCount { get; set; }
    public string ServiceClass { get; set; }
    public bool HasLuggage { get; set; }
    public decimal LuggageWeight { get; set; }
    public bool IsRoundTrip { get; set; }
    public List<string> AdditionalServices { get; set; }
    public Dictionary<string, decimal> Discounts { get; set; }

    public TravelBooking()
    {
        AdditionalServices = new List<string>();
        Discounts = new Dictionary<string, decimal>();
    }

    public void AddDiscount(string type, decimal value)
    {
        Discounts[type] = value;
    }

    public decimal ApplyDiscounts(decimal baseCost)
    {
        decimal finalCost = baseCost;
        foreach (var discount in Discounts)
        {
            finalCost -= finalCost * discount.Value / 100;
        }
        return finalCost;
    }
}

public class AirplaneCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelBooking booking)
    {
        decimal baseCost = booking.Distance * 0.15m;
        
        switch (booking.ServiceClass.ToLower())
        {
            case "business":
                baseCost *= 2.5m;
                break;
            case "first":
                baseCost *= 4.0m;
                break;
            default:
                baseCost *= 1.0m;
                break;
        }

        if (booking.HasLuggage)
        {
            baseCost += Math.Max(20, booking.LuggageWeight * 2);
        }

        if (booking.IsRoundTrip)
        {
            baseCost *= 1.8m;
        }

        baseCost += booking.AdditionalServices.Count * 15;
        baseCost *= booking.PassengerCount;

        return booking.ApplyDiscounts(baseCost);
    }
}

public class TrainCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelBooking booking)
    {
        decimal baseCost = booking.Distance * 0.08m;
        
        switch (booking.ServiceClass.ToLower())
        {
            case "business":
                baseCost *= 1.8m;
                break;
            case "luxury":
                baseCost *= 3.0m;
                break;
            default:
                baseCost *= 1.0m;
                break;
        }

        if (booking.HasLuggage)
        {
            baseCost += Math.Max(10, booking.LuggageWeight * 1);
        }

        if (booking.IsRoundTrip)
        {
            baseCost *= 1.7m;
        }

        baseCost += booking.AdditionalServices.Count * 8;
        baseCost *= booking.PassengerCount;

        return booking.ApplyDiscounts(baseCost);
    }
}

public class BusCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelBooking booking)
    {
        decimal baseCost = booking.Distance * 0.05m;
        
        if (booking.ServiceClass.ToLower() == "premium")
        {
            baseCost *= 1.5m;
        }

        if (booking.HasLuggage)
        {
            baseCost += Math.Max(5, booking.LuggageWeight * 0.5m);
        }

        if (booking.IsRoundTrip)
        {
            baseCost *= 1.6m;
        }

        baseCost += booking.AdditionalServices.Count * 5;
        baseCost *= booking.PassengerCount;

        return booking.ApplyDiscounts(baseCost);
    }
}

public class CarRentalCostStrategy : ICostCalculationStrategy
{
    public decimal CalculateCost(TravelBooking booking)
    {
        decimal baseCost = booking.Distance * 0.12m;
        decimal dailyRate = 50m;
        int estimatedDays = (int)Math.Ceiling(booking.Distance / 500m);

        baseCost += dailyRate * estimatedDays;

        if (booking.ServiceClass.ToLower() == "premium")
        {
            baseCost *= 1.8m;
        }
        else if (booking.ServiceClass.ToLower() == "suv")
        {
            baseCost *= 2.2m;
        }

        baseCost += booking.AdditionalServices.Count * 10;
        baseCost *= booking.PassengerCount;

        return booking.ApplyDiscounts(baseCost);
    }
}

public class TravelBookingContext
{
    private ICostCalculationStrategy _strategy;
    private TravelBooking _booking;

    public TravelBookingContext()
    {
        _booking = new TravelBooking();
    }

    public void SetStrategy(ICostCalculationStrategy strategy)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public void SetBookingDetails(string transportType, decimal distance, int passengers, string serviceClass)
    {
        if (distance <= 0)
            throw new ArgumentException("Расстояние должно быть положительным числом.");
        
        if (passengers <= 0)
            throw new ArgumentException("Количество пассажиров должно быть положительным числом.");

        _booking.TransportationType = transportType;
        _booking.Distance = distance;
        _booking.PassengerCount = passengers;
        _booking.ServiceClass = serviceClass;
    }

    public void AddLuggage(decimal weight)
    {
        if (weight < 0)
            throw new ArgumentException("Вес багажа не может быть отрицательным.");

        _booking.HasLuggage = true;
        _booking.LuggageWeight = weight;
    }

    public void SetRoundTrip(bool isRoundTrip)
    {
        _booking.IsRoundTrip = isRoundTrip;
    }

    public void AddAdditionalService(string service)
    {
        _booking.AdditionalServices.Add(service);
    }

    public void AddDiscount(string type, decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Скидка должна быть в диапазоне от 0 до 100%.");

        _booking.AddDiscount(type, percent);
    }

    public decimal CalculateCost()
    {
        if (_strategy == null)
            throw new InvalidOperationException("Стратегия расчета не установлена.");

        return _strategy.CalculateCost(_booking);
    }

    public void DisplayBookingDetails()
    {
        Console.WriteLine("\n=== Детали бронирования ===");
        Console.WriteLine($"Тип транспорта: {_booking.TransportationType}");
        Console.WriteLine($"Расстояние: {_booking.Distance} км");
        Console.WriteLine($"Пассажиров: {_booking.PassengerCount}");
        Console.WriteLine($"Класс обслуживания: {_booking.ServiceClass}");
        Console.WriteLine($"Багаж: {( _booking.HasLuggage ? $"Да ({_booking.LuggageWeight}кг)" : "Нет")}");
        Console.WriteLine($"Тип поездки: {(_booking.IsRoundTrip ? "Туда-обратно" : "В одну сторону")}");
        
        if (_booking.AdditionalServices.Count > 0)
        {
            Console.WriteLine("Дополнительные услуги: " + string.Join(", ", _booking.AdditionalServices));
        }
        
        if (_booking.Discounts.Count > 0)
        {
            Console.WriteLine("Скидки:");
            foreach (var discount in _booking.Discounts)
            {
                Console.WriteLine($"  - {discount.Key}: {discount.Value}%");
            }
        }
    }
}

class TravelBookingProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система бронирования путешествий ===");
        
        TravelBookingContext context = new TravelBookingContext();

        try
        {
            while (true)
            {
                Console.WriteLine("\nВыберите тип транспорта:");
                Console.WriteLine("1 - Самолет");
                Console.WriteLine("2 - Поезд");
                Console.WriteLine("3 - Автобус");
                Console.WriteLine("4 - Аренда автомобиля");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                if (choice == "0") break;

                ICostCalculationStrategy strategy = choice switch
                {
                    "1" => new AirplaneCostStrategy(),
                    "2" => new TrainCostStrategy(),
                    "3" => new BusCostStrategy(),
                    "4" => new CarRentalCostStrategy(),
                    _ => throw new ArgumentException("Неверный выбор транспорта")
                };

                context.SetStrategy(strategy);

                Console.Write("Введите расстояние (км): ");
                decimal distance = decimal.Parse(Console.ReadLine());

                Console.Write("Введите количество пассажиров: ");
                int passengers = int.Parse(Console.ReadLine());

                Console.Write("Введите класс обслуживания: ");
                string serviceClass = Console.ReadLine();

                context.SetBookingDetails(
                    strategy.GetType().Name.Replace("CostStrategy", ""),
                    distance, passengers, serviceClass
                );

                Console.Write("Добавить багаж? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                {
                    Console.Write("Введите вес багажа (кг): ");
                    decimal weight = decimal.Parse(Console.ReadLine());
                    context.AddLuggage(weight);
                }

                Console.Write("Поездка туда-обратно? (y/n): ");
                context.SetRoundTrip(Console.ReadLine().ToLower() == "y");

                Console.Write("Добавить питание? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                    context.AddAdditionalService("Питание");

                Console.Write("Добавить страховку? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                    context.AddAdditionalService("Страховка");

                Console.Write("Есть ли детская скидка? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                    context.AddDiscount("Детская", 15);

                Console.Write("Есть ли пенсионная скидка? (y/n): ");
                if (Console.ReadLine().ToLower() == "y")
                    context.AddDiscount("Пенсионная", 10);

                context.DisplayBookingDetails();
                decimal cost = context.CalculateCost();
                Console.WriteLine($"\n💰 Итоговая стоимость: {cost:C}");

                Console.WriteLine("\nНажмите любую клавишу для нового бронирования...");
                Console.ReadKey();
                Console.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
