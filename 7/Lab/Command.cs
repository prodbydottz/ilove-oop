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

    public void On()
    {
        _isOn = true;
        Console.WriteLine($"Свет в {_location} включен");
    }

    public void Off()
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

    public void On()
    {
        _isOn = true;
        Console.WriteLine($"Телевизор в {_location} включен. Канал: {_channel}, громкость: {_volume}%");
    }

    public void Off()
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
}

public class AirConditioner
{
    private string _location;
    private bool _isOn;
    private int _temperature;
    private string _mode;

    public AirConditioner(string location)
    {
        _location = location;
        _isOn = false;
        _temperature = 22;
        _mode = "охлаждение";
    }

    public void On()
    {
        _isOn = true;
        Console.WriteLine($"Кондиционер в {_location} включен. Температура: {_temperature}°C, режим: {_mode}");
    }

    public void Off()
    {
        _isOn = false;
        Console.WriteLine($"Кондиционер в {_location} выключен");
    }

    public void SetTemperature(int temperature)
    {
        if (temperature < 16) temperature = 16;
        if (temperature > 30) temperature = 30;
        
        _temperature = temperature;
        Console.WriteLine($"Температура кондиционера в {_location} установлена на {temperature}°C");
    }

    public void SetMode(string mode)
    {
        _mode = mode;
        Console.WriteLine($"Режим кондиционера в {_location} изменен на: {mode}");
    }

    public void SetEnergySavingMode()
    {
        _mode = "энергосбережение";
        _temperature = 24;
        Console.WriteLine($"Кондиционер в {_location} переведен в режим энергосбережения");
    }

    public bool IsOn => _isOn;
    public int Temperature => _temperature;
    public string Mode => _mode;
}

public class LightOnCommand : ICommand
{
    private Light _light;
    private bool _previousState;

    public LightOnCommand(Light light)
    {
        _light = light ?? throw new ArgumentNullException(nameof(light));
    }

    public void Execute()
    {
        _previousState = _light.IsOn;
        _light.On();
    }

    public void Undo()
    {
        if (!_previousState)
        {
            _light.Off();
        }
    }

    public string GetDescription()
    {
        return $"Включение света в {_light.Location}";
    }
}

public class LightOffCommand : ICommand
{
    private Light _light;
    private bool _previousState;

    public LightOffCommand(Light light)
    {
        _light = light ?? throw new ArgumentNullException(nameof(light));
    }

    public void Execute()
    {
        _previousState = _light.IsOn;
        _light.Off();
    }

    public void Undo()
    {
        if (_previousState)
        {
            _light.On();
        }
    }

    public string GetDescription()
    {
        return $"Выключение света в {_light.Location}";
    }
}

public class TelevisionOnCommand : ICommand
{
    private Television _tv;
    private bool _previousState;

    public TelevisionOnCommand(Television tv)
    {
        _tv = tv ?? throw new ArgumentNullException(nameof(tv));
    }

    public void Execute()
    {
        _previousState = _tv.IsOn;
        _tv.On();
    }

    public void Undo()
    {
        if (!_previousState)
        {
            _tv.Off();
        }
    }

    public string GetDescription()
    {
        return $"Включение телевизора";
    }
}

public class TelevisionOffCommand : ICommand
{
    private Television _tv;
    private bool _previousState;

    public TelevisionOffCommand(Television tv)
    {
        _tv = tv ?? throw new ArgumentNullException(nameof(tv));
    }

    public void Execute()
    {
        _previousState = _tv.IsOn;
        _tv.Off();
    }

    public void Undo()
    {
        if (_previousState)
        {
            _tv.On();
        }
    }

    public string GetDescription()
    {
        return $"Выключение телевизора";
    }
}

public class AirConditionerOnCommand : ICommand
{
    private AirConditioner _ac;
    private bool _previousState;

    public AirConditionerOnCommand(AirConditioner ac)
    {
        _ac = ac ?? throw new ArgumentNullException(nameof(ac));
    }

