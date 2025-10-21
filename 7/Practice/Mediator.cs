using System;
using System.Collections.Generic;
using System.Linq;

public interface IMediator
{
    void RegisterUser(IUser user, string channel);
    void UnregisterUser(IUser user, string channel);
    void SendMessage(string message, IUser sender, string channel);
    void SendPrivateMessage(string message, IUser sender, string recipientName);
    void CreateChannel(string channelName);
    void RemoveChannel(string channelName);
    void DisplayChannelUsers(string channel);
    List<string> GetChannels();
}

public interface IUser
{
    string Name { get; }
    void ReceiveMessage(string message, string senderName, string channel);
    void ReceivePrivateMessage(string message, string senderName);
    void Notify(string notification);
    void JoinChannel(string channel);
    void LeaveChannel(string channel);
    List<string> GetJoinedChannels();
}

public class ChatMediator : IMediator
{
    private Dictionary<string, List<IUser>> _channels;
    private Dictionary<string, IUser> _allUsers;
    private List<PrivateMessage> _privateMessages;

    public ChatMediator()
    {
        _channels = new Dictionary<string, List<IUser>>();
        _allUsers = new Dictionary<string, IUser>();
        _privateMessages = new List<PrivateMessage>();
        
        CreateChannel("general");
        CreateChannel("random");
    }

    public void RegisterUser(IUser user, string channel = "general")
    {
        if (_allUsers.ContainsKey(user.Name))
        {
            throw new InvalidOperationException($"Пользователь с именем '{user.Name}' уже зарегистрирован");
        }

        _allUsers[user.Name] = user;
        
        if (!_channels.ContainsKey(channel))
        {
            CreateChannel(channel);
        }

        JoinUserToChannel(user, channel);

        string notification = $"{user.Name} присоединился к чату и каналу {channel}";
        BroadcastToChannel(notification, null, channel);
        
        Console.WriteLine(notification);
    }

    public void UnregisterUser(IUser user, string channel)
    {
        if (_allUsers.ContainsKey(user.Name))
        {
            LeaveUserFromChannel(user, channel);
            
            if (user.GetJoinedChannels().Count == 0)
            {
                _allUsers.Remove(user.Name);
                string notification = $"{user.Name} покинул чат";
                BroadcastToAllChannels(notification, null);
                Console.WriteLine(notification);
            }
        }
    }

    public void SendMessage(string message, IUser sender, string channel)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        if (!_allUsers.ContainsKey(sender.Name))
            throw new InvalidOperationException($"Пользователь {sender.Name} не зарегистрирован в чате");

        if (!_channels.ContainsKey(channel))
            throw new InvalidOperationException($"Канал '{channel}' не существует");

        if (!_channels[channel].Contains(sender))
            throw new InvalidOperationException($"Пользователь {sender.Name} не состоит в канале '{channel}'");

        string formattedMessage = $"[{channel}] {sender.Name}: {message}";
        
        foreach (var user in _channels[channel])
        {
            if (user != sender)
            {
                user.ReceiveMessage(message, sender.Name, channel);
            }
        }

