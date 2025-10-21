using System;
using System.Collections.Generic;

public abstract class Beverage
{
    public void PrepareRecipe()
    {
        Console.WriteLine($"\n=== Начинаем приготовление {GetBeverageName()} ===");
        
        BoilWater();
        Brew();
        PourInCup();
        
        if (CustomerWantsCondiments())
        {
            AddCondiments();
        }
        
        if (CustomerWantsExtraSteps())
        {
            PerformExtraSteps();
        }
        
        Serve();
        Console.WriteLine($"=== {GetBeverageName()} готов! ===\n");
    }

    protected virtual string GetBeverageName()
    {
        return "Напиток";
    }

    private void BoilWater()
    {
        Console.WriteLine("Кипятим воду");
    }

    protected abstract void Brew();

    private void PourInCup()
    {
        Console.WriteLine("Наливаем в чашку");
    }

    protected abstract void AddCondiments();

    protected virtual bool CustomerWantsCondiments()
    {
        return true;
    }

    protected virtual bool CustomerWantsExtraSteps()
    {
        return false;
    }

    protected virtual void PerformExtraSteps()
    {
    }

    protected virtual void Serve()
    {
        Console.WriteLine("Подаем напиток");
    }
}

public class Tea : Beverage
{
    protected override string GetBeverageName()
    {
        return "Чай";
    }

    protected override void Brew()
    {
        Console.WriteLine("🫖 Завариваем чайные листья");
    }

    protected override void AddCondiments()
    {
        Console.WriteLine("Добавляем лимон");
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Добавить лимон в чай? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "y";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override bool CustomerWantsExtraSteps()
    {
        Console.Write("Сделать чай с мятой? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "n";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override void PerformExtraSteps()
    {
        Console.WriteLine("Добавляем свежую мяту");
    }
}

public class Coffee : Beverage
{
    private bool _withMilk;

    protected override string GetBeverageName()
    {
        return "Кофе";
    }

    protected override void Brew()
    {
        Console.WriteLine("Завариваем кофе в фильтре");
    }

    protected override void AddCondiments()
    {
        if (_withMilk)
        {
            Console.WriteLine("Добавляем молоко и сахар");
        }
        else
        {
            Console.WriteLine("Добавляем сахар");
        }
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Добавить сахар в кофе? (y/n): ");
        string sugarInput = Console.ReadLine()?.ToLower() ?? "y";
        bool wantsSugar = sugarInput == "y" || sugarInput == "yes" || sugarInput == "д";

        if (wantsSugar)
        {
            Console.Write("Добавить молоко? (y/n): ");
            string milkInput = Console.ReadLine()?.ToLower() ?? "y";
            _withMilk = milkInput == "y" || milkInput == "yes" || milkInput == "д";
        }

        return wantsSugar;
    }

    protected override void Serve()
    {
        string servingStyle = _withMilk ? "кофе с молоком" : "черный кофе";
        Console.WriteLine($"Подаем {servingStyle}");
    }
}

public class HotChocolate : Beverage
{
    private bool _withMarshmallows;
    private bool _withCinnamon;

    protected override string GetBeverageName()
    {
        return "Горячий шоколад";
    }

    protected override void Brew()
    {
        Console.WriteLine("Растворяем шоколад в горячем молоке");
    }

    protected override void AddCondiments()
    {
        if (_withMarshmallows)
        {
            Console.WriteLine("Добавляем маршмеллоу");
        }
        
        if (_withCinnamon)
        {
            Console.WriteLine("Посыпаем корицей");
        }
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Добавить маршмеллоу в горячий шоколад? (y/n): ");
        string marshmallowInput = Console.ReadLine()?.ToLower() ?? "y";
        _withMarshmallows = marshmallowInput == "y" || marshmallowInput == "yes" || marshmallowInput == "д";

        Console.Write("Добавить корицу? (y/n): ");
        string cinnamonInput = Console.ReadLine()?.ToLower() ?? "y";
        _withCinnamon = cinnamonInput == "y" || cinnamonInput == "yes" || cinnamonInput == "д";

        return _withMarshmallows || _withCinnamon;
    }

    protected override void Serve()
    {
        Console.WriteLine("Подаем горячий шоколад с пенной шапкой");
    }
}

public class Espresso : Beverage
{
    protected override string GetBeverageName()
    {
        return "Эспрессо";
    }

    protected override void Brew()
    {
        Console.WriteLine("Готовим эспрессо в кофемашине под давлением");
    }

    protected override void AddCondiments()
    {
        Console.WriteLine("Подаем с долькой лимона и сахаром");
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Эспрессо принято пить без добавок. Все равно добавить? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "n";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override void Serve()
    {
        Console.WriteLine("Подаем эспрессо в маленькой чашке");
    }
}

public class Smoothie : Beverage
{
    private List<string> _fruits;

    public Smoothie()
    {
        _fruits = new List<string>();
    }

    protected override string GetBeverageName()
    {
        return "Фруктовый смузи";
    }

    protected override void BoilWater()
    {
    }

    protected override void Brew()
    {
        Console.WriteLine("Смешиваем фрукты в блендере:");
        foreach (var fruit in _fruits)
        {
            Console.WriteLine($"   - {fruit}");
        }
    }

    protected override void PourInCup()
    {
        Console.WriteLine "Переливаем смузи в высокий стакан";
    }

    protected override void AddCondiments()
    {
        Console.WriteLine("Добавляем мед и йогурт");
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.Write("Добавить мед и йогурт? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "y";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override bool CustomerWantsExtraSteps()
    {
        return true;
    }

    protected override void PerformExtraSteps()
    {
        SelectFruits();
        AddIce();
    }

    private void SelectFruits()
    {
        Console.WriteLine("Выберите фрукты для смузи (вводите названия, пустая строка - закончить):");
        
        while (true)
        {
            Console.Write("Фрукт: ");
            string fruit = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(fruit))
                break;
                
            _fruits.Add(fruit);
        }

        if (_fruits.Count == 0)
        {
            _fruits.Add("банан");
            _fruits.Add("клубника");
            Console.WriteLine("Используем стандартный набор: банан, клубника");
        }
    }

    private void AddIce()
    {
        Console.Write("Добавить лед? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "y";
        if (input == "y" || input == "yes" || input == "д")
        {
            Console.WriteLine("🧊 Добавляем кубики льда");
        }
    }

    protected override void Serve()
    {
        Console.WriteLine "Подаем смузи с соломинкой и украшением";
    }
}

class BeverageProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Кафе 'Шаблонные напитки' ===\n");

        Dictionary<string, Beverage> beverages = new Dictionary<string, Beverage>
        {
            ["1"] = new Tea(),
            ["2"] = new Coffee(),
            ["3"] = new HotChocolate(),
            ["4"] = new Espresso(),
            ["5"] = new Smoothie()
        };

        bool continueMaking = true;

        while (continueMaking)
        {
            Console.WriteLine("Выберите напиток:");
            Console.WriteLine("1 - Чай");
            Console.WriteLine("2 - Кофе");
            Console.WriteLine("3 - Горячий шоколад");
            Console.WriteLine("4 - Эспрессо");
            Console.WriteLine("5 - Фруктовый смузи");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            if (choice == "0")
            {
                continueMaking = false;
                continue;
            }

            if (beverages.ContainsKey(choice))
            {
                try
                {
                    beverages[choice].PrepareRecipe();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при приготовлении: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор напитка");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine("Спасибо за посещение нашего кафе!");
    }
}
