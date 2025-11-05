using System;
using System.Collections.Generic;

public class AudioSystem
{
    private string _owner;

    public AudioSystem(string owner)
    {
        _owner = owner;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{_owner} включает аудиосистему.");
    }

    public void SetVolume(int level)
    {
        Console.WriteLine($"{_owner} устанавливает громкость на {level}.");
    }

    public void SetSurroundSound()
    {
        Console.WriteLine("Активирован объемный звук - как в салоне Кабана");
    }

    public void TurnOff()
    {
        Console.WriteLine($"{_owner} выключает аудиосистему");
    }
}

public class VideoProjector
{
    private string _owner;

    public VideoProjector(string owner)
    {
        _owner = owner;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{_owner} включает видеопроектор");
    }

    public void SetResolution(string resolution)
    {
        Console.WriteLine($"Установлено разрешение {resolution}");
    }

    public void SetAspectRatio(string ratio)
    {
        Console.WriteLine($"Установлено соотношение сторон {ratio}");
    }

    public void TurnOff()
    {
        Console.WriteLine($"{_owner} выключает видеопроектор");
    }
}

public class LightingSystem
{
    private string _location;

    public LightingSystem(string location)
    {
        _location = location;
    }

    public void TurnOn()
    {
        Console.WriteLine($"Включается освещение в {_location}");
    }

    public void SetBrightness(int level)
    {
        Console.WriteLine($"Яркость освещения установлена на {level}%");
    }

    public void SetColor(string color)
    {
        Console.WriteLine($"Цвет освещения изменен на {color}");
    }

    public void TurnOff()
    {
        Console.WriteLine($"Освещение в {_location} выключено");
    }
}

public class SnackSystem
{
    private List<string> _availableSnacks;

    public SnackSystem()
    {
        _availableSnacks = new List<string>
        {
            "Бургер из Баханди",
            "Фри из Баханди",
            "Пицца из Додо",
            "Кола в железной банке",
            "Чизбургер",
            "Наггетсы"
        };
    }

    public void PrepareSnacks(List<string> snacks)
    {
        Console.WriteLine("Система закусок готовит:");
        foreach (var snack in snacks)
        {
            if (_availableSnacks.Contains(snack))
            {
                Console.WriteLine($"  - {snack}");
            }
            else
            {
                Console.WriteLine($"  - {snack} (нет в наличии)");
            }
        }
    }

    public void ServeSnacks()
    {
        Console.WriteLine("Закуски поданы: бургеры, фри и кола в железной банке готовы к употреблению!");
    }
}

public class ClimateControl
{
    private string _room;

    public ClimateControl(string room)
    {
        _room = room;
    }

    public void SetTemperature(int temperature)
    {
        Console.WriteLine($"Температура в {_room} установлена на {temperature}°C");
    }

    public void SetHumidity(int humidity)
    {
        Console.WriteLine($"Влажность установлена на {humidity}%");
    }

    public void TurnOnAirConditioning()
    {
        Console.WriteLine("Кондиционер включен.");
    }
}

public class HomeTheaterFacade
{
    private AudioSystem _audioSystem;
    private VideoProjector _videoProjector;
    private LightingSystem _lightingSystem;
    private SnackSystem _snackSystem;
    private ClimateControl _climateControl;
    private string _mainUser;

    public HomeTheaterFacade(string mainUser, string location)
    {
        _mainUser = mainUser;
        _audioSystem = new AudioSystem(mainUser);
        _videoProjector = new VideoProjector(mainUser);
        _lightingSystem = new LightingSystem(location);
        _snackSystem = new SnackSystem();
        _climateControl = new ClimateControl(location);
    }

    public void StartMovie(string movieTitle, List<string> snacks)
    {
        Console.WriteLine($"=== {_mainUser} начинает просмотр фильма '{movieTitle}' ===\n");
        
        _climateControl.SetTemperature(22);
        _climateControl.SetHumidity(50);
        _climateControl.TurnOnAirConditioning();
        
        _lightingSystem.TurnOn();
        _lightingSystem.SetBrightness(10);
        _lightingSystem.SetColor("синий");
        
        _audioSystem.TurnOn();
        _audioSystem.SetVolume(15);
        _audioSystem.SetSurroundSound();
        
        _videoProjector.TurnOn();
        _videoProjector.SetResolution("4K Ultra HD");
        _videoProjector.SetAspectRatio("16:9");
        
        _snackSystem.PrepareSnacks(snacks);
        _snackSystem.ServeSnacks();
        
        Console.WriteLine($"\nФильм '{movieTitle}' начинается! Приятного просмотра!");
        Console.WriteLine("Звук как в БМВ е38 из бумера, картинка четкая. !\n");
    }

