using System;
using System.Collections.Generic;
using System.Linq;

public interface ICommand
{
    void Execute();
    void Undo();
    string GetDescription();
}

public class Light
{
    private string _location;
    private bool _isOn;
    private int _brightness;

    public Light(string location)
    {
        _location = location;
        _isOn = false;
        _brightness = 100;
    }

    public void TurnOn()
    {
        _isOn = true;
        Console.WriteLine($"Свет в {_location} включен");
    }

    public void TurnOff()
    {
        _isOn = false;
        Console.WriteLine($"Свет в {_location} выключен");
    }

    public void SetBrightness(int level)
    {
        if (level < 0) level = 0;
        if (level > 100) level = 100;
        
        _brightness = level;
        Console.WriteLine($"Яркость света в {_location} установлена на {level}%");
    }

    public bool IsOn => _isOn;
    public int Brightness => _brightness;
    public string Location => _location;

    public override string ToString()
    {
        return $"Свет в {_location}: {(IsOn ? "ВКЛ" : "ВЫКЛ")}, яркость: {Brightness}%";
    }
}

public class Door
{
    private string _location;
    private bool _isOpen;
    private bool _isLocked;

    public Door(string location)
    {
        _location = location;
        _isOpen = false;
        _isLocked = true;
    }

    public void Open()
    {
        if (_isLocked)
        {
            Console.WriteLine($"Дверь в {_location} закрыта на замок! Сначала нужно открыть замок.");
            return;
        }
        
        _isOpen = true;
        Console.WriteLine($"Дверь в {_location} открыта");
    }

    public void Close()
    {
        _isOpen = false;
        Console.WriteLine($"Дверь в {_location} закрыта");
    }

    public void Lock()
    {
        _isLocked = true;
        Console.WriteLine($"Дверь в {_location} закрыта на замок");
    }

    public void Unlock()
    {
        _isLocked = false;
        Console.WriteLine($"Дверь в {_location} открыта");
    }

    public bool IsOpen => _isOpen;
    public bool IsLocked => _isLocked;
    public string Location => _location;

    public override string ToString()
    {
        return $"Дверь в {_location}: {(IsOpen ? "ОТКРЫТА" : "ЗАКРЫТА")}, {(IsLocked ? "ЗАМКНУТА" : "ОТКРЫТА")}";
    }
}

public class Thermostat
{
    private string _location;
    private double _temperature;
    private bool _isOn;

    public Thermostat(string location)
    {
        _location = location;
        _temperature = 22.0;
        _isOn = false;
    }

    public void TurnOn()
    {
        _isOn = true;
        Console.WriteLine($"Термостат в {_location} включен. Температура: {_temperature}°C");
    }

    public void TurnOff()
    {
        _isOn = false;
        Console.WriteLine($"Термостат в {_location} выключен");
    }

    public void SetTemperature(double temperature)
    {
        if (temperature < 10) temperature = 10;
        if (temperature > 30) temperature = 30;
        
        double oldTemp = _temperature;
        _temperature = temperature;
        
        Console.WriteLine($"Температура в {_location} изменена: {oldTemp}°C → {_temperature}°C");
    }

    public bool IsOn => _isOn;
    public double Temperature => _temperature;
    public string Location => _location;

    public override string ToString()
    {
        return $"Термостат в {_location}: {(IsOn ? "ВКЛ" : "ВЫКЛ")}, температура: {Temperature}°C";
    }
}

public class Television
{
    private string _location;
    private bool _isOn;
    private int _volume;
    private int _channel;

    public Television(string location)
    {
        _location = location;
        _isOn = false;
        _volume = 50;
        _channel = 1;
    }

    public void TurnOn()
    {
        _isOn = true;
        Console.WriteLine($"Телевизор в {_location} включен. Канал: {_channel}, громкость: {_volume}%");
    }

    public void TurnOff()
    {
        _isOn = false;
        Console.WriteLine($"Телевизор в {_location} выключен");
    }

    public void SetVolume(int volume)
    {
        if (volume < 0) volume = 0;
        if (volume > 100) volume = 100;
        
        _volume = volume;
        Console.WriteLine($"Громкость телевизора в {_location} установлена на {volume}%");
    }

