using System;
using System.Collections.Generic;

// Существующий интерфейс платежной системы
public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    string GetProcessorName();
}

// Существующий класс PayPal
public class PayPalPaymentProcessor : IPaymentProcessor
{
    private string _accountEmail;

    public PayPalPaymentProcessor(string accountEmail)
    {
        _accountEmail = accountEmail;
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка платежа через PayPal: {amount:C}");
        Console.WriteLine($"Аккаунт: {_accountEmail}");
        
        // Симуляция обработки платежа
        bool isSuccessful = amount > 0 && amount <= 10000m;
        
        if (isSuccessful)
        {
            Console.WriteLine("PayPal: Платеж успешно обработан");
        }
        else
        {
            Console.WriteLine("PayPal: Ошибка обработки платежа");
        }
        
        return isSuccessful;
    }

    public string GetProcessorName()
    {
        return "PayPal";
    }
}

// Сторонний сервис Stripe с другим интерфейсом
public class StripePaymentService
{
    private string _apiKey;
    private string _merchantId;

    public StripePaymentService(string apiKey, string merchantId)
    {
        _apiKey = apiKey;
        _merchantId = merchantId;
    }

    public bool MakeTransaction(decimal totalAmount)
    {
        Console.WriteLine($"Stripe: Создание транзакции на сумму {totalAmount:C}");
        Console.WriteLine($"Merchant ID: {_merchantId}");
        
        // Симуляция логики Stripe
        bool transactionSuccess = totalAmount > 0 && totalAmount <= 5000m;
        
        if (transactionSuccess)
        {
            Console.WriteLine("Stripe: Транзакция успешно завершена");
        }
        else
        {
            Console.WriteLine("Stripe: Транзакция отклонена");
        }
        
        return transactionSuccess;
    }

    public string GetServiceInfo()
    {
        return "Stripe Payment Service";
    }
}

// Сторонний сервис Square с другим интерфейсом
public class SquarePaymentService
{
    private string _locationId;
    private string _accessToken;

    public SquarePaymentService(string locationId, string accessToken)
    {
        _locationId = locationId;
        _accessToken = accessToken;
    }

    public bool CreatePayment(decimal amount, string currency = "USD")
    {
        Console.WriteLine($"Square: Создание платежа {amount:C} ({currency})");
        Console.WriteLine($"Location: {_locationId}");
        
        // Симуляция логики Square
        bool paymentCreated = amount > 0 && amount <= 3000m;
        
        if (paymentCreated)
        {
            Console.WriteLine("Square: Платеж создан успешно");
        }
        else
        {
            Console.WriteLine("Square: Не удалось создать платеж");
        }
        
        return paymentCreated;
    }

    public string GetServiceDetails()
    {
        return "Square Payment Platform";
    }
}

// Адаптер для Stripe
public class StripePaymentAdapter : IPaymentProcessor
{
    private StripePaymentService _stripeService;

    public StripePaymentAdapter(StripePaymentService stripeService)
    {
        _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService));
    }

    public bool ProcessPayment(decimal amount)
    {
        // Адаптация вызова: ProcessPayment -> MakeTransaction
        return _stripeService.MakeTransaction(amount);
    }

    public string GetProcessorName()
    {
        return _stripeService.GetServiceInfo();
    }
}

// Адаптер для Square
public class SquarePaymentAdapter : IPaymentProcessor
{
    private SquarePaymentService _squareService;

    public SquarePaymentAdapter(SquarePaymentService squareService)
    {
        _squareService = squareService ?? throw new ArgumentNullException(nameof(squareService));
    }

    public bool ProcessPayment(decimal amount)
    {
        // Адаптация вызова: ProcessPayment -> CreatePayment
        return _squareService.CreatePayment(amount, "USD");
    }

    public string GetProcessorName()
    {
        return _squareService.GetServiceDetails();
    }
}

// Новый сторонний сервис CryptoPay с уникальным интерфейсом
public class CryptoPaymentService
{
    private string _walletAddress;
    private string _cryptoType;

    public CryptoPaymentService(string walletAddress, string cryptoType = "BTC")
    {
        _walletAddress = walletAddress;
        _cryptoType = cryptoType;
    }

    public bool ExecuteCryptoTransfer(decimal fiatAmount, string currency)
    {
        Console.WriteLine($"CryptoPay: Перевод {fiatAmount:C} ({currency}) в {_cryptoType}");
        Console.WriteLine($"Кошелек: {_walletAddress}");
        
        // Симуляция крипто-перевода
        decimal cryptoAmount = fiatAmount / 50000m; // Примерный курс
        bool transferSuccess = fiatAmount > 0 && fiatAmount <= 1000m;
        
        if (transferSuccess)
        {
            Console.WriteLine($"CryptoPay: Успешно переведено {cryptoAmount:F6} {_cryptoType}");
        }
        else
        {
            Console.WriteLine("CryptoPay: Ошибка крипто-перевода");
        }
        
        return transferSuccess;
    }

    public string GetCryptoServiceName()
    {
        return $"CryptoPay ({_cryptoType})";
    }
}

// Адаптер для CryptoPay
public class CryptoPaymentAdapter : IPaymentProcessor
{
    private CryptoPaymentService _cryptoService;

    public CryptoPaymentAdapter(CryptoPaymentService cryptoService)
    {
        _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
    }

    public bool ProcessPayment(decimal amount)
    {
        // Адаптация вызова: ProcessPayment -> ExecuteCryptoTransfer
        return _cryptoService.ExecuteCryptoTransfer(amount, "USD");
    }

