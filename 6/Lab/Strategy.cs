using System;

public interface IShippingStrategy
{
    decimal CalculateShippingCost(decimal weight, decimal distance);
}

public class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 0.5m + distance * 0.1m;
    }
}

public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return (weight * 0.75m + distance * 0.2m) + 10;
    }
}

public class InternationalShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        return weight * 1.0m + distance * 0.5m + 15;
    }
}

public class OvernightShippingStrategy : IShippingStrategy
{
    public decimal CalculateShippingCost(decimal weight, decimal distance)
    {
        decimal baseCost = weight * 0.9m + distance * 0.3m;
        return baseCost + 25;
    }
}

public class DeliveryContext
{
    private IShippingStrategy _shippingStrategy;

    public void SetShippingStrategy(IShippingStrategy strategy)
    {
        _shippingStrategy = strategy;
    }

    public decimal CalculateCost(decimal weight, decimal distance)
    {
        if (_shippingStrategy == null)
        {
            throw new InvalidOperationException("Стратегия доставки не установлена.");
        }

        ValidateInput(weight, distance);

        return _shippingStrategy.CalculateShippingCost(weight, distance);
    }

    private void ValidateInput(decimal weight, decimal distance)
    {
        if (weight <= 0)
        {
            throw new ArgumentException("Вес должен быть положительным числом.");
        }

        if (distance <= 0)
        {
            throw new ArgumentException("Расстояние должно быть положительным числом.");
        }

        if (weight > 1000)
        {
            throw new ArgumentException("Вес не может превышать 1000 кг.");
        }

        if (distance > 50000)
        {
            throw new ArgumentException("Расстояние не может превышать 50000 км.");
        }
    }
}

class ShippingProgram
{
    static void Main(string[] args)
    {
        DeliveryContext deliveryContext = new DeliveryContext();

        Console.WriteLine("Система расчета стоимости доставки");
        Console.WriteLine("====================================");

        try
        {
            Console.WriteLine("Выберите тип доставки:");
            Console.WriteLine("1 - Стандартная доставка");
            Console.WriteLine("2 - Экспресс-доставка");
            Console.WriteLine("3 - Международная доставка");
            Console.WriteLine("4 - Ночная доставка");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    deliveryContext.SetShippingStrategy(new StandardShippingStrategy());
                    break;
                case "2":
                    deliveryContext.SetShippingStrategy(new ExpressShippingStrategy());
                    break;
                case "3":
                    deliveryContext.SetShippingStrategy(new InternationalShippingStrategy());
                    break;
                case "4":
                    deliveryContext.SetShippingStrategy(new OvernightShippingStrategy());
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Используется стандартная доставка.");
                    deliveryContext.SetShippingStrategy(new StandardShippingStrategy());
                    break;
            }

            Console.Write("Введите вес посылки (кг): ");
            decimal weight = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Введите расстояние доставки (км): ");
            decimal distance = Convert.ToDecimal(Console.ReadLine());

            decimal cost = deliveryContext.CalculateCost(weight, distance);
            Console.WriteLine($"Стоимость доставки: {cost:C}");

        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Введены некорректные данные. Используйте числа.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