    public void Execute()
    {
        _previousState = _ac.IsOn;
        _ac.On();
    }

    public void Undo()
    {
        if (!_previousState)
        {
            _ac.Off();
        }
    }

    public string GetDescription()
    {
        return $"Включение кондиционера";
    }
}

public class AirConditionerOffCommand : ICommand
{
    private AirConditioner _ac;
    private bool _previousState;

    public AirConditionerOffCommand(AirConditioner ac)
    {
        _ac = ac ?? throw new ArgumentNullException(nameof(ac));
    }

    public void Execute()
    {
        _previousState = _ac.IsOn;
        _ac.Off();
    }

    public void Undo()
    {
        if (_previousState)
        {
            _ac.On();
        }
    }

    public string GetDescription()
    {
        return $"Выключение кондиционера";
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
        return $"Изменение яркости на {_newBrightness}%";
    }
}

public class TemperatureCommand : ICommand
{
    private AirConditioner _ac;
    private int _previousTemperature;
    private int _newTemperature;

    public TemperatureCommand(AirConditioner ac, int temperature)
    {
        _ac = ac ?? throw new ArgumentNullException(nameof(ac));
        _newTemperature = temperature;
    }

    public void Execute()
    {
        _previousTemperature = _ac.Temperature;
        _ac.SetTemperature(_newTemperature);
    }

    public void Undo()
    {
        _ac.SetTemperature(_previousTemperature);
    }

    public string GetDescription()
    {
        return $"Установка температуры на {_newTemperature}°C";
    }
}

public class EnergySavingCommand : ICommand
{
    private AirConditioner _ac;
    private string _previousMode;
    private int _previousTemperature;

    public EnergySavingCommand(AirConditioner ac)
    {
        _ac = ac ?? throw new ArgumentNullException(nameof(ac));
    }

    public void Execute()
    {
        _previousMode = _ac.Mode;
        _previousTemperature = _ac.Temperature;
        _ac.SetEnergySavingMode();
    }

    public void Undo()
    {
        _ac.SetMode(_previousMode);
        _ac.SetTemperature(_previousTemperature);
    }

    public string GetDescription()
    {
        return $"Включение режима энергосбережения";
    }
}

public class MacroCommand : ICommand
{
    private List<ICommand> _commands;
    private string _description;

    public MacroCommand(string description, params ICommand[] commands)
    {
        _commands = commands?.ToList() ?? new List<ICommand>();
        _description = description ?? "Макрокоманда";
    }

    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }

    public void Undo()
    {
        foreach (var command in _commands.Reverse<ICommand>())
        {
            command.Undo();
        }
    }

    public string GetDescription()
    {
        return _description;
    }
}

public class RemoteControl
{
    private Dictionary<int, ICommand> _onCommands;
    private Dictionary<int, ICommand> _offCommands;
    private Stack<ICommand> _commandHistory;
    private List<string> _commandLog;

    public RemoteControl()
    {
        _onCommands = new Dictionary<int, ICommand>();
        _offCommands = new Dictionary<int, ICommand>();
        _commandHistory = new Stack<ICommand>();
        _commandLog = new List<string>();
    }

    public void SetCommand(int slot, ICommand onCommand, ICommand offCommand)
    {
        if (onCommand != null)
            _onCommands[slot] = onCommand;
        
        if (offCommand != null)
            _offCommands[slot] = offCommand;
    }

    public void PressOnButton(int slot)
    {
        if (_onCommands.ContainsKey(slot))
        {
            var command = _onCommands[slot];
            command.Execute();
            _commandHistory.Push(command);
            LogCommand($"Выполнено: {command.GetDescription()}");
        }
        else
        {
            Console.WriteLine("Ошибка: Команда не назначена на эту кнопку");
        }
    }

    public void PressOffButton(int slot)
    {
        if (_offCommands.ContainsKey(slot))
        {
            var command = _offCommands[slot];
            command.Execute();
            _commandHistory.Push(command);
            LogCommand($"Выполнено: {command.GetDescription()}");
        }
        else
        {
            Console.WriteLine("Ошибка: Команда не назначена на эту кнопку");
        }
    }