    public string GetProcessorName()
    {
        return _cryptoService.GetCryptoServiceName();
    }
}

// Фабрика для создания платежных процессоров
public class PaymentProcessorFactory
{
    public IPaymentProcessor CreatePayPal(string email)
    {
        return new PayPalPaymentProcessor(email);
    }

    public IPaymentProcessor CreateStripe(string apiKey, string merchantId)
    {
        var stripeService = new StripePaymentService(apiKey, merchantId);
        return new StripePaymentAdapter(stripeService);
    }

    public IPaymentProcessor CreateSquare(string locationId, string accessToken)
    {
        var squareService = new SquarePaymentService(locationId, accessToken);
        return new SquarePaymentAdapter(squareService);
    }

    public IPaymentProcessor CreateCrypto(string walletAddress, string cryptoType = "BTC")
    {
        var cryptoService = new CryptoPaymentService(walletAddress, cryptoType);
        return new CryptoPaymentAdapter(cryptoService);
    }
}

// Система интернет-магазина
public class OnlineStore
{
    private List<IPaymentProcessor> _availableProcessors;
    private PaymentProcessorFactory _factory;

    public OnlineStore()
    {
        _factory = new PaymentProcessorFactory();
        _availableProcessors = new List<IPaymentProcessor>();
        
        // Инициализация доступных платежных систем
        InitializeProcessors();
    }

    private void InitializeProcessors()
    {
        _availableProcessors.Add(_factory.CreatePayPal("emir.moldakhulov@email.com"));
        
        _availableProcessors.Add(_factory.CreateStripe("sk_test_123456", "merchant_ali"));
        
        _availableProcessors.Add(_factory.CreateSquare("loc_amir_789", "sq0atp-token_amir"));
        
        _availableProcessors.Add(_factory.CreateCrypto("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa", "BTC"));
        
        _availableProcessors.Add(_factory.CreatePayPal("kazimir.k@email.com"));
        
        _availableProcessors.Add(_factory.CreateStripe("sk_test_654321", "merchant_snow"));
        
        _availableProcessors.Add(_factory.CreateCrypto("0x742d35Cc6634C0532925a3b8D", "ETH"));
    }

    public void ProcessOrder(string customerName, decimal amount, int processorIndex)
    {
        if (processorIndex < 0 || processorIndex >= _availableProcessors.Count)
        {
            Console.WriteLine($"Ошибка: неверный индекс процессора {processorIndex}");
            return;
        }

        var processor = _availableProcessors[processorIndex];
        
        Console.WriteLine($"\n=== ОБРАБОТКА ЗАКАЗА ДЛЯ {customerName} ===");
        Console.WriteLine($"Сумма: {amount:C}");
        Console.WriteLine($"Платежная система: {processor.GetProcessorName()}");

        bool success = processor.ProcessPayment(amount);
        
        if (success)
        {
            Console.WriteLine($"✅ Заказ для {customerName} успешно обработан!");
        }
        else
        {
            Console.WriteLine($"❌ Ошибка обработки заказа для {customerName}");
        }
    }

    public void DisplayAvailableProcessors()
    {
        Console.WriteLine("\n=== ДОСТУПНЫЕ ПЛАТЕЖНЫЕ СИСТЕМЫ ===");
        for (int i = 0; i < _availableProcessors.Count; i++)
        {
            Console.WriteLine($"{i}. {_availableProcessors[i].GetProcessorName()}");
        }
    }
}

// Клиентский код
class PaymentSystemDemo
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА ИНТЕГРАЦИИ ПЛАТЕЖНЫХ СЕРВИСОВ С ПАТТЕРНОМ АДАПТЕР ===\n");

        OnlineStore store = new OnlineStore();
        
        // Отображаем доступные платежные системы
        store.DisplayAvailableProcessors();

        // Обработка заказов через разные платежные системы
        Console.WriteLine("\n=== ОБРАБОТКА ЗАКАЗОВ ===");
        
        store.ProcessOrder("Молдахулов Эмир", 2500m, 0);  // PayPal
        store.ProcessOrder("Кожабек Али", 1500m, 1);      // Stripe
        store.ProcessOrder("Байжан Амир", 800m, 2);       // Square
        store.ProcessOrder("Изатов Диас", 500m, 3);       // Crypto BTC
        store.ProcessOrder("Казимир Казимирович", 3000m, 4); // PayPal
        store.ProcessOrder("Дмитрий Снег", 4500m, 5);     // Stripe
        store.ProcessOrder("Дмитрий Довгешко", 750m, 6);  // Crypto ETH

        // Тестирование граничных случаев
        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ГРАНИЧНЫХ СЛУЧАЕВ ===");
        
        store.ProcessOrder("Тест 1", 0m, 1);           // Нулевая сумма
        store.ProcessOrder("Тест 2", 10000m, 0);       // Максимальная для PayPal
        store.ProcessOrder("Тест 3", 6000m, 1);        // Превышение лимита Stripe
        store.ProcessOrder("Тест 4", 3500m, 2);        // Превышение лимита Square
        store.ProcessOrder("Тест 5", 1500m, 3);        // Превышение лимита Crypto

        // Тестирование неверного индекса
        Console.WriteLine("\n=== ТЕСТИРОВАНИЕ ОШИБОК ===");
        store.ProcessOrder("Тест ошибки", 100m, 10);   // Неверный индекс

        Console.WriteLine("\nДемонстрация завершена. Все платежные системы работают через единый интерфейс!");
    }
}
