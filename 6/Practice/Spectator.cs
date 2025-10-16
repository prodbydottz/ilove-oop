using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class StockPrice
{
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public decimal Change { get; set; }
    public decimal ChangePercent { get; set; }
    public DateTime Timestamp { get; set; }

    public StockPrice(string symbol, decimal price, decimal previousPrice)
    {
        Symbol = symbol;
        Price = price;
        Change = price - previousPrice;
        ChangePercent = previousPrice != 0 ? (Change / previousPrice) * 100 : 0;
        Timestamp = DateTime.Now;
    }
}

public interface IObserver
{
    Task UpdateAsync(StockPrice stockPrice);
    string GetName();
    List<string> GetSubscribedStocks();
}

public interface ISubject
{
    void RegisterObserver(IObserver observer, string stockSymbol);
    void RemoveObserver(IObserver observer, string stockSymbol);
    void RemoveObserver(IObserver observer);
    Task NotifyObserversAsync(StockPrice stockPrice);
    List<IObserver> GetObserversForStock(string stockSymbol);
}

public class StockExchange : ISubject
{
    private Dictionary<string, List<IObserver>> _observers;
    private Dictionary<string, decimal> _stockPrices;
    private readonly object _lockObject = new object();

    public string Name { get; private set; }

    public StockExchange(string name)
    {
        Name = name;
        _observers = new Dictionary<string, List<IObserver>>();
        _stockPrices = new Dictionary<string, decimal>();
        
        InitializeDefaultStocks();
    }

    private void InitializeDefaultStocks()
    {
        _stockPrices["AAPL"] = 150.00m;
        _stockPrices["GOOGL"] = 2800.00m;
        _stockPrices["MSFT"] = 330.00m;
        _stockPrices["TSLA"] = 250.00m;
        _stockPrices["AMZN"] = 3400.00m;
    }

    public void RegisterObserver(IObserver observer, string stockSymbol)
    {
        lock (_lockObject)
        {
            if (!_observers.ContainsKey(stockSymbol))
            {
                _observers[stockSymbol] = new List<IObserver>();
            }

            if (!_observers[stockSymbol].Contains(observer))
            {
                _observers[stockSymbol].Add(observer);
                Console.WriteLine($"[{Name}] {observer.GetName()} подписан на {stockSymbol}");
            }
        }
    }

    public void RemoveObserver(IObserver observer, string stockSymbol)
    {
        lock (_lockObject)
        {
            if (_observers.ContainsKey(stockSymbol))
            {
                _observers[stockSymbol].Remove(observer);
                Console.WriteLine($"[{Name}] {observer.GetName()} отписан от {stockSymbol}");
                
                if (_observers[stockSymbol].Count == 0)
                {
                    _observers.Remove(stockSymbol);
                }
            }
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        lock (_lockObject)
        {
            foreach (var stock in _observers.Keys.ToList())
            {
                _observers[stock].Remove(observer);
                
                if (_observers[stock].Count == 0)
                {
                    _observers.Remove(stock);
                }
            }
            Console.WriteLine($"[{Name}] {observer.GetName()} полностью отписан от всех акций");
        }
    }

    public async Task NotifyObserversAsync(StockPrice stockPrice)
    {
        List<IObserver> observersToNotify;

        lock (_lockObject)
        {
            if (!_observers.ContainsKey(stockPrice.Symbol))
                return;

            observersToNotify = new List<IObserver>(_observers[stockPrice.Symbol]);
        }

        var tasks = observersToNotify.Select(observer => 
            observer.UpdateAsync(stockPrice));
        
        await Task.WhenAll(tasks);
    }

    public List<IObserver> GetObserversForStock(string stockSymbol)
    {
        lock (_lockObject)
        {
            return _observers.ContainsKey(stockSymbol) 
                ? new List<IObserver>(_observers[stockSymbol]) 
                : new List<IObserver>();
        }
    }

    public async Task UpdateStockPrice(string symbol, decimal newPrice)
    {
        if (!_stockPrices.ContainsKey(symbol))
        {
            _stockPrices[symbol] = newPrice;
            return;
        }

        decimal oldPrice = _stockPrices[symbol];
        
        if (oldPrice == newPrice)
            return;

        _stockPrices[symbol] = newPrice;
        var stockPrice = new StockPrice(symbol, newPrice, oldPrice);

        Console.WriteLine($"\n[{Name}] 📈 {symbol}: {oldPrice:C} → {newPrice:C} " +
                         $"({stockPrice.Change:+0.00;-0.00;0} [{stockPrice.ChangePercent:+0.00;-0.00;0}%])");

        await NotifyObserversAsync(stockPrice);
    }

    public decimal GetCurrentPrice(string symbol)
    {
        return _stockPrices.ContainsKey(symbol) ? _stockPrices[symbol] : 0;
    }

    public void DisplayMarketStatus()
    {
        Console.WriteLine($"\n=== {Name} - Рыночная ситуация ===");
        foreach (var stock in _stockPrices)
        {
            Console.WriteLine($"{stock.Key}: {stock.Value:C}");
        }
        
        Console.WriteLine($"\nПодписки:");
        foreach (var stock in _observers)
        {
            Console.WriteLine($"{stock.Key}: {stock.Value.Count} подписчиков");
        }
    }
}

public class Trader : IObserver
{
    public string Name { get; private set; }
    private decimal _budget;
    private Dictionary<string, int> _portfolio;
    private List<string> _subscribedStocks;