        Console.WriteLine(formattedMessage);
    }

    public void SendPrivateMessage(string message, IUser sender, string recipientName)
    {
        if (sender == null)
            throw new ArgumentNullException(nameof(sender));

        if (!_allUsers.ContainsKey(sender.Name))
            throw new InvalidOperationException($"Пользователь {sender.Name} не зарегистрирован в чате");

        if (!_allUsers.ContainsKey(recipientName))
            throw new InvalidOperationException($"Пользователь {recipientName} не найден");

        if (sender.Name == recipientName)
            throw new InvalidOperationException("Нельзя отправлять сообщения самому себе");

        var recipient = _allUsers[recipientName];
        
        recipient.ReceivePrivateMessage(message, sender.Name);
        sender.ReceivePrivateMessage($"(отправлено {recipientName}): {message}", sender.Name);

        _privateMessages.Add(new PrivateMessage(sender.Name, recipientName, message, DateTime.Now));
        
        Console.WriteLine($"[ЛС] {sender.Name} -> {recipientName}: {message}");
    }

    public void CreateChannel(string channelName)
    {
        if (_channels.ContainsKey(channelName))
            throw new InvalidOperationException($"Канал '{channelName}' уже существует");

        _channels[channelName] = new List<IUser>();
        Console.WriteLine($"Создан новый канал: {channelName}");
    }

    public void RemoveChannel(string channelName)
    {
        if (!_channels.ContainsKey(channelName))
            throw new InvalidOperationException($"Канал '{channelName}' не существует");

        if (channelName == "general")
            throw new InvalidOperationException("Нельзя удалить основной канал 'general'");

        var usersInChannel = new List<IUser>(_channels[channelName]);
        foreach (var user in usersInChannel)
        {
            LeaveUserFromChannel(user, channelName);
            user.JoinChannel("general");
        }

        _channels.Remove(channelName);
        Console.WriteLine($"Канал '{channelName}' удален");
    }

    public void DisplayChannelUsers(string channel)
    {
        if (!_channels.ContainsKey(channel))
        {
            Console.WriteLine($"Канал '{channel}' не существует");
            return;
        }

        Console.WriteLine($"\nПользователи в канале '{channel}':");
        foreach (var user in _channels[channel])
        {
            string role = user is AdminUser ? "[АДМИН]" : "[ПОЛЬЗОВАТЕЛЬ]";
            Console.WriteLine($"- {user.Name} {role}");
        }
    }

    public List<string> GetChannels()
    {
        return _channels.Keys.ToList();
    }

    private void JoinUserToChannel(IUser user, string channel)
    {
        if (!_channels.ContainsKey(channel))
            CreateChannel(channel);

        if (!_channels[channel].Contains(user))
        {
            _channels[channel].Add(user);
            user.JoinChannel(channel);
        }
    }

    private void LeaveUserFromChannel(IUser user, string channel)
    {
        if (_channels.ContainsKey(channel) && _channels[channel].Contains(user))
        {
            _channels[channel].Remove(user);
            user.LeaveChannel(channel);
        }
    }

    private void BroadcastToChannel(string message, IUser sender, string channel)
    {
        if (_channels.ContainsKey(channel))
        {
            foreach (var user in _channels[channel])
            {
                if (user != sender)
                {
                    user.Notify(message);
                }
            }
        }
    }

    private void BroadcastToAllChannels(string message, IUser sender)
    {
        foreach (var channel in _channels.Values)
        {
            foreach (var user in channel)
            {
                if (user != sender)
                {
                    user.Notify(message);
                }
            }
        }
    }
}

public class PrivateMessage
{
    public string Sender { get; }
    public string Recipient { get; }
    public string Message { get; }
    public DateTime Timestamp { get; }

    public PrivateMessage(string sender, string recipient, string message, DateTime timestamp)
    {
        Sender = sender;
        Recipient = recipient;
        Message = message;
        Timestamp = timestamp;
    }
}

public class User : IUser
{
    public string Name { get; }
    private IMediator _mediator;
    private List<string> _joinedChannels;

    public User(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя пользователя не может быть пустым");

        Name = name.Trim();
        _joinedChannels = new List<string>();
    }

    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void ReceiveMessage(string message, string senderName, string channel)
    {
        Console.WriteLine($"{Name} получил в канале '{channel}' от {senderName}: {message}");
    }

    public void ReceivePrivateMessage(string message, string senderName)
    {
        Console.WriteLine($"{Name} получил ЛС от {senderName}: {message}");
    }

    public void Notify(string notification)
    {
        Console.WriteLine($"{Name} получил уведомление: {notification}");
    }

    public void SendMessage(string message, string channel)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        _mediator.SendMessage(message, this, channel);
    }

    public void SendPrivateMessage(string message, string recipientName)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        _mediator.SendPrivateMessage(message, this, recipientName);
    }

    public void JoinChannel(string channel)
    {
        if (!_joinedChannels.Contains(channel))
        {
            _joinedChannels.Add(channel);
        }
    }

    public void LeaveChannel(string channel)
    {
        _joinedChannels.Remove(channel);
    }

    public List<string> GetJoinedChannels()
    {
        return new List<string>(_joinedChannels);
    }

    public void SwitchChannel(string fromChannel, string toChannel)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        _mediator.UnregisterUser(this, fromChannel);
        _mediator.RegisterUser(this, toChannel);
    }
}

public class AdminUser : User
{
    private ChatMediator _chatMediator;

    public AdminUser(string name) : base(name)
    {
    }

    public void SetChatMediator(ChatMediator mediator)
    {
        _chatMediator = mediator;
    }