    public void PressUndoButton()
    {
        if (_commandHistory.Count > 0)
        {
            var command = _commandHistory.Pop();
            command.Undo();
            LogCommand($"Отменено: {command.GetDescription()}");
        }
        else
        {
            Console.WriteLine("Нет команд для отмены");
        }
    }

    private void LogCommand(string message)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
        _commandLog.Add(logEntry);
        Console.WriteLine(logEntry);
    }

    public void ShowCommandLog()
    {
        Console.WriteLine("\n=== Журнал команд ===");
        if (_commandLog.Count == 0)
        {
            Console.WriteLine("Журнал пуст");
            return;
        }

        foreach (var log in _commandLog)
        {
            Console.WriteLine(log);
        }
    }

    public void ShowButtonLayout()
    {
        Console.WriteLine("\n=== Расположение кнопок пульта ===");
        for (int i = 1; i <= 7; i++)
        {
            string onCommand = _onCommands.ContainsKey(i) ? _onCommands[i].GetDescription() : "Не назначено";
            string offCommand = _offCommands.ContainsKey(i) ? _offCommands[i].GetDescription() : "Не назначено";
            Console.WriteLine($"Кнопка {i}: ВКЛ - {onCommand}, ВЫКЛ - {offCommand}");
        }
    }
}

class SmartHomeProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система управления умным домом ===\n");

        RemoteControl remote = new RemoteControl();

        Light livingRoomLight = new Light("Гостиная");
        Light kitchenLight = new Light("Кухня");
        Television livingRoomTV = new Television("Гостиная");
        AirConditioner bedroomAC = new AirConditioner("Спальня");

        ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
        ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
        ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
        ICommand kitchenLightOff = new LightOffCommand(kitchenLight);
        ICommand tvOn = new TelevisionOnCommand(livingRoomTV);
        ICommand tvOff = new TelevisionOffCommand(livingRoomTV);
        ICommand acOn = new AirConditionerOnCommand(bedroomAC);
        ICommand acOff = new AirConditionerOffCommand(bedroomAC);
        ICommand brightnessLow = new BrightnessCommand(livingRoomLight, 30);
        ICommand brightnessHigh = new BrightnessCommand(livingRoomLight, 80);
        ICommand setTempCool = new TemperatureCommand(bedroomAC, 18);
        ICommand setTempWarm = new TemperatureCommand(bedroomAC, 25);
        ICommand energySaving = new EnergySavingCommand(bedroomAC);

        ICommand eveningMode = new MacroCommand("Вечерний режим", 
            livingRoomLightOn, brightnessLow, tvOn, acOn, setTempWarm);

        ICommand goodNightMode = new MacroCommand("Ночной режим",
            livingRoomLightOff, tvOff, setTempCool);

        remote.SetCommand(1, livingRoomLightOn, livingRoomLightOff);
        remote.SetCommand(2, kitchenLightOn, kitchenLightOff);
        remote.SetCommand(3, tvOn, tvOff);
        remote.SetCommand(4, acOn, acOff);
        remote.SetCommand(5, brightnessHigh, brightnessLow);
        remote.SetCommand(6, setTempWarm, setTempCool);
        remote.SetCommand(7, eveningMode, goodNightMode);

        remote.ShowButtonLayout();

        Console.WriteLine("\n=== Тестирование команд ===");
        
        remote.PressOnButton(1);
        remote.PressOnButton(3);
        remote.PressOnButton(4);
        remote.PressOnButton(5);
        remote.PressOffButton(5);
        remote.PressOnButton(6);

        Console.WriteLine("\n=== Тестирование макрокоманд ===");
        remote.PressOnButton(7);
        
        Console.WriteLine("\n=== Тестирование отмены ===");
        remote.PressUndoButton();
        remote.PressUndoButton();

        Console.WriteLine("\n=== Тестирование ошибок ===");
        remote.PressOnButton(10);

        remote.ShowCommandLog();
    }
}
