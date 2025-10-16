using System;
using System.Collections.Generic;

public interface IObserver
{
    void Update(float temperature);
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}

public class WeatherStation : ISubject
{
    private List<IObserver> observers;
    private float temperature;

    public WeatherStation()
    {
        observers = new List<IObserver>();
    }

    public void RegisterObserver(IObserver observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer), "Наблюдатель не может быть null");
        }

        if (!observers.Contains(observer))
        {
            observers.Add(observer);
            Console.WriteLine($"Наблюдатель {observer.GetType().Name} зарегистрирован.");
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observer == null)
        {
            throw new ArgumentNullException(nameof(observer), "Наблюдатель не может быть null");
        }

        if (observers.Contains(observer))
        {
            observers.Remove(observer);
            Console.WriteLine($"Наблюдатель {observer.GetType().Name} удален.");
        }
        else
        {
            Console.WriteLine("Наблюдатель не найден в списке подписчиков.");
        }
    }

    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.Update(temperature);
        }
    }

    public void SetTemperature(float newTemperature)
    {
        if (newTemperature < -100 || newTemperature > 100)
        {
            throw new ArgumentException("Температура должна быть в диапазоне от -100 до +100 градусов.");
        }

        Console.WriteLine($"\nМетеостанция: изменение температуры с {temperature}°C на {newTemperature}°C");
        temperature = newTemperature;
        NotifyObservers();
    }

    public float GetCurrentTemperature()
    {
        return temperature;
    }
}

public class WeatherDisplay : IObserver
{
    private string _name;

    public WeatherDisplay(string name)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void Update(float temperature)
    {
        Console.WriteLine($"{_name}: получено обновление температуры - {temperature}°C");
        
        if (temperature > 30)
        {
            Console.WriteLine($"   {_name}: Внимание! Высокая температура!");
        }
        else if (temperature < 0)
        {
            Console.WriteLine($"   {_name}: Предупреждение о заморозках!");
        }
    }
}

public class EmailNotifier : IObserver
{
    private string _email;

    public EmailNotifier(string email)
    {
        _email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void Update(float temperature)
    {
        Console.WriteLine($"Email отправлен на {_email}: Температура изменилась до {temperature}°C");
    }
}

public class SoundAlarm : IObserver
{
    public void Update(float temperature)
    {
        if (temperature > 35 || temperature < -20)
        {
            Console.WriteLine($"🚨 ЗВУКОВОЕ ОПОВЕЩЕНИЕ: Критическая температура {temperature}°C!");
        }
    }
}

class WeatherMonitoringProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("Система мониторинга погодных условий");
        Console.WriteLine("=====================================");

        WeatherStation weatherStation = new WeatherStation();

        try
        {
            WeatherDisplay mobileApp = new WeatherDisplay("Мобильное приложение");
            WeatherDisplay digitalBillboard = new WeatherDisplay("Электронное табло");
            EmailNotifier emailNotifier = new EmailNotifier("admin@weather.com");
            SoundAlarm soundAlarm = new SoundAlarm();

            weatherStation.RegisterObserver(mobileApp);
            weatherStation.RegisterObserver(digitalBillboard);

            Console.WriteLine("\n--- Тестирование изменений температуры ---");
            weatherStation.SetTemperature(25.0f);
            weatherStation.SetTemperature(30.0f);

            Console.WriteLine("\n--- Добавление новых наблюдателей ---");
            weatherStation.RegisterObserver(emailNotifier);
            weatherStation.RegisterObserver(soundAlarm);

            weatherStation.SetTemperature(36.0f);

            Console.WriteLine("\n--- Удаление одного из наблюдателей ---");
            weatherStation.RemoveObserver(digitalBillboard);

            weatherStation.SetTemperature(28.0f);

            Console.WriteLine("\n--- Тестирование экстремальных температур ---");
            weatherStation.SetTemperature(-25.0f);

            Console.WriteLine("\n--- Попытка удалить несуществующего наблюдателя ---");
            WeatherDisplay nonExistent = new WeatherDisplay("Несуществующий");
            weatherStation.RemoveObserver(nonExistent);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
