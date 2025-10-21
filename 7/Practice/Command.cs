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

    public bool IsOn => _isOn;
    public int Temperature => _temperature;
    public string Mode => _mode;
}

public class SmartCurtains
{
    private string _location;
    private bool _isOpen;
    private int _openPercentage;

    public SmartCurtains(string location)
    {
        _location = location;
        _isOpen = false;
        _openPercentage = 0;
    }

    public void Open()
    {
        _isOpen = true;
        _openPercentage = 100;
        Console.WriteLine($"Шторы в {_location} открыты");
    }

    public void Close()
    {
        _isOpen = false;
        _openPercentage = 0;
        Console.WriteLine($"Шторы в {_location} закрыты");
    }

    public void SetOpenPercentage(int percentage)
    {
        if (percentage < 0) percentage = 0;
        if (percentage > 100) percentage = 100;
        
        _openPercentage = percentage;
        _isOpen = percentage > 0;
        Console.WriteLine($"Шторы в {_location} установлены на {percentage}% открытия");
    }

    public bool IsOpen => _isOpen;
    public int OpenPercentage => _openPercentage;
}

public class MusicPlayer
{
    private string _location;
    private bool _isOn;
    private int _volume;
    private string _currentTrack;

    public MusicPlayer(string location)
    {
        _location = location;
        _isOn = false;
        _volume = 50;
        _currentTrack = "Нет трека";
    }

    public void On()
    {
        _isOn = true;
        Console.WriteLine($"Музыкальный плеер в {_location} включен. Трек: {_currentTrack}, громкость: {_volume}%");
    }

    public void Off()
    {
        _isOn = false;
        Console.WriteLine($"Музыкальный плеер в {_location} выключен");
    }

    public void SetVolume(int volume)
    {
        if (volume < 0) volume = 0;
        if (volume > 100) volume = 100;
        
        _volume = volume;
        Console.WriteLine($"Громкость музыкального плеера в {_location} установлена на {volume}%");
    }

    public void PlayTrack(string track)
    {
        _currentTrack = track;
        Console.WriteLine($"Музыкальный плеер в {_location} воспроизводит: {track}");
    }

    public bool IsOn => _isOn;
    public int Volume => _volume;
    public string CurrentTrack => _currentTrack;
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
        return $"Включение телевизора в {_tv.Location}";
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
        return $"Выключение телевизора в {_tv.Location}";
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
        return $"Включение кондиционера в {_ac.Location}";
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
        return $"Выключение кондиционера в {_ac.Location}";
    }
}

public class CurtainsOpenCommand : ICommand
{
    private SmartCurtains _curtains;
    private bool _previousState;
    private int _previousPercentage;

    public CurtainsOpenCommand(SmartCurtains curtains)
    {
        _curtains = curtains ?? throw new ArgumentNullException(nameof(curtains));
    }

    public void Execute()
    {
        _previousState = _curtains.IsOpen;
        _previousPercentage = _curtains.OpenPercentage;
        _curtains.Open();
    }

    public void Undo()
    {
        if (!_previousState)
        {
            _curtains.Close();
        }
        else
        {
            _curtains.SetOpenPercentage(_previousPercentage);
        }
    }

    public string GetDescription()
    {
        return $"Открытие штор в {_curtains.Location}";
    }
}

public class MusicPlayerOnCommand : ICommand
{
    private MusicPlayer _player;
    private bool _previousState;

    public MusicPlayerOnCommand(MusicPlayer player)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
    }

    public void Execute()
    {
        _previousState = _player.IsOn;
        _player.On();
    }

    public void Undo()
    {
        if (!_previousState)
        {
            _player.Off();
        }
    }

    public string GetDescription()
    {
        return $"Включение музыкального плеера в {_player.Location}";
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
        return $"Установка температуры кондиционера в {_ac.Location} на {_newTemperature}°C";
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

    public void AddCommand(ICommand command)
    {
        if (command != null)
        {
            _commands.Add(command);
        }
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
    private Stack<ICommand> _redoHistory;
    private List<string> _commandLog;
    private bool _isRecordingMacro;
    private MacroCommand _currentMacro;

    public RemoteControl()
    {
        _onCommands = new Dictionary<int, ICommand>();
        _offCommands = new Dictionary<int, ICommand>();
        _commandHistory = new Stack<ICommand>();
        _redoHistory = new Stack<ICommand>();
        _commandLog = new List<string>();
        _isRecordingMacro = false;
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
            ExecuteCommand(command);
            
            if (_isRecordingMacro && _currentMacro != null)
            {
                _currentMacro.AddCommand(command);
            }
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
            ExecuteCommand(command);
            
            if (_isRecordingMacro && _currentMacro != null)
            {
                _currentMacro.AddCommand(command);
            }
        }
        else
        {
            Console.WriteLine("Ошибка: Команда не назначена на эту кнопку");
        }
    }

    private void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
        _redoHistory.Clear();
        LogCommand($"Выполнено: {command.GetDescription()}");
    }

    public void PressUndoButton()
    {
        if (_commandHistory.Count > 0)
        {
            var command = _commandHistory.Pop();
            command.Undo();
            _redoHistory.Push(command);
            LogCommand($"Отменено: {command.GetDescription()}");
        }
        else
        {
            Console.WriteLine("Нет команд для отмены");
        }
    }

    public void PressRedoButton()
    {
        if (_redoHistory.Count > 0)
        {
            var command = _redoHistory.Pop();
            command.Execute();
            _commandHistory.Push(command);
            LogCommand($"Повторено: {command.GetDescription()}");
        }
        else
        {
            Console.WriteLine("Нет команд для повтора");
        }
    }

    public void StartMacroRecording(string macroName)
    {
        if (_isRecordingMacro)
        {
            Console.WriteLine("Уже ведется запись макрокоманды");
            return;
        }

        _isRecordingMacro = true;
        _currentMacro = new MacroCommand(macroName);
        Console.WriteLine($"Начата запись макрокоманды: {macroName}");
    }

    public ICommand StopMacroRecording()
    {
        if (!_isRecordingMacro)
        {
            Console.WriteLine("Запись макрокоманды не активна");
            return null;
        }

        _isRecordingMacro = false;
        var macro = _currentMacro;
        _currentMacro = null;
        Console.WriteLine($"Завершена запись макрокоманды: {macro.GetDescription()}");
        return macro;
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
        for (int i = 1; i <= 10; i++)
        {
            string onCommand = _onCommands.ContainsKey(i) ? _onCommands[i].GetDescription() : "Не назначено";
            string offCommand = _offCommands.ContainsKey(i) ? _offCommands[i].GetDescription() : "Не назначено";
            Console.WriteLine($"Кнопка {i}: ВКЛ - {onCommand}, ВЫКЛ - {offCommand}");
        }
    }

    public void ClearHistory()
    {
        _commandHistory.Clear();
        _redoHistory.Clear();
        _commandLog.Clear();
        Console.WriteLine("История команд очищена");
    }
}

class SmartHomeProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Расширенная система управления умным домом ===\n");

        RemoteControl remote = new RemoteControl();

        Light livingRoomLight = new Light("Гостиная");
        Light kitchenLight = new Light("Кухня");
        Television livingRoomTV = new Television("Гостиная");
        AirConditioner bedroomAC = new AirConditioner("Спальня");
        SmartCurtains livingRoomCurtains = new SmartCurtains("Гостиная");
        MusicPlayer livingRoomPlayer = new MusicPlayer("Гостиная");

        ICommand livingRoomLightOn = new LightOnCommand(livingRoomLight);
        ICommand livingRoomLightOff = new LightOffCommand(livingRoomLight);
        ICommand kitchenLightOn = new LightOnCommand(kitchenLight);
        ICommand kitchenLightOff = new LightOffCommand(kitchenLight);
        ICommand tvOn = new TelevisionOnCommand(livingRoomTV);
        ICommand tvOff = new TelevisionOffCommand(livingRoomTV);
        ICommand acOn = new AirConditionerOnCommand(bedroomAC);
        ICommand acOff = new AirConditionerOffCommand(bedroomAC);
        ICommand curtainsOpen = new CurtainsOpenCommand(livingRoomCurtains);
        ICommand musicOn = new MusicPlayerOnCommand(livingRoomPlayer);
        ICommand brightnessLow = new BrightnessCommand(livingRoomLight, 30);
        ICommand brightnessHigh = new BrightnessCommand(livingRoomLight, 80);
        ICommand setTempCool = new TemperatureCommand(bedroomAC, 18);
        ICommand setTempWarm = new TemperatureCommand(bedroomAC, 25);

        ICommand eveningMode = new MacroCommand("Вечерний режим", 
            livingRoomLightOn, brightnessLow, curtainsOpen, tvOn, setTempWarm);

        ICommand goodNightMode = new MacroCommand("Ночной режим",
            livingRoomLightOff, tvOff, setTempCool);

        remote.SetCommand(1, livingRoomLightOn, livingRoomLightOff);
        remote.SetCommand(2, kitchenLightOn, kitchenLightOff);
        remote.SetCommand(3, tvOn, tvOff);
        remote.SetCommand(4, acOn, acOff);
        remote.SetCommand(5, curtainsOpen, new CurtainsOpenCommand(livingRoomCurtains));
        remote.SetCommand(6, musicOn, new MusicPlayerOnCommand(livingRoomPlayer));
        remote.SetCommand(7, brightnessHigh, brightnessLow);
        remote.SetCommand(8, setTempWarm, setTempCool);
        remote.SetCommand(9, eveningMode, goodNightMode);

        remote.ShowButtonLayout();

        Console.WriteLine("\n=== Тестирование отдельных команд ===");
        remote.PressOnButton(1);
        remote.PressOnButton(3);
        remote.PressOnButton(4);
        remote.PressOnButton(5);
        remote.PressOnButton(7);

        Console.WriteLine("\n=== Тестирование отмены и повтора ===");
        remote.PressUndoButton();
        remote.PressUndoButton();
        remote.PressRedoButton();

        Console.WriteLine("\n=== Тестирование макрокоманд ===");
        remote.PressOnButton(9);

        Console.WriteLine("\n=== Тестирование записи макрокоманды ===");
        remote.StartMacroRecording("Утренний режим");
        remote.PressOnButton(1);
        remote.PressOnButton(5);
        remote.PressOnButton(6);
        remote.PressOnButton(7);
        var morningMacro = remote.StopMacroRecording();

        if (morningMacro != null)
        {
            remote.SetCommand(10, morningMacro, new MacroCommand("Выключение утреннего режима"));
            Console.WriteLine("\n=== Выполнение записанной макрокоманды ===");
            remote.PressOnButton(10);
        }

        Console.WriteLine("\n=== Тестирование ошибок ===");
        remote.PressOnButton(15);
        remote.PressOffButton(20);

        remote.ShowCommandLog();
    }
}
