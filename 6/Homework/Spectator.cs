using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IObserver
{
    Task UpdateAsync(CurrencyPair currencyPair);
    string GetName();
}

public interface ISubject
{
    void RegisterObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    Task NotifyObserversAsync(CurrencyPair currencyPair);
    string GetSubjectName();
}

public class CurrencyPair
{
    public string BaseCurrency { get; set; }
    public string TargetCurrency { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal PreviousRate { get; set; }
    public decimal ChangePercent { get; set; }
    public DateTime LastUpdated { get; set; }

    public CurrencyPair(string baseCurrency, string targetCurrency, decimal exchangeRate, decimal previousRate)
    {
        BaseCurrency = baseCurrency;
        TargetCurrency = targetCurrency;
        ExchangeRate = exchangeRate;
        PreviousRate = previousRate;
        ChangePercent = previousRate != 0 ? ((exchangeRate - previousRate) / previousRate) * 100 : 0;
        LastUpdated = DateTime.Now;
    }

    public string GetPairSymbol()
    {
        return $"{BaseCurrency}/{TargetCurrency}";
    }

    public override string ToString()
    {
        string trend = ChangePercent >= 0 ? "📈" : "📉";
        return $"{GetPairSymbol()} {ExchangeRate:F4} ({trend} {Math.Abs(ChangePercent):F2}%)";
    }
}

public class CurrencyExchange : ISubject
{
    private List<IObserver> _observers;
    private Dictionary<string, decimal> _exchangeRates;
    private Dictionary<string, decimal> _previousRates;
    public string Name { get; private set; }

    public CurrencyExchange(string name)
    {
        Name = name;
        _observers = new List<IObserver>();
        _exchangeRates = new Dictionary<string, decimal>();
        _previousRates = new Dictionary<string, decimal>();
        
        InitializeDefaultRates();
    }

    private void InitializeDefaultRates()
    {
        _exchangeRates["USD/RUB"] = 92.50m;
        _exchangeRates["EUR/RUB"] = 99.80m;
        _exchangeRates["USD/EUR"] = 0.93m;
        _exchangeRates["GBP/RUB"] = 115.20m;
        _exchangeRates["CNY/RUB"] = 12.80m;
        
        foreach (var rate in _exchangeRates)
        {
            _previousRates[rate.Key] = rate.Value;
        }
    }

    public string GetSubjectName() => Name;

    public void RegisterObserver(IObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"✅ {observer.GetName()} подписан на обновления {Name}");
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
            Console.WriteLine($"❌ {observer.GetName()} отписан от обновлений {Name}");
        }
        else
        {
            Console.WriteLine($"⚠️  Наблюдатель {observer.GetName()} не найден в списке подписчиков");
        }
    }

    public async Task NotifyObserversAsync(CurrencyPair currencyPair)
    {
        if (_observers.Count == 0)
            return;

        Console.WriteLine($"\n🔔 {Name} уведомляет {_observers.Count} наблюдателей об изменении {currencyPair.GetPairSymbol()}");

        var tasks = new List<Task>();
        foreach (var observer in _observers)
        {
            tasks.Add(observer.UpdateAsync(currencyPair));
        }

        await Task.WhenAll(tasks);
    }

    public async Task UpdateExchangeRate(string pair, decimal newRate)
    {
        if (string.IsNullOrEmpty(pair))
            throw new ArgumentException("Пара валют не может быть пустой");

        if (newRate <= 0)
            throw new ArgumentException("Курс обмена должен быть положительным");

        if (!_exchangeRates.ContainsKey(pair))
        {
            _exchangeRates[pair] = newRate;
            _previousRates[pair] = newRate;
            Console.WriteLine($"Добавлена новая валютная пара: {pair} = {newRate:F4}");
            return;
        }

        decimal oldRate = _exchangeRates[pair];
        
        if (oldRate == newRate)
            return;

        _previousRates[pair] = oldRate;
        _exchangeRates[pair] = newRate;

        var currencyPair = new CurrencyPair(
            pair.Split('/')[0],
            pair.Split('/')[1],
            newRate,
            oldRate
        );

        Console.WriteLine($"\n=== Изменение курса на {Name} ===");
        Console.WriteLine($"{pair}: {oldRate:F4} → {newRate:F4} " +
                         $"({(newRate > oldRate ? "↑" : "↓")} {Math.Abs(currencyPair.ChangePercent):F2}%)");

        await NotifyObserversAsync(currencyPair);
    }

    public decimal GetCurrentRate(string pair)
    {
        return _exchangeRates.ContainsKey(pair) ? _exchangeRates[pair] : 0;
    }

    public void DisplayCurrentRates()
    {
        Console.WriteLine($"\n=== Текущие курсы на {Name} ===");
        foreach (var rate in _exchangeRates)
        {
            decimal previous = _previousRates[rate.Key];
            decimal change = previous != 0 ? ((rate.Value - previous) / previous) * 100 : 0;
            string trend = change >= 0 ? "📈" : "📉";
            Console.WriteLine($"{rate.Key}: {rate.Value:F4} ({trend} {Math.Abs(change):F2}%)");
        }
    }

    public void DisplaySubscribers()
    {
        Console.WriteLine($"\n=== Подписчики {Name} ===");
        if (_observers.Count == 0)
        {
            Console.WriteLine("Нет активных подписчиков");
            return;
        }

        foreach (var observer in _observers)
        {
            Console.WriteLine($"• {observer.GetName()}");
        }
    }
}