    public void StartGamingSession(string gameTitle)
    {
        Console.WriteLine($"=== {_mainUser} начинает игровую сессию '{gameTitle}' ===\n");
        
        _climateControl.SetTemperature(20);
        _climateControl.TurnOnAirConditioning();
        
        _lightingSystem.TurnOn();
        _lightingSystem.SetBrightness(30);
        _lightingSystem.SetColor("красный");
        
        _audioSystem.TurnOn();
        _audioSystem.SetVolume(20);
        _audioSystem.SetSurroundSound();
        
        _videoProjector.TurnOn();
        _videoProjector.SetResolution("1080p");
        _videoProjector.SetAspectRatio("16:9");
        
        Console.WriteLine($"Игровая сессия '{gameTitle}' началась!");
        Console.WriteLine("Готов к победам.\n");
    }

    public void StartMusicListening(string playlist)
    {
        Console.WriteLine($"=== {_mainUser} включает музыку: '{playlist}' ===\n");
        
        _lightingSystem.TurnOn();
        _lightingSystem.SetBrightness(40);
        _lightingSystem.SetColor("разноцветный");
        
        _audioSystem.TurnOn();
        _audioSystem.SetVolume(12);
        _audioSystem.SetSurroundSound();
        
        Console.WriteLine($"Воспроизводится плейлист: {playlist}");
        Console.WriteLine("Звук мощный как выхлоп у Мерс Кабана!\n");
    }

    public void EndEntertainment()
    {
        Console.WriteLine($"=== {_mainUser} завершает развлечение ===\n");
        
        _videoProjector.TurnOff();
        _audioSystem.TurnOff();
        _lightingSystem.TurnOff();
        
        Console.WriteLine("Все системы выключены. До следующего раза!");
        Console.WriteLine("Остатки еды: пустые банки из-под колы и упаковки от Баханди\n");
    }

    public void EmergencyShutdown()
    {
        Console.WriteLine("=== АВАРИЙНОЕ ВЫКЛЮЧЕНИЕ ===");
        
        _videoProjector.TurnOff();
        _audioSystem.TurnOff();
        _lightingSystem.TurnOff();
        
        Console.WriteLine("Все системы экстренно отключены!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА ДОМАШНЕГО КИНОТЕАТРА С ПАТТЕРНОМ ФАСАД ===\n");

        // Создаем фасад для разных пользователей
        HomeTheaterFacade emirTheater = new HomeTheaterFacade("Молдахулов Эмир", "Талдыкорган, мкрн 3, дом 15");
        HomeTheaterFacade aliTheater = new HomeTheaterFacade("Кожабек Али", "Талдыкорган, мкрн 5, дом 22");
        HomeTheaterFacade amirTheater = new HomeTheaterFacade("Байжан Амир", "Талдыкорган, мкрн 1, дом 8");

        // смотрим фильм
        var emirSnacks = new List<string> { "Бургер из Баханди", "Фри из Баханди", "Кола в железной банке" };
        emirTheater.StartMovie("Жмурки", emirSnacks);
        System.Threading.Thread.Sleep(2000);
        emirTheater.EndEntertainment();

        Console.WriteLine(new string('=', 50) + "\n");

        // играем в игры
        aliTheater.StartGamingSession("Need for Speed");
        System.Threading.Thread.Sleep(2000);
        aliTheater.EndEntertainment();

        Console.WriteLine(new string('=', 50) + "\n");

        // Байжан Амир слушает музыку
        amirTheater.StartMusicListening("Лучшие хиты 2024");
        System.Threading.Thread.Sleep(2000);
        amirTheater.EndEntertainment();

        Console.WriteLine(new string('=', 50) + "\n");

        // Дмитрий Довгешко, тест аварийного отключения
        HomeTheaterFacade dmitryTheater = new HomeTheaterFacade("Дмитрий Довгешко", "Черновцы, мкрн 4, дом 33");
        var dmitrySnacks = new List<string> { "Пицца из Додо", "Кола в железной банке" };
        dmitryTheater.StartMovie("Бумер 2", dmitrySnacks);
        System.Threading.Thread.Sleep(1000);
        dmitryTheater.EmergencyShutdown();

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("Демонстрация завершена.");
    }
}
