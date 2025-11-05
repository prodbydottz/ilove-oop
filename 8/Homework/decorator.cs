using System;
using System.Collections.Generic;

// Базовый интерфейс напитка
public interface IBeverage
{
    string GetDescription();
    decimal GetCost();
}

// Базовые классы напитков
public class Espresso : IBeverage
{
    public string GetDescription()
    {
        return "Эспрессо";
    }

    public decimal GetCost()
    {
        return 120m;
    }
}

public class Tea : IBeverage
{
    public string GetDescription()
    {
        return "Чай";
    }

    public decimal GetCost()
    {
        return 80m;
    }
}

public class Latte : IBeverage
{
    public string GetDescription()
    {
        return "Латте";
    }

    public decimal GetCost()
    {
        return 150m;
    }
}

public class Mocha : IBeverage
{
    public string GetDescription()
    {
        return "Мокка";
    }

    public decimal GetCost()
    {
        return 160m;
    }
}

public class Americano : IBeverage
{
    public string GetDescription()
    {
        return "Американо";
    }

    public decimal GetCost()
    {
        return 100m;
    }
}

// Абстрактный класс декоратора
public abstract class BeverageDecorator : IBeverage
{
    protected IBeverage _beverage;

    protected BeverageDecorator(IBeverage beverage)
    {
        _beverage = beverage ?? throw new ArgumentNullException(nameof(beverage));
    }

    public virtual string GetDescription()
    {
        return _beverage.GetDescription();
    }

    public virtual decimal GetCost()
    {
        return _beverage.GetCost();
    }
}

// Конкретные декораторы-добавки
public class MilkDecorator : BeverageDecorator
{
    public MilkDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", молоко";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 30m;
    }
}

public class SugarDecorator : BeverageDecorator
{
    public SugarDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", сахар";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 10m;
    }
}

public class WhippedCreamDecorator : BeverageDecorator
{
    public WhippedCreamDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", взбитые сливки";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 40m;
    }
}

public class SyrupDecorator : BeverageDecorator
{
    private string _syrupType;

    public SyrupDecorator(IBeverage beverage, string syrupType) : base(beverage)
    {
        _syrupType = syrupType;
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + $", {_syrupType} сироп";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 25m;
    }
}

public class IceDecorator : BeverageDecorator
{
    public IceDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", лёд";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 15m;
    }
}

public class CinnamonDecorator : BeverageDecorator
{
    public CinnamonDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", корица";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 20m;
    }
}

// Класс для построения заказов
public class BeverageBuilder
{
    private IBeverage _beverage;

    public BeverageBuilder(IBeverage baseBeverage)
    {
        _beverage = baseBeverage;
    }

    public BeverageBuilder AddMilk()
    {
        _beverage = new MilkDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddSugar()
    {
        _beverage = new SugarDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddWhippedCream()
    {
        _beverage = new WhippedCreamDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddSyrup(string syrupType)
    {
        _beverage = new SyrupDecorator(_beverage, syrupType);
        return this;
    }

    public BeverageBuilder AddIce()
    {
        _beverage = new IceDecorator(_beverage);
        return this;
    }

    public BeverageBuilder AddCinnamon()
    {
        _beverage = new CinnamonDecorator(_beverage);
        return this;
    }

    public IBeverage Build()
    {
        return _beverage;
    }
}

// Клиентский код
class CafeOrderSystem
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ЗАКАЗАМИ В КАФЕ ===\n");

        Console.WriteLine("Заказ для Молдахулова Эмира:");
        IBeverage emirOrder = new BeverageBuilder(new Espresso())
            .AddMilk()
            .AddSugar()
            .AddCinnamon()
            .Build();
        DisplayOrder(emirOrder);

        Console.WriteLine("\nЗаказ для Кожабек Али:");
        IBeverage aliOrder = new BeverageBuilder(new Latte())
            .AddWhippedCream()
            .AddSyrup("ванильный")
            .Build();
        DisplayOrder(aliOrder);

        Console.WriteLine("\nЗаказ для Байжан Амира:");
        IBeverage amirOrder = new BeverageBuilder(new Tea())
            .AddSugar()
            .AddSugar() // Двойной сахар
            .AddLemon()
            .Build();
        DisplayOrder(amirOrder);

        Console.WriteLine("\nЗаказ для Изатова Диаса:");
        IBeverage diasOrder = new BeverageBuilder(new Mocha())
            .AddWhippedCream()
            .AddSyrup("шоколадный")
            .AddCinnamon()
            .Build();
        DisplayOrder(diasOrder);

        Console.WriteLine("\nЗаказ для Казимира Казимировича:");
        IBeverage kazimirOrder = new BeverageBuilder(new Americano())
            .AddIce()
            .AddSyrup("карамельный")
            .Build();
        DisplayOrder(kazimirOrder);

        Console.WriteLine("\nЗаказ для Дмитрия Снега:");
        IBeverage dmitrySnowOrder = new BeverageBuilder(new Espresso())
            .AddMilk()
            .AddWhippedCream()
            .AddSyrup("ореховый")
            .Build();
        DisplayOrder(dmitrySnowOrder);

        Console.WriteLine("\nЗаказ для Дмитрия Довгешко:");
        IBeverage dmitryDovgeshkoOrder = new BeverageBuilder(new Latte())
            .AddMilk()
            .AddSugar()
            .AddCinnamon()
            .AddWhippedCream()
            .Build();
        DisplayOrder(dmitryDovgeshkoOrder);

        // Тестирование различных комбинаций
        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ РАЗЛИЧНЫХ КОМБИНАЦИЙ ===");
        
        TestCombination("Эспрессо с молоком и сахаром", 
            new BeverageBuilder(new Espresso()).AddMilk().AddSugar().Build());
        
        TestCombination("Латте со взбитыми сливками и ванильным сиропом", 
            new BeverageBuilder(new Latte()).AddWhippedCream().AddSyrup("ванильный").Build());
        
        TestCombination("Чай с лимоном и сахаром", 
            new BeverageBuilder(new Tea()).AddLemon().AddSugar().Build());
        
        TestCombination("Мокка со всем", 
            new BeverageBuilder(new Mocha()).AddMilk().AddSugar().AddWhippedCream().AddSyrup("шоколадный").AddCinnamon().Build());
    }

    static void DisplayOrder(IBeverage beverage)
    {
        Console.WriteLine($"Напиток: {beverage.GetDescription()}");
        Console.WriteLine($"Стоимость: {beverage.GetCost():C}");
    }

    static void TestCombination(string testName, IBeverage beverage)
    {
        Console.WriteLine($"\n{testName}:");
        DisplayOrder(beverage);
    }
}

// Дополнительный декоратор для лимона
public class LemonDecorator : BeverageDecorator
{
    public LemonDecorator(IBeverage beverage) : base(beverage)
    {
    }

    public override string GetDescription()
    {
        return _beverage.GetDescription() + ", лимон";
    }

    public override decimal GetCost()
    {
        return _beverage.GetCost() + 15m;
    }
}

// Extension методы для BeverageBuilder
public static class BeverageBuilderExtensions
{
    public static BeverageBuilder AddLemon(this BeverageBuilder builder)
    {
        return new BeverageBuilder(new LemonDecorator(builder.Build()));
    }
}