public class MobileAppObserver : IObserver
{
    public string AppName { get; private set; }
    private List<string> _notifications;

    public MobileAppObserver(string appName)
    {
        AppName = appName;
        _notifications = new List<string>();
    }

    public string GetName() => $"Мобильное приложение '{AppName}'";

    public async Task UpdateAsync(CurrencyPair currencyPair)
    {
        await Task.Run(() =>
        {
            string notification = $"{DateTime.Now:HH:mm:ss} - {currencyPair}";
            _notifications.Add(notification);
            
            string emoji = currencyPair.ChangePercent >= 1 ? "🚀" : 
                          currencyPair.ChangePercent <= -1 ? "🔻" : "⚡";
            
            Console.WriteLine($"   📱 {AppName}: {emoji} Уведомление - {currencyPair}");
            
            if (Math.Abs(currencyPair.ChangePercent) > 2)
            {
                Console.WriteLine($"   💰 {AppName}: Значительное изменение курса!");
            }
        });
    }

    public void DisplayNotifications()
    {
        Console.WriteLine($"\n=== Уведомления {AppName} ===");
        if (_notifications.Count == 0)
        {
            Console.WriteLine("Нет уведомлений");
            return;
        }

        foreach (var notification in _notifications)
        {
            Console.WriteLine($"• {notification}");
        }
    }
}

public class EmailNotificationObserver : IObserver
{
    public string Email { get; private set; }
    private List<string> _sentEmails;

    public EmailNotificationObserver(string email)
    {
        Email = email;
        _sentEmails = new List<string>();
    }

    public string GetName() => $"Email рассылка '{Email}'";

    public async Task UpdateAsync(CurrencyPair currencyPair)
    {
        await Task.Run(() =>
        {
            string subject = Math.Abs(currencyPair.ChangePercent) > 1 ? 
                "ВАЖНО: Значительное изменение курса!" : "Обновление курса валют";
            
            string body = $"Курс {currencyPair.GetPairSymbol()} изменился: {currencyPair.PreviousRate:F4} → {currencyPair.ExchangeRate:F4}\n" +
                         $"Изменение: {currencyPair.ChangePercent:F2}%\n" +
                         $"Время: {currencyPair.LastUpdated:HH:mm:ss}";

            _sentEmails.Add($"{currencyPair.LastUpdated:yyyy-MM-dd} - {currencyPair.GetPairSymbol()}");

            Console.WriteLine($"   📧 Email отправлен на {Email}: {subject}");
            Console.WriteLine($"   💌 Текст: {currencyPair.GetPairSymbol()} = {currencyPair.ExchangeRate:F4} ({currencyPair.ChangePercent:+#.##;-#.##;0}%)");
        });
    }

    public void DisplaySentEmails()
    {
        Console.WriteLine($"\n=== Отправленные emails на {Email} ===");
        foreach (var email in _sentEmails)
        {
            Console.WriteLine($"• {email}");
        }
    }
}

public class TradingBotObserver : IObserver
{
    public string BotName { get; private set; }
    private Dictionary<string, (decimal buyThreshold, decimal sellThreshold)> _tradingRules;
    private List<string> _tradingActions;

    public TradingBotObserver(string botName)
    {
        BotName = botName;
        _tradingRules = new Dictionary<string, (decimal, decimal)>();
        _tradingActions = new List<string>();
        
        InitializeDefaultRules();
    }

    private void InitializeDefaultRules()
    {
        _tradingRules["USD/RUB"] = (90.00m, 95.00m);
        _tradingRules["EUR/RUB"] = (98.00m, 102.00m);
        _tradingRules["USD/EUR"] = (0.90m, 0.96m);
    }

    public string GetName() => $"Торговый робот '{BotName}'";

    public void SetTradingRule(string pair, decimal buyThreshold, decimal sellThreshold)
    {
        _tradingRules[pair] = (buyThreshold, sellThreshold);
    }

    public async Task UpdateAsync(CurrencyPair currencyPair)
    {
        await Task.Run(() =>
        {
            string pair = currencyPair.GetPairSymbol();
            
            if (!_tradingRules.ContainsKey(pair))
                return;

            var (buyThreshold, sellThreshold) = _tradingRules[pair];
            string action = "";

            if (currencyPair.ExchangeRate <= buyThreshold)
            {
                action = $"ПОКУПКА {pair} по {currencyPair.ExchangeRate:F4} (ниже порога {buyThreshold:F4})";
                Console.WriteLine($"   🤖 {BotName}: 🟢 {action}");
            }
            else if (currencyPair.ExchangeRate >= sellThreshold)
            {
                action = $"ПРОДАЖА {pair} по {currencyPair.ExchangeRate:F4} (выше порога {sellThreshold:F4})";
                Console.WriteLine($"   🤖 {BotName}: 🔴 {action}");
            }
            else
            {
                action = $"НАБЛЮДЕНИЕ {pair} = {currencyPair.ExchangeRate:F4}";
                Console.WriteLine($"   🤖 {BotName}: ⚪ {action}");
            }

            _tradingActions.Add($"{DateTime.Now:HH:mm:ss} - {action}");
        });
    }

