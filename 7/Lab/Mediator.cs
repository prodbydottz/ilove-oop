using System;
using System.Collections.Generic;
using System.Linq;

public interface IMediator
{
    void RegisterUser(User user);
    void UnregisterUser(User user);
    void SendMessage(string message, User sender);
    void SendPrivateMessage(string message, User sender, string recipientName);
    void BroadcastMessage(string message, User sender);
    void DisplayUserList();
}

public abstract class User
{
    public string Name { get; private set; }
    protected IMediator _mediator;

    protected User(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя пользователя не может быть пустым");

        Name = name.Trim();
    }

    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void Send(string message)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Сообщение не может быть пустым");

        _mediator.SendMessage(message, this);
    }

    public void SendTo(string message, string recipientName)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Сообщение не может быть пустым");

        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentException("Имя получателя не может быть пустым");

        _mediator.SendPrivateMessage(message, this, recipientName);
    }

    public virtual void Receive(string message, string senderName)
    {
        Console.WriteLine($"{Name} получил сообщение от {senderName}: {message}");
    }

    public virtual void ReceivePrivate(string message, string senderName)
    {
        Console.WriteLine($"{Name} получил ЛС от {senderName}: {message}");
    }

    public virtual void Notify(string notification)
    {
        Console.WriteLine($"{Name} получил уведомление: {notification}");
    }
}

public class ChatMediator : IMediator
{
    private Dictionary<string, User> _users;
    private List<string> _messageHistory;
    private Dictionary<string, List<string>> _privateChats;

    public string RoomName { get; private set; }

    public ChatMediator(string roomName)
    {
        RoomName = roomName;
        _users = new Dictionary<string, User>();
        _messageHistory = new List<string>();
        _privateChats = new Dictionary<string, List<string>>();
    }

    public void RegisterUser(User user)
    {
        if (_users.ContainsKey(user.Name))
        {
            throw new InvalidOperationException($"Пользователь с именем '{user.Name}' уже зарегистрирован");
        }

        _users[user.Name] = user;
        user.SetMediator(this);

        string joinMessage = $"{user.Name} присоединился к чату";
        BroadcastMessage(joinMessage, null);
        
        _messageHistory.Add($"[СИСТЕМА] {joinMessage}");
        
        Console.WriteLine($"{joinMessage}");
    }

    public void UnregisterUser(User user)
    {
        if (_users.ContainsKey(user.Name))
        {
            _users.Remove(user.Name);
            
            string leaveMessage = $"{user.Name} покинул чат";
            BroadcastMessage(leaveMessage, null);
            
            _messageHistory.Add($"[СИСТЕМА] {leaveMessage}");
            
            Console.WriteLine($"{leaveMessage}");
        }
    }

    public void SendMessage(string message, User sender)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        if (!_users.ContainsKey(sender.Name))
            throw new InvalidOperationException($"Пользователь {sender.Name} не зарегистрирован в чате");

        string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] {sender.Name}: {message}";
        _messageHistory.Add(formattedMessage);

        foreach (var user in _users.Values)
        {
            if (user != sender)
            {
                user.Receive(message, sender.Name);
            }
        }

        Console.WriteLine($"{formattedMessage}");
    }

    public void SendPrivateMessage(string message, User sender, string recipientName)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        if (!_users.ContainsKey(sender.Name))
            throw new InvalidOperationException($"Пользователь {sender.Name} не зарегистрирован в чате");

        if (!_users.ContainsKey(recipientName))
            throw new InvalidOperationException($"Пользователь {recipientName} не найден в чате");

        if (sender.Name == recipientName)
            throw new InvalidOperationException("Нельзя отправлять сообщения самому себе");

        string privateMessage = $"[{DateTime.Now:HH:mm:ss}] [ЛС] {sender.Name} -> {recipientName}: {message}";
        
        string chatKey = GetChatKey(sender.Name, recipientName);
        if (!_privateChats.ContainsKey(chatKey))
        {
            _privateChats[chatKey] = new List<string>();
        }
        _privateChats[chatKey].Add(privateMessage);

        _users[recipientName].ReceivePrivate(message, sender.Name);

        Console.WriteLine($"[ЛС] {sender.Name} -> {recipientName}: {message}");
    }

    public void BroadcastMessage(string message, User sender)
    {
        string broadcastMessage = $"[{DateTime.Now:HH:mm:ss}] [СИСТЕМА] {message}";
        _messageHistory.Add(broadcastMessage);

        foreach (var user in _users.Values)
        {
            user.Notify(message);
        }

        Console.WriteLine($"[СИСТЕМА] {message}");
    }

    public void DisplayUserList()
    {
        Console.WriteLine($"\nУчастники чата '{RoomName}':");
        if (_users.Count == 0)
        {
            Console.WriteLine("В чате нет участников");
            return;
        }

        foreach (var user in _users.Values)
        {
            string status = user is AdminUser ? "[АДМИН]" : "[ПОЛЬЗОВАТЕЛЬ]";
            Console.WriteLine($"- {user.Name} {status}");
        }
        Console.WriteLine($"Всего участников: {_users.Count}");
    }

    public void DisplayChatHistory()
    {
        Console.WriteLine($"\nИстория чата '{RoomName}':");
        if (_messageHistory.Count == 0)
        {
            Console.WriteLine("История сообщений пуста");
            return;
        }

        foreach (var message in _messageHistory.TakeLast(20))
        {
            Console.WriteLine(message);
        }
    }

    public void DisplayPrivateChatHistory(string user1, string user2)
    {
        string chatKey = GetChatKey(user1, user2);
        if (_privateChats.ContainsKey(chatKey))
        {
            Console.WriteLine($"\nИстория переписки {user1} <-> {user2}:");
            foreach (var message in _privateChats[chatKey])
            {
                Console.WriteLine(message);
            }
        }
        else
        {
            Console.WriteLine("История переписки не найдена");
        }
    }

    private string GetChatKey(string user1, string user2)
    {
        var users = new[] { user1, user2 }.OrderBy(u => u).ToArray();
        return $"{users[0]}_{users[1]}";
    }

    public bool UserExists(string userName)
    {
        return _users.ContainsKey(userName);
    }
}