    public void SetChannel(int channel)
    {
        if (channel < 1) channel = 1;
        if (channel > 999) channel = 999;
        
        _channel = channel;
        Console.WriteLine($"Канал телевизора в {_location} изменен на {channel}");
    }

    public bool IsOn => _isOn;
    public int Volume => _volume;
    public int Channel => _channel;
    public string Location => _location;

    public override string ToString()
    {
        return $"Телевизор в {_location}: {(IsOn ? "ВКЛ" : "ВЫКЛ")}, канал: {Channel}, громкость: {Volume}%";
    }
}

public class LightCommand : ICommand
{
    private Light _light;
    private bool _previousState;

    public LightCommand(Light light)
    {
        _light = light ?? throw new ArgumentNullException(nameof(light));
    }

    public void Execute()
    {
        _previousState = _light.IsOn;
        
        if (_light.IsOn)
            _light.TurnOff();
        else
            _light.TurnOn();
    }

    public void Undo()
    {
        if (_previousState)
            _light.TurnOn();
        else
            _light.TurnOff();
    }

    public string GetDescription()
    {
        return $"Переключение света в {_light.Location}";
    }
}

public class BrightnessCommand : ICommand
{
    private Light _light;
    private int _previousBrightness;
    private int _newBrightness;

    public BrightnessCommand(Light light, int brightness)
    {
        _light = light ?? throw new ArgumentNullException(nameof(light));
        _newBrightness = brightness;
    }

    public void Execute()
    {
        _previousBrightness = _light.Brightness;
        _light.SetBrightness(_newBrightness);
    }

    public void Undo()
    {
        _light.SetBrightness(_previousBrightness);
    }

    public string GetDescription()
    {
        return $"Изменение яркости света в {_light.Location} на {_newBrightness}%";
    }
}

public class DoorCommand : ICommand
{
    private Door _door;
    private bool _previousState;

    public DoorCommand(Door door)
    {
        _door = door ?? throw new ArgumentNullException(nameof(door));
    }

    public void Execute()
    {
        _previousState = _door.IsOpen;
        
        if (_door.IsOpen)
            _door.Close();
        else
            _door.Open();
    }

    public void Undo()
    {
        if (_previousState)
            _door.Open();
        else
            _door.Close();
    }

    public string GetDescription()
    {
        return $"Переключение двери в {_door.Location}";
    }
}

public class LockCommand : ICommand
{
    private Door _door;
    private bool _previousState;

    public LockCommand(Door door)
    {
        _door = door ?? throw new ArgumentNullException(nameof(door));
    }

    public void Execute()
    {
        _previousState = _door.IsLocked;
        
        if (_door.IsLocked)
            _door.Unlock();
        else
            _door.Lock();
    }

    public void Undo()
    {
        if (_previousState)
            _door.Lock();
        else
            _door.Unlock();
    }

    public string GetDescription()
    {
        return $"Переключение замка двери в {_door.Location}";
    }
}

public class ThermostatCommand : ICommand
{
    private Thermostat _thermostat;
    private bool _previousState;

    public ThermostatCommand(Thermostat thermostat)
    {
        _thermostat = thermostat ?? throw new ArgumentNullException(nameof(thermostat));
    }

    public void Execute()
    {
        _previousState = _thermostat.IsOn;
        
        if (_thermostat.IsOn)
            _thermostat.TurnOff();
        else
            _thermostat.TurnOn();
    }

    public void Undo()
    {
        if (_previousState)
            _thermostat.TurnOn();
        else
            _thermostat.TurnOff();
    }

    public string GetDescription()
    {
        return $"Переключение термостата в {_thermostat.Location}";
    }
}

public class TemperatureCommand : ICommand
{
    private Thermostat _thermostat;
    private double _previousTemperature;
    private double _newTemperature;

    public TemperatureCommand(Thermostat thermostat, double temperature)
    {
        _thermostat = thermostat ?? throw new ArgumentNullException(nameof(thermostat));
        _newTemperature = temperature;
    }

    public void Execute()
    {
        _previousTemperature = _thermostat.Temperature;
        _thermostat.SetTemperature(_newTemperature);
    }

    public void Undo()
    {
        _thermostat.SetTemperature(_previousTemperature);
    }