    public void BanUser(string userName, string channel)
    {
        if (_chatMediator == null)
            throw new InvalidOperationException("Администратор не связан с медиатором");

        try
        {
            var userField = _chatMediator.GetType().GetField("_allUsers", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (userField != null)
            {
                var allUsers = userField.GetValue(_chatMediator) as Dictionary<string, IUser>;
                if (allUsers != null && allUsers.ContainsKey(userName))
                {
                    _chatMediator.UnregisterUser(allUsers[userName], channel);
                    Console.WriteLine($"Пользователь {userName} забанен в канале {channel} администратором {Name}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при бане пользователя: {ex.Message}");
        }
    }

    public void CreateNewChannel(string channelName)
    {
        if (_chatMediator == null)
            throw new InvalidOperationException("Администратор не связан с медиатором");

        _chatMediator.CreateChannel(channelName);
    }

    public void RemoveChannel(string channelName)
    {
        if (_chatMediator == null)
            throw new InvalidOperationException("Администратор не связан с медиатором");

        _chatMediator.RemoveChannel(channelName);
    }

    public override void ReceiveMessage(string message, string senderName, string channel)
    {
        Console.WriteLine($"{Name} (админ) получил в канале '{channel}' от {senderName}: {message}");
        
        if (message.ToLower().Contains("помощь"))
        {
            SendMessage("Администратор здесь! Чем могу помочь?", channel);
        }
    }
}

public class CrossChannelUser : User
{
    public CrossChannelUser(string name) : base(name)
    {
    }

    public void SendCrossChannelMessage(string message, string fromChannel, string toChannel)
    {
        if (_mediator == null)
            throw new InvalidOperationException("Пользователь не зарегистрирован в чате");

        try
        {
            _mediator.SendMessage($"[Переслано из {fromChannel}] {message}", this, toChannel);
            Console.WriteLine($"{Name} переслал сообщение из {fromChannel} в {toChannel}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при кросс-канальной отправке: {ex.Message}");
        }
    }
}

class AdvancedChatProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Расширенная чат-система с поддержкой каналов ===\n");

        ChatMediator chatMediator = new ChatMediator();

        User alice = new User("Алиса");
        User bob = new User("Боб");
        AdminUser admin = new AdminUser("Администратор");
        CrossChannelUser charlie = new CrossChannelUser("Чарли");

        try
        {
            chatMediator.RegisterUser(alice, "general");
            chatMediator.RegisterUser(bob, "general");
            chatMediator.RegisterUser(admin, "general");
            chatMediator.RegisterUser(charlie, "random");
            
            admin.SetChatMediator(chatMediator);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка регистрации: {ex.Message}");
        }

        Console.WriteLine("\n=== Доступные каналы ===");
        foreach (var channel in chatMediator.GetChannels())
        {
            Console.WriteLine($"- {channel}");
        }

        Console.WriteLine("\n=== Общение в каналах ===");
        alice.SendMessage("Всем привет из general!", "general");
        bob.SendMessage("Привет, Алиса!", "general");
        charlie.SendMessage("Привет из random канала!", "random");

        Console.WriteLine("\n=== Приватные сообщения ===");
        alice.SendPrivateMessage("Секретное сообщение для Боба", "Боб");
        bob.SendPrivateMessage("Получил твое сообщение!", "Алиса");

        Console.WriteLine("\n=== Кросс-канальное общение ===");
        charlie.SendCrossChannelMessage("Привет из random канала!", "random", "general");

        Console.WriteLine("\n=== Действия администратора ===");
        admin.CreateNewChannel("техподдержка");
        chatMediator.DisplayChannelUsers("general");
        
        Console.WriteLine("\n=== Смена канала ===");
        alice.SwitchChannel("general", "техподдержка");
        alice.SendMessage("Я перешла в техподдержку!", "техподдержка");

        Console.WriteLine("\n=== Тестирование ошибок ===");
        try
        {
            User diana = new User("Диана");
            diana.SendMessage("Это вызовет ошибку", "general");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ожидаемая ошибка: {ex.Message}");
        }

        try
        {
            alice.SendMessage("Сообщение в несуществующий канал", "несуществующий");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ожидаемая ошибка: {ex.Message}");
        }

        Console.WriteLine("\n=== Финальная статистика ===");
        foreach (var channel in chatMediator.GetChannels())
        {
            chatMediator.DisplayChannelUsers(channel);
        }
    }
}
