using System;
using System.Collections.Generic;
using System.Linq;

public abstract class GarageComponent
{
    protected string _name;
    protected string _owner;

    public GarageComponent(string name, string owner)
    {
        _name = name;
        _owner = owner;
    }

    public abstract void Display(int depth);
    public abstract decimal GetTotalValue();
    public abstract List<string> GetCarList();

    public virtual void Add(GarageComponent component)
    {
        throw new NotImplementedException();
    }

    public virtual void Remove(GarageComponent component)
    {
        throw new NotImplementedException();
    }

    public virtual GarageComponent GetChild(int index)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsCar()
    {
        return false;
    }
}

public class Car : GarageComponent
{
    private decimal _price;
    private string _model;
    private List<string> _features;

    public Car(string name, string owner, decimal price, string model) : base(name, owner)
    {
        _price = price;
        _model = model;
        _features = new List<string>();
    }

    public void AddFeature(string feature)
    {
        _features.Add(feature);
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string(' ', depth) + $"- {_name} ({_model})");
        Console.WriteLine(new string(' ', depth + 2) + $"Владелец: {_owner}");
        Console.WriteLine(new string(' ', depth + 2) + $"Стоимость: {_price:C}");
        if (_features.Count > 0)
        {
            Console.WriteLine(new string(' ', depth + 2) + "Особенности: " + string.Join(", ", _features));
        }
    }

    public override decimal GetTotalValue()
    {
        return _price;
    }

    public override List<string> GetCarList()
    {
        return new List<string> { $"{_name} ({_model}) - {_owner}" };
    }

    public override bool IsCar()
    {
        return true;
    }
}

public class Garage : GarageComponent
{
    private List<GarageComponent> _children = new List<GarageComponent>();
    private string _location;

    public Garage(string name, string owner, string location) : base(name, owner)
    {
        _location = location;
    }

    public override void Add(GarageComponent component)
    {
        _children.Add(component);
    }

    public override void Remove(GarageComponent component)
    {
        _children.Remove(component);
    }

    public override GarageComponent GetChild(int index)
    {
        return _children[index];
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string(' ', depth) + $"+ {_name}");
        Console.WriteLine(new string(' ', depth + 2) + $"Местоположение: {_location}");
        Console.WriteLine(new string(' ', depth + 2) + $"Владелец гаража: {_owner}");
        Console.WriteLine(new string(' ', depth + 2) + $"Количество автомобилей: {_children.Count}");
        Console.WriteLine(new string(' ', depth + 2) + $"Общая стоимость: {GetTotalValue():C}");

        foreach (var component in _children)
        {
            component.Display(depth + 4);
        }
    }

    public override decimal GetTotalValue()
    {
        return _children.Sum(child => child.GetTotalValue());
    }

    public override List<string> GetCarList()
    {
        var carList = new List<string>();
        foreach (var component in _children)
        {
            carList.AddRange(component.GetCarList());
        }
        return carList;
    }

    public int GetCarCount()
    {
        return _children.Count;
    }

    public List<GarageComponent> GetCarsByOwner(string owner)
    {
        return _children.Where(c => c._owner == owner).ToList();
    }
}

public class GarageGroup : GarageComponent
{
    private List<GarageComponent> _children = new List<GarageComponent>();
    private string _description;

    public GarageGroup(string name, string owner, string description) : base(name, owner)
    {
        _description = description;
    }

    public override void Add(GarageComponent component)
    {
        _children.Add(component);
    }

    public override void Remove(GarageComponent component)
    {
        _children.Remove(component);
    }

    public override GarageComponent GetChild(int index)
    {
        return _children[index];
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string(' ', depth) + $"# {_name}");
        Console.WriteLine(new string(' ', depth + 2) + $"Описание: {_description}");
        Console.WriteLine(new string(' ', depth + 2) + $"Владелец группы: {_owner}");
        Console.WriteLine(new string(' ', depth + 2) + $"Количество гаражей: {_children.Count}");
        Console.WriteLine(new string(' ', depth + 2) + $"Общая стоимость всех автомобилей: {GetTotalValue():C}");