    public string GetDescription()
    {
        return $"Изменение температуры в {_thermostat.Location} на {_newTemperature}°C";
    }
}

public class TelevisionCommand : ICommand
{
    private Television _tv;
    private bool _previousState;

    public TelevisionCommand(Television tv)
    {
        _tv = tv ?? throw new ArgumentNullException(nameof(tv));
    }

    public void Execute()
    {
        _previousState = _tv.IsOn;
        
        if (_tv.IsOn)
            _tv.TurnOff();
        else
            _tv.TurnOn();
    }

    public void Undo()
    {
        if (_previousState)
            _tv.TurnOn();
        else
            _tv.TurnOff();
    }

    public string GetDescription()
    {
        return $"Переключение телевизора в {_tv.Location}";
    }
}

public class VolumeCommand : ICommand
{
    private Television _tv;
    private int _previousVolume;
    private int _newVolume;

    public VolumeCommand(Television tv, int volume)
    {
        _tv = tv ?? throw new ArgumentNullException(nameof(tv));
        _newVolume = volume;
    }

    public void Execute()
    {
        _previousVolume = _tv.Volume;
        _tv.SetVolume(_newVolume);
    }

    public void Undo()
    {
        _tv.SetVolume(_previousVolume);
    }

    public string GetDescription()
    {
        return $"Изменение громкости телевизора в {_tv.Location} на {_newVolume}%";
    }
}

public class SmartHomeInvoker
{
    private Stack<ICommand> _commandHistory;
    private Stack<ICommand> _redoHistory;
    private const int MAX_HISTORY_SIZE = 10;

    public SmartHomeInvoker()
    {
        _commandHistory = new Stack<ICommand>();
        _redoHistory = new Stack<ICommand>();
    }

    public void ExecuteCommand(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        try
        {
            command.Execute();
            _commandHistory.Push(command);
            _redoHistory.Clear();

            if (_commandHistory.Count > MAX_HISTORY_SIZE)
            {
                var oldCommands = new List<ICommand>(_commandHistory.Take(MAX_HISTORY_SIZE));
                _commandHistory = new Stack<ICommand>(oldCommands.Reverse<ICommand>());
            }

            Console.WriteLine($"Выполнено: {command.GetDescription()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выполнения команды: {ex.Message}");
        }
    }

    public void Undo()
    {
        if (_commandHistory.Count == 0)
        {
            Console.WriteLine("Нет команд для отмены");
            return;
        }

        var command = _commandHistory.Pop();
        command.Undo();
        _redoHistory.Push(command);
        
        Console.WriteLine($"Отменено: {command.GetDescription()}");
    }

    public void Redo()
    {
        if (_redoHistory.Count == 0)
        {
            Console.WriteLine("Нет команд для повтора");
            return;
        }

        var command = _redoHistory.Pop();
        command.Execute();
        _commandHistory.Push(command);
        
        Console.WriteLine($"Повторено: {command.GetDescription()}");
    }

    public void ShowHistory()
    {
        Console.WriteLine("\n=== История команд ===");
        if (_commandHistory.Count == 0)
        {
            Console.WriteLine("История пуста");
            return;
        }

        int index = 1;
        foreach (var command in _commandHistory.Reverse())
        {
            Console.WriteLine($"{index}. {command.GetDescription()}");
            index++;
        }
    }

    public void ClearHistory()
    {
        _commandHistory.Clear();
        _redoHistory.Clear();
        Console.WriteLine("🗑️ История команд очищена");
    }
}

class SmartHomeProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система управления умным домом ===\n");

        SmartHomeInvoker invoker = new SmartHomeInvoker();

        Light livingRoomLight = new Light("Гостиная");
        Light kitchenLight = new Light("Кухня");
        Door frontDoor = new Door("Парадная");
        Door garageDoor = new Door("Гараж");
        Thermostat livingRoomThermostat = new Thermostat("Гостиная");
        Television livingRoomTV = new Television("Гостиная");

        bool running = true;

        while (running)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Управление светом");
            Console.WriteLine("2 - Управление дверями");
            Console.WriteLine("3 - Управление температурой");
            Console.WriteLine("4 - Управление телевизором");
            Console.WriteLine("5 - История команд");
            Console.WriteLine("6 - Отменить последнюю команду");
            Console.WriteLine("7 - Повторить отмененную команду");
            Console.WriteLine("8 - Показать статус устройств");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        ManageLights(invoker, livingRoomLight, kitchenLight);
                        break;
                    case "2":
                        ManageDoors(invoker, frontDoor, garageDoor);
                        break;
                    case "3":
                        ManageThermostat(invoker, livingRoomThermostat);
                        break;
                    case "4":
                        ManageTelevision(invoker, livingRoomTV);
                        break;
                    case "5":
                        invoker.ShowHistory();
                        break;
                    case "6":
                        invoker.Undo();
                        break;
                    case "7":
                        invoker.Redo();
                        break;
                    case "8":
                        ShowDeviceStatus(livingRoomLight, kitchenLight, frontDoor, garageDoor, livingRoomThermostat, livingRoomTV);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        Console.WriteLine("Досвидания!");
    }

    static void ManageLights(SmartHomeInvoker invoker, Light livingRoomLight, Light kitchenLight)
    {
        Console.WriteLine("\n--- Управление светом ---");
        Console.WriteLine("1 - Переключить свет в гостиной");
        Console.WriteLine("2 - Переключить свет на кухне");
        Console.WriteLine("3 - Установить яркость в гостиной");
        Console.WriteLine("4 - Установить яркость на кухне");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                invoker.ExecuteCommand(new LightCommand(livingRoomLight));
                break;
            case "2":
                invoker.ExecuteCommand(new LightCommand(kitchenLight));
                break;
            case "3":
                Console.Write("Введите яркость (0-100): ");
                if (int.TryParse(Console.ReadLine(), out int brightness1))
                    invoker.ExecuteCommand(new BrightnessCommand(livingRoomLight, brightness1));
                break;
            case "4":
                Console.Write("Введите яркость (0-100): ");
                if (int.TryParse(Console.ReadLine(), out int brightness2))
                    invoker.ExecuteCommand(new BrightnessCommand(kitchenLight, brightness2));
                break;
        }
    }

    static void ManageDoors(SmartHomeInvoker invoker, Door frontDoor, Door garageDoor)
    {
        Console.WriteLine("\n--- Управление дверями ---");
        Console.WriteLine("1 - Переключить парадную дверь");
        Console.WriteLine("2 - Переключить дверь гаража");
        Console.WriteLine("3 - Переключить замок парадной двери");
        Console.WriteLine("4 - Переключить замок двери гаража");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                invoker.ExecuteCommand(new DoorCommand(frontDoor));
                break;
            case "2":
                invoker.ExecuteCommand(new DoorCommand(garageDoor));
                break;
            case "3":
                invoker.ExecuteCommand(new LockCommand(frontDoor));
                break;
            case "4":
                invoker.ExecuteCommand(new LockCommand(garageDoor));
                break;
        }
    }

    static void ManageThermostat(SmartHomeInvoker invoker, Thermostat thermostat)
    {
        Console.WriteLine("\n--- Управление температурой ---");
        Console.WriteLine("1 - Переключить термостат");
        Console.WriteLine("2 - Установить температуру");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                invoker.ExecuteCommand(new ThermostatCommand(thermostat));
                break;
            case "2":
                Console.Write("Введите температуру (10-30°C): ");
                if (double.TryParse(Console.ReadLine(), out double temp))
                    invoker.ExecuteCommand(new TemperatureCommand(thermostat, temp));
                break;
        }
    }

    static void ManageTelevision(SmartHomeInvoker invoker, Television tv)
    {
        Console.WriteLine("\n--- Управление телевизором ---");
        Console.WriteLine("1 - Переключить телевизор");
        Console.WriteLine("2 - Установить громкость");
        Console.Write("Ваш выбор: ");

        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                invoker.ExecuteCommand(new TelevisionCommand(tv));
                break;
            case "2":
                Console.Write("Введите громкость (0-100): ");
                if (int.TryParse(Console.ReadLine(), out int volume))
                    invoker.ExecuteCommand(new VolumeCommand(tv, volume));
                break;
        }
    }

    static void ShowDeviceStatus(params object[] devices)
    {
        Console.WriteLine("\n=== Статус устройств ===");
        foreach (var device in devices)
        {
            Console.WriteLine($"• {device}");
        }
    }
}