    public void DisplayTradingActions()
    {
        Console.WriteLine($"\n=== Действия робота {BotName} ===");
        foreach (var action in _tradingActions)
        {
            Console.WriteLine($"• {action}");
        }
    }
}

public class AnalyticsDisplayObserver : IObserver
{
    public string DisplayName { get; private set; }
    private List<CurrencyPair> _history;

    public AnalyticsDisplayObserver(string displayName)
    {
        DisplayName = displayName;
        _history = new List<CurrencyPair>();
    }

    public string GetName() => $"Аналитический дисплей '{DisplayName}'";

    public async Task UpdateAsync(CurrencyPair currencyPair)
    {
        await Task.Run(() =>
        {
            _history.Add(currencyPair);
            
            string trend = currencyPair.ChangePercent >= 0 ? "рост" : "падение";
            string color = Math.Abs(currencyPair.ChangePercent) > 1 ? "🔴" : "🟡";
            
            if (Math.Abs(currencyPair.ChangePercent) > 2)
                color = "🚨";

            Console.WriteLine($"   📊 {DisplayName}: {color} {currencyPair.GetPairSymbol()} - {trend} на {Math.Abs(currencyPair.ChangePercent):F2}%");
            Console.WriteLine($"   📈 {DisplayName}: График обновлен для {currencyPair.GetPairSymbol()}");

            if (_history.Count >= 2)
            {
                var lastTwo = _history.GetRange(_history.Count - 2, 2);
                decimal acceleration = lastTwo[1].ChangePercent - lastTwo[0].ChangePercent;
                
                if (Math.Abs(acceleration) > 0.5m)
                {
                    Console.WriteLine($"   ⚡ {DisplayName}: Ускорение тренда {acceleration:+#.##;-#.##;0}%");
                }
            }
        });
    }

    public void DisplayAnalysis()
    {
        Console.WriteLine($"\n=== Аналитика {DisplayName} ===");
        if (_history.Count == 0)
        {
            Console.WriteLine("Нет данных для анализа");
            return;
        }

        var lastUpdate = _history[^1];
        Console.WriteLine($"Последнее обновление: {lastUpdate.GetPairSymbol()} = {lastUpdate.ExchangeRate:F4}");
        Console.WriteLine($"Всего записей: {_history.Count}");
    }
}

class CurrencyObserverProgram
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Система мониторинга курсов валют ===\n");

        CurrencyExchange exchange = new CurrencyExchange("Московская Биржа");

        MobileAppObserver mobileApp = new MobileAppObserver("МойТрейдер");
        EmailNotificationObserver emailNotifier = new EmailNotificationObserver("trader@example.com");
        TradingBotObserver tradingBot = new TradingBotObserver("AlphaTrader");
        AnalyticsDisplayObserver analyticsDisplay = new AnalyticsDisplayObserver("Торговый терминал");

        exchange.RegisterObserver(mobileApp);
        exchange.RegisterObserver(emailNotifier);
        exchange.RegisterObserver(tradingBot);

        exchange.DisplayCurrentRates();
        exchange.DisplaySubscribers();

        Console.WriteLine("\n=== Начало обновления курсов ===");

        Random random = new Random();
        
        for (int i = 0; i < 8; i++)
        {
            string[] pairs = { "USD/RUB", "EUR/RUB", "USD/EUR", "GBP/RUB", "CNY/RUB" };
            string randomPair = pairs[random.Next(pairs.Length)];
            
            decimal currentRate = exchange.GetCurrentRate(randomPair);
            decimal change = (decimal)(random.NextDouble() - 0.5) * 2;
            decimal newRate = Math.Max(0.1m, currentRate + change);

            await exchange.UpdateExchangeRate(randomPair, newRate);
            
            await Task.Delay(1500);
        }

        Console.WriteLine("\n=== Добавление нового наблюдателя ===");
        exchange.RegisterObserver(analyticsDisplay);

        Console.WriteLine("\n=== Удаление email наблюдателя ===");
        exchange.RemoveObserver(emailNotifier);

        await exchange.UpdateExchangeRate("USD/RUB", 94.80m);
        await exchange.UpdateExchangeRate("EUR/RUB", 101.20m);

        Console.WriteLine("\n=== Финальная статистика ===");
        exchange.DisplayCurrentRates();
        exchange.DisplaySubscribers();

        mobileApp.DisplayNotifications();
        emailNotifier.DisplaySentEmails();
        tradingBot.DisplayTradingActions();
        analyticsDisplay.DisplayAnalysis();

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
