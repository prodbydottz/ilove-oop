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
        Console.WriteLine("Завариваем чайные листья");
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

public class CustomCoffee : Beverage
{
    private string _milkType;
    private string _sweetener;

    protected override string GetBeverageName()
    {
        return "Кастомный кофе";
    }

    protected override void Brew()
    {
        Console.WriteLine("Завариваем кофе выбранным способом");
    }

    protected override void AddCondiments()
    {
        if (!string.IsNullOrEmpty(_milkType))
        {
            Console.WriteLine($"Добавляем {_milkType}");
        }
        
        if (!string.IsNullOrEmpty(_sweetener))
        {
            Console.WriteLine($"Добавляем {_sweetener}");
        }
    }

    protected override bool CustomerWantsCondiments()
    {
        Console.WriteLine("Выберите тип молока:");
        Console.WriteLine("1 - Обычное молоко");
        Console.WriteLine("2 - Миндальное молоко");
        Console.WriteLine("3 - Овсяное молоко");
        Console.WriteLine("4 - Без молока");
        Console.Write("Ваш выбор: ");
        
        string milkChoice = Console.ReadLine();
        _milkType = milkChoice switch
        {
            "1" => "обычное молоко",
            "2" => "миндальное молоко",
            "3" => "овсяное молоко",
            _ => null
        };

        Console.WriteLine("Выберите подсластитель:");
        Console.WriteLine("1 - Сахар");
        Console.WriteLine("2 - Мед");
        Console.WriteLine("3 - Стевия");
        Console.WriteLine("4 - Без подсластителя");
        Console.Write("Ваш выбор: ");
        
        string sweetenerChoice = Console.ReadLine();
        _sweetener = sweetenerChoice switch
        {
            "1" => "сахар",
            "2" => "мед",
            "3" => "стевия",
            _ => null
        };

        return _milkType != null || _sweetener != null;
    }

    protected override bool CustomerWantsExtraSteps()
    {
        Console.Write("Добавить корицу? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "n";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override void PerformExtraSteps()
    {
        Console.WriteLine("Посыпаем корицей");
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
            ["5"] = new CustomCoffee()
        };

        bool continueMaking = true;

        while (continueMaking)
        {
            Console.WriteLine("Выберите напиток:");
            Console.WriteLine("1 - Чай");
            Console.WriteLine("2 - Кофе");
            Console.WriteLine("3 - Горячий шоколад");
            Console.WriteLine("4 - Эспрессо");
            Console.WriteLine("5 - Кастомный кофе");
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