    public Trader(string name, decimal initialBudget)
    {
        Name = name;
        _budget = initialBudget;
        _portfolio = new Dictionary<string, int>();
        _subscribedStocks = new List<string>();
    }

    public string GetName() => Name;

    public List<string> GetSubscribedStocks() => new List<string>(_subscribedStocks);

    public async Task UpdateAsync(StockPrice stockPrice)
    {
        await Task.Run(() =>
        {
            Console.WriteLine($"[Трейдер {Name}] Получено обновление: {stockPrice.Symbol} " +
                            $"{stockPrice.Price:C} ({stockPrice.ChangePercent:+0.00;-0.00;0}%)");
            
            if (stockPrice.ChangePercent < -5.0m)
            {
                Console.WriteLine($"   ⚠️  {Name}: Сильное падение {stockPrice.Symbol}! Возможность покупки?");
            }
            else if (stockPrice.ChangePercent > 3.0m)
            {
                Console.WriteLine($"   📈 {Name}: Рост {stockPrice.Symbol}! Возможность продажи?");
            }
        });
    }

    public void SubscribeToStock(StockExchange exchange, string symbol)
    {
        exchange.RegisterObserver(this, symbol);
        if (!_subscribedStocks.Contains(symbol))
        {
            _subscribedStocks.Add(symbol);
        }
    }

    public void UnsubscribeFromStock(StockExchange exchange, string symbol)
    {
        exchange.RemoveObserver(this, symbol);
        _subscribedStocks.Remove(symbol);
    }

    public void BuyStock(string symbol, int quantity, decimal price)
    {
        decimal totalCost = quantity * price;
        if (_budget >= totalCost)
        {
            _budget -= totalCost;
            _portfolio[symbol] = _portfolio.ContainsKey(symbol) 
                ? _portfolio[symbol] + quantity 
                : quantity;
            
            Console.WriteLine($"[{Name}] Куплено {quantity} акций {symbol} по {price:C}. " +
                            $"Остаток бюджета: {_budget:C}");
        }
        else
        {
            Console.WriteLine($"[{Name}] ❌ Недостаточно средств для покупки {quantity} акций {symbol}");
        }
    }

    public void DisplayPortfolio()
    {
        Console.WriteLine($"\n=== Портфель {Name} ===");
        Console.WriteLine($"Бюджет: {_budget:C}");
        Console.WriteLine("Акции:");
        foreach (var holding in _portfolio)
        {
            Console.WriteLine($"  {holding.Key}: {holding.Value} акций");
        }
    }
}

public class TradingRobot : IObserver
{
    public string Name { get; private set; }
    private Dictionary<string, (decimal buyThreshold, decimal sellThreshold)> _tradingRules;
    private List<string> _subscribedStocks;

    public TradingRobot(string name)
    {
        Name = name;
        _tradingRules = new Dictionary<string, (decimal, decimal)>();
        _subscribedStocks = new List<string>();
    }

    public string GetName() => $"Робот-{Name}";

    public List<string> GetSubscribedStocks() => new List<string>(_subscribedStocks);

    public void SetTradingRule(string symbol, decimal buyThreshold, decimal sellThreshold)
    {
        _tradingRules[symbol] = (buyThreshold, sellThreshold);
    }