public class RegularUser : User
{
    public RegularUser(string name) : base(name)
    {
    }

    public override void Receive(string message, string senderName)
    {
        Console.WriteLine($"{Name} видит сообщение в общем чате от {senderName}: {message}");
    }

    public override void ReceivePrivate(string message, string senderName)
    {
        Console.WriteLine($"{Name} получил приватное сообщение от {senderName}: {message}");
    }
}

public class AdminUser : User
{
    public AdminUser(string name) : base(name)
    {
    }

    public void BroadcastAnnouncement(string announcement)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Администратор не зарегистрирован в чате");

        _mediator.BroadcastMessage($"ВАЖНО от {Name}: {announcement}", this);
    }

    public void KickUser(string userName, ChatMediator chatRoom)
    {
        if (chatRoom.UserExists(userName))
        {
            var user = chatRoom.GetType().GetField("_users", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                .GetValue(chatRoom) as Dictionary<string, User>;
            
            if (user != null && user.ContainsKey(userName))
            {
                chatRoom.UnregisterUser(user[userName]);
                chatRoom.BroadcastMessage($"Пользователь {userName} был исключен администратором {Name}", this);
            }
        }
    }

    public override void Receive(string message, string senderName)
    {
        Console.WriteLine($"{Name} (админ) видит сообщение от {senderName}: {message}");
    }

    public override void ReceivePrivate(string message, string senderName)
    {
        Console.WriteLine($"{Name} (админ) получил ЛС от {senderName}: {message}");
    }
}

public class BotUser : User
{
    private List<string> _responses;

    public BotUser(string name) : base(name)
    {
        _responses = new List<string>
        {
            "Привет! Я бот этого чата",
            "Как дела?",
            "Интересный разговор!",
            "Погода сегодня отличная!",
            "Кто-нибудь хочет послушать шутку?",
            "Я всегда на связи!",
            "Не забывайте соблюдать правила чата!",
            "Спасибо за общение!",
            "Удачи всем!",
            "Добро пожаловать в наш чат!"
        };
    }

    public override void Receive(string message, string senderName)
    {
        Console.WriteLine($"{Name} обрабатывает сообщение от {senderName}");

        if (message.ToLower().Contains("привет") || message.ToLower().Contains("hello"))
        {
            Send($"Привет, {senderName}! Рад тебя видеть!");
        }
        else if (message.ToLower().Contains("бот"))
        {
            var random = new Random();
            string response = _responses[random.Next(_responses.Count)];
            Send(response);
        }
        else if (message.ToLower().Contains("время"))
        {
            Send($"Текущее время: {DateTime.Now:HH:mm:ss}");
        }
        else if (message.ToLower().Contains("помощь") || message.ToLower().Contains("help"))
        {
            Send("Доступные команды: напиши 'бот' для общения, 'время' для получения времени");
        }
    }

    public override void Notify(string notification)
    {
        if (notification.Contains("присоединился"))
        {
            Send("Добро пожаловать в чат! Напиши 'помощь' для списка команд");
        }
    }
}

class ChatProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система чата с паттерном 'Посредник' ===\n");

        ChatMediator mainChat = new ChatMediator("Главный чат");

        RegularUser alice = new RegularUser("Алиса");
        RegularUser bob = new RegularUser("Боб");
        AdminUser admin = new AdminUser("Администратор");
        BotUser chatBot = new BotUser("ЧатБот");

        try
        {
            mainChat.RegisterUser(alice);
            mainChat.RegisterUser(bob);
            mainChat.RegisterUser(admin);
            mainChat.RegisterUser(chatBot);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
        }

        mainChat.DisplayUserList();

        Console.WriteLine("\n=== Начало общения в чате ===");

        alice.Send("Всем привет! Как ваши дела?");
        bob.Send("Привет, Алиса! Все отлично, спасибо!");
        chatBot.Send("Приветствую всех участников чата!");
        
        alice.SendTo("Привет, Боб! Это приватное сообщение", "Боб");
        bob.SendTo("Получил твое ЛС, Алиса! Отвечаю тем же", "Алиса");

        admin.BroadcastAnnouncement("Напоминаю о правилах чата: будьте вежливы!");
        
        alice.Send("Бот, скажи какое-нибудь время");
        bob.Send("Бот, расскажи шутку");

        admin.Send("Всем участникам: не забывайте про наше собрание в 15:00");

        Console.WriteLine("\n=== Демонстрация исключения пользователя ===");
        try
        {
            RegularUser charlie = new RegularUser("Чарли");
            charlie.Send("Это сообщение вызовет ошибку - я не зарегистрирован!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Добавление нового пользователя ===");
        RegularUser diana = new RegularUser("Диана");
        mainChat.RegisterUser(diana);
        diana.Send("Всем привет! Я новенькая здесь");

        Console.WriteLine("\n=== Финальная статистика ===");
        mainChat.DisplayUserList();
        mainChat.DisplayChatHistory();
        mainChat.DisplayPrivateChatHistory("Алиса", "Боб");

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