        foreach (var component in _children)
        {
            component.Display(depth + 4);
        }
    }

    public override decimal GetTotalValue()
    {
        return _children.Sum(child => child.GetTotalValue());
    }

    public override List<string> GetCarList()
    {
        var carList = new List<string>();
        foreach (var component in _children)
        {
            carList.AddRange(component.GetCarList());
        }
        return carList;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА АВТОМОБИЛЬНЫХ ГАРАЖЕЙ С ПАТТЕРНОМ КОМПОНОВЩИК ===\n");

        // Создаем автомобили для Молдахулова Эмира
        Car emirCar1 = new Car("Mercedes C43 AMG", "Молдахулов Эмир", 85000, "2023");
        emirCar1.AddFeature("Полный привод");
        emirCar1.AddFeature("Кожаный салон");
        emirCar1.AddFeature("Панорамная крыша");

        Car emirCar2 = new Car("БМВ е34", "Молдахулов Эмир", 15000, "1995");
        emirCar2.AddFeature("Ручная КПП");
        emirCar2.AddFeature("Спортивная подвеска");

        // Создаем автомобили для Кожабек Али
        Car aliCar1 = new Car("Мерс Кабан", "Кожабек Али", 120000, "2024");
        aliCar1.AddFeature("V8 двигатель");
        aliCar1.AddFeature("Премиум аудиосистема");

        Car aliCar2 = new Car("БМВ е38 как в бумере", "Кожабек Али", 8000, "1998");
        aliCar2.AddFeature("Длинная база");
        aliCar2.AddFeature("Деревянная отделка");

        // Создаем автомобили для Байжан Амира
        Car amirCar = new Car("Mercedes C43 AMG", "Байжан Амир", 82000, "2023");
        amirCar.AddFeature("Ночное видение");
        amirCar.AddFeature("Массаж сидений");

        // Создаем автомобили для Изатова Диаса
        Car diasCar = new Car("БМВ е34", "Изатов Диас", 12000, "1994");
        diasCar.AddFeature("Тюнингованный выхлоп");

        // Создаем автомобили для Казимира Казимировича
        Car kazimirCar = new Car("БМВ е38 как в бумере", "Казимир Казимирович", 10000, "1999");
        kazimirCar.AddFeature("Люк");
        kazimirCar.AddFeature("Климат-контроль");

        // Создаем автомобили для Дмитрия Снега
        Car dmitrySnowCar = new Car("Mercedes C43 AMG", "Дмитрий Снег", 88000, "2024");
        dmitrySnowCar.AddFeature("Зимний пакет");
        dmitrySnowCar.AddFeature("Подогрев всех сидений");

        // Создаем автомобили для Дмитрия Довгешко
        Car dmitryDovgeshkoCar = new Car("Мерс Кабан", "Дмитрий Довгешко", 125000, "2024");
        dmitryDovgeshkoCar.AddFeature("AMG пакет");
        dmitryDovgeshkoCar.AddFeature("Углеродное волокно");

        // Создаем гаражи
        Garage emirGarage = new Garage("Гараж Эмира", "Молдахулов Эмир", "Талдыкорган, мкрн 3, дом 15");
        emirGarage.Add(emirCar1);
        emirGarage.Add(emirCar2);

        Garage aliGarage = new Garage("Гараж Али", "Кожабек Али", "Талдыкорган, мкрн 5, дом 22");
        aliGarage.Add(aliCar1);
        aliGarage.Add(aliCar2);

        Garage amirGarage = new Garage("Гараж Амира", "Байжан Амир", "Талдыкорган, мкрн 1, дом 8");
        amirGarage.Add(amirCar);

        Garage premiumGarage = new Garage("Премиум гараж", "Дмитрий Довгешко", "Талдыкорган, мкрн 7, дом 33");
        premiumGarage.Add(dmitryDovgeshkoCar);
        premiumGarage.Add(dmitrySnowCar);
        premiumGarage.Add(diasCar);
        premiumGarage.Add(kazimirCar);

        // Создаем группы гаражей
        GarageGroup friendsGroup = new GarageGroup("Группа друзей", "Молдахулов Эмир", "Автомобили нашей компании");
        friendsGroup.Add(emirGarage);
        friendsGroup.Add(aliGarage);
        friendsGroup.Add(amirGarage);

        GarageGroup premiumGroup = new GarageGroup("Премиум группа", "Дмитрий Довгешко", "Элитные автомобили");

        // Создаем общую структуру
        GarageGroup totalCollection = new GarageGroup("Вся коллекция", "Администратор", "Все автомобили системы");
        totalCollection.Add(friendsGroup);
        totalCollection.Add(premiumGarage);

        // Отображаем структуру
        Console.WriteLine("=== ПОЛНАЯ СТРУКТУРА АВТОМОБИЛЬНОЙ КОЛЛЕКЦИИ ===");
        totalCollection.Display(1);

        Console.WriteLine("\n" + new string('=', 60));
        
        // Аналитика
        Console.WriteLine("=== АНАЛИТИКА КОЛЛЕКЦИИ ===");
        Console.WriteLine($"Общее количество автомобилей: {totalCollection.GetCarList().Count}");
        Console.WriteLine($"Общая стоимость коллекции: {totalCollection.GetTotalValue():C}");
        
        Console.WriteLine("\nСписок всех автомобилей:");
        foreach (var car in totalCollection.GetCarList())
        {
            Console.WriteLine($"  - {car}");
        }

        Console.WriteLine("\n" + new string('=', 60));
        
        // Поиск автомобилей по владельцу
        Console.WriteLine("=== АВТОМОБИЛИ МОЛДАХУЛОВА ЭМИРА ===");
        var emirsCars = premiumGarage.GetCarsByOwner("Молдахулов Эмир");
        foreach (var car in emirsCars)
        {
            car.Display(2);
        }

        Console.WriteLine("\n" + new string('=', 60));
        
        // Отдельный гараж
        Console.WriteLine("=== ГАРАЖ КОЖАБЕК АЛИ ===");
        aliGarage.Display(1);

        Console.WriteLine("\nВсё работает чётко.");
    }
}