    public async Task UpdateAsync(StockPrice stockPrice)
    {
        await Task.Run(() =>
        {
            if (!_tradingRules.ContainsKey(stockPrice.Symbol))
                return;

            var (buyThreshold, sellThreshold) = _tradingRules[stockPrice.Symbol];

            if (stockPrice.Price <= buyThreshold)
            {
                Console.WriteLine($"   🤖 {Name}: Цена {stockPrice.Symbol} упала до {stockPrice.Price:C} " +
                                $"(ниже порога {buyThreshold:C}) - ПОКУПКА!");
            }
            else if (stockPrice.Price >= sellThreshold)
            {
                Console.WriteLine($"   🤖 {Name}: Цена {stockPrice.Symbol} выросла до {stockPrice.Price:C} " +
                                $"(выше порога {sellThreshold:C}) - ПРОДАЖА!");
            }
        });
    }

    public void SubscribeToStock(StockExchange exchange, string symbol)
    {
        exchange.RegisterObserver(this, symbol);
        if (!_subscribedStocks.Contains(symbol))
        {
            _subscribedStocks.Add(symbol);
        }
    }
}

public class EmailNotifier : IObserver
{
    public string Email { get; private set; }
    private List<string> _subscribedStocks;

    public EmailNotifier(string email)
    {
        Email = email;
        _subscribedStocks = new List<string>();
    }

    public string GetName() => $"Email-{Email}";

    public List<string> GetSubscribedStocks() => new List<string>(_subscribedStocks);

    public async Task UpdateAsync(StockPrice stockPrice)
    {
        await Task.Run(() =>
        {
            string trend = stockPrice.Change >= 0 ? "📈 рост" : "📉 падение";
            Console.WriteLine($"   📧 Email отправлен на {Email}: {stockPrice.Symbol} - " +
                            $"{stockPrice.Price:C} ({trend} {Math.Abs(stockPrice.ChangePercent):0.00}%)");
        });
    }

    public void SubscribeToStock(StockExchange exchange, string symbol)
    {
        exchange.RegisterObserver(this, symbol);
        if (!_subscribedStocks.Contains(symbol))
        {
            _subscribedStocks.Add(symbol);
        }
    }
}

class StockExchangeProgram
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Система управления биржевыми торгами ===\n");

        StockExchange nasdaq = new StockExchange("NASDAQ");

        Trader trader1 = new Trader("Иван Петров", 10000m);
        Trader trader2 = new Trader("Мария Сидорова", 15000m);
        
        TradingRobot robot1 = new TradingRobot("AlphaTrader");
        TradingRobot robot2 = new TradingRobot("MegaInvest");
        
        EmailNotifier emailNotifier = new EmailNotifier("alerts@trader.com");

        trader1.SubscribeToStock(nasdaq, "AAPL");
        trader1.SubscribeToStock(nasdaq, "TSLA");
        
        trader2.SubscribeToStock(nasdaq, "GOOGL");
        trader2.SubscribeToStock(nasdaq, "AMZN");
        
        robot1.SubscribeToStock(nasdaq, "AAPL");
        robot1.SetTradingRule("AAPL", 145.00m, 160.00m);
        
        robot2.SubscribeToStock(nasdaq, "TSLA");
        robot2.SetTradingRule("TSLA", 240.00m, 270.00m);
        
        emailNotifier.SubscribeToStock(nasdaq, "MSFT");
        emailNotifier.SubscribeToStock(nasdaq, "AMZN");

        nasdaq.DisplayMarketStatus();

        Console.WriteLine("\n=== Начало торгов ===");
        
        Random random = new Random();
        
        for (int i = 0; i < 10; i++)
        {
            string[] stocks = { "AAPL", "GOOGL", "MSFT", "TSLA", "AMZN" };
            string randomStock = stocks[random.Next(stocks.Length)];
            
            decimal currentPrice = nasdaq.GetCurrentPrice(randomStock);
            decimal change = (decimal)(random.NextDouble() - 0.5) * 10;
            decimal newPrice = Math.Max(1, currentPrice + change);

            await nasdaq.UpdateStockPrice(randomStock, newPrice);
            
            await Task.Delay(2000);
        }

        Console.WriteLine("\n=== Итоги торгов ===");
        nasdaq.DisplayMarketStatus();
        
        trader1.DisplayPortfolio();
        trader2.DisplayPortfolio();

        Console.WriteLine("\nОтписываем трейдера 1 от TSLA...");
        trader1.UnsubscribeFromStock(nasdaq, "TSLA");

        await nasdaq.UpdateStockPrice("TSLA", 260.00m);

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
