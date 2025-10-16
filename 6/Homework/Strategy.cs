using System;
using System.Collections.Generic;

public interface IPaymentStrategy
{
    bool ProcessPayment(decimal amount);
    string GetPaymentDetails();
}

public class CreditCardPaymentStrategy : IPaymentStrategy
{
    private string _cardNumber;
    private string _cardHolder;
    private string _cvv;
    private string _expiryDate;

    public CreditCardPaymentStrategy(string cardNumber, string cardHolder, string cvv, string expiryDate)
    {
        if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length != 16)
            throw new ArgumentException("Номер карты должен содержать 16 цифр");
        
        if (string.IsNullOrEmpty(cardHolder))
            throw new ArgumentException("Имя владельца карты обязательно");
        
        if (string.IsNullOrEmpty(cvv) || cvv.Length != 3)
            throw new ArgumentException("CVV код должен содержать 3 цифры");
        
        if (string.IsNullOrEmpty(expiryDate))
            throw new ArgumentException("Срок действия карты обязателен");

        _cardNumber = cardNumber;
        _cardHolder = cardHolder;
        _cvv = cvv;
        _expiryDate = expiryDate;
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка платежа через банковскую карту на сумму {amount:C}");
        
        if (amount <= 0)
        {
            Console.WriteLine("❌ Ошибка: Сумма платежа должна быть положительной");
            return false;
        }

        if (amount > 10000)
        {
            Console.WriteLine("❌ Ошибка: Превышен лимит платежа по карте");
            return false;
        }

        Console.WriteLine($"✅ Платеж на сумму {amount:C} успешно обработан через карту {MaskCardNumber(_cardNumber)}");
        return true;
    }

    public string GetPaymentDetails()
    {
        return $"Банковская карта: {MaskCardNumber(_cardNumber)}, держатель: {_cardHolder}";
    }

    private string MaskCardNumber(string cardNumber)
    {
        return $"****-****-****-{cardNumber.Substring(12)}";
    }
}

public class PayPalPaymentStrategy : IPaymentStrategy
{
    private string _email;
    private string _password;

    public PayPalPaymentStrategy(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            throw new ArgumentException("Некорректный email адрес");
        
        if (string.IsNullOrEmpty(password) || password.Length < 6)
            throw new ArgumentException("Пароль должен содержать минимум 6 символов");

        _email = email;
        _password = password;
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка платежа через PayPal на сумму {amount:C}");
        
        if (amount <= 0)
        {
            Console.WriteLine("❌ Ошибка: Сумма платежа должна быть положительной");
            return false;
        }

        Console.WriteLine($"🔐 Аутентификация пользователя {_email}...");
        
        if (_email.Contains("test"))
        {
            Console.WriteLine("❌ Ошибка аутентификации PayPal");
            return false;
        }

        Console.WriteLine($"✅ Платеж на сумму {amount:C} успешно обработан через PayPal аккаунт {_email}");
        return true;
    }

    public string GetPaymentDetails()
    {
        return $"PayPal аккаунт: {_email}";
    }
}

public class CryptoPaymentStrategy : IPaymentStrategy
{
    private string _walletAddress;
    private string _cryptoType;

    public CryptoPaymentStrategy(string walletAddress, string cryptoType = "BTC")
    {
        if (string.IsNullOrEmpty(walletAddress) || walletAddress.Length < 26)
            throw new ArgumentException("Некорректный адрес криптокошелька");
        
        if (string.IsNullOrEmpty(cryptoType))
            throw new ArgumentException("Тип криптовалюты обязателен");

        _walletAddress = walletAddress;
        _cryptoType = cryptoType.ToUpper();
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка платежа через {_cryptoType} на сумму {amount:C}");
        
        if (amount <= 0)
        {
            Console.WriteLine("❌ Ошибка: Сумма платежа должна быть положительной");
            return false;
        }

        if (amount < 0.01m)
        {
            Console.WriteLine("❌ Ошибка: Сумма платежа слишком мала для криптовалюты");
            return false;
        }

        Console.WriteLine($"🔗 Подтверждение транзакции в блокчейне...");
        Console.WriteLine($"📧 Отправка {amount:C} на адрес {MaskWalletAddress(_walletAddress)}");
        
        Console.WriteLine($"✅ Криптоплатеж на сумму {amount:C} успешно обработан через {_cryptoType}");
        return true;
    }

    public string GetPaymentDetails()
    {
        return $"Криптокошелек {_cryptoType}: {MaskWalletAddress(_walletAddress)}";
    }

    private string MaskWalletAddress(string address)
    {
        if (address.Length <= 8)
            return address;
        
        return $"{address.Substring(0, 4)}...{address.Substring(address.Length - 4)}";
    }
}

public class BankTransferPaymentStrategy : IPaymentStrategy
{
    private string _accountNumber;
    private string _bankName;
    private string _recipientName;

    public BankTransferPaymentStrategy(string accountNumber, string bankName, string recipientName)
    {
        if (string.IsNullOrEmpty(accountNumber) || accountNumber.Length < 5)
            throw new ArgumentException("Некорректный номер счета");
        
        if (string.IsNullOrEmpty(bankName))
            throw new ArgumentException("Название банка обязательно");
        
        if (string.IsNullOrEmpty(recipientName))
            throw new ArgumentException("Имя получателя обязательно");

        _accountNumber = accountNumber;
        _bankName = bankName;
        _recipientName = recipientName;
    }

    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Обработка банковского перевода на сумму {amount:C}");
        
        if (amount <= 0)
        {
            Console.WriteLine("❌ Ошибка: Сумма перевода должна быть положительной");
            return false;
        }

        if (amount > 50000)
        {
            Console.WriteLine("⚠️  Требуется дополнительная верификация для крупного перевода");
        }

        Console.WriteLine($"🏦 Перевод {amount:C} в банк {_bankName} на счет {MaskAccountNumber(_accountNumber)}");
        Console.WriteLine($"✅ Банковский перевод на сумму {amount:C} успешно обработан");
        return true;
    }

    public string GetPaymentDetails()
    {
        return $"Банковский перевод: {_bankName}, счет: {MaskAccountNumber(_accountNumber)}, получатель: {_recipientName}";
    }

    private string MaskAccountNumber(string accountNumber)
    {
        if (accountNumber.Length <= 4)
            return accountNumber;
        
        return $"**{accountNumber.Substring(accountNumber.Length - 4)}";
    }
}

public class PaymentContext
{
    private IPaymentStrategy _paymentStrategy;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy), "Стратегия оплаты не может быть null");
    }

    public bool ExecutePayment(decimal amount)
    {
        if (_paymentStrategy == null)
        {
            throw new InvalidOperationException("Стратегия оплаты не установлена. Сначала выберите способ оплаты.");
        }

        if (amount <= 0)
        {
            throw new ArgumentException("Сумма платежа должна быть положительной", nameof(amount));
        }

        Console.WriteLine($"\n=== Начало обработки платежа ===");
        Console.WriteLine($"Способ оплаты: {_paymentStrategy.GetPaymentDetails()}");
        Console.WriteLine($"Сумма: {amount:C}");

        bool result = _paymentStrategy.ProcessPayment(amount);

        Console.WriteLine(result ? "🎉 Платеж завершен успешно!" : "💥 Платеж не прошел");
        Console.WriteLine("=== Конец обработки платежа ===\n");

        return result;
    }

    public string GetCurrentPaymentMethod()
    {
        return _paymentStrategy?.GetPaymentDetails() ?? "Способ оплаты не выбран";
    }
}

class PaymentSystemProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система обработки платежей ===");
        
        PaymentContext paymentContext = new PaymentContext();
        bool continueProcessing = true;

        while (continueProcessing)
        {
            try
            {
                Console.WriteLine("\nВыберите способ оплаты:");
                Console.WriteLine("1 - Банковская карта");
                Console.WriteLine("2 - PayPal");
                Console.WriteLine("3 - Криптовалюта");
                Console.WriteLine("4 - Банковский перевод");
                Console.WriteLine("0 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    continueProcessing = false;
                    continue;
                }

                IPaymentStrategy strategy = null;

                switch (choice)
                {
                    case "1":
                        strategy = CreateCreditCardStrategy();
                        break;
                    case "2":
                        strategy = CreatePayPalStrategy();
                        break;
                    case "3":
                        strategy = CreateCryptoStrategy();
                        break;
                    case "4":
                        strategy = CreateBankTransferStrategy();
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        continue;
                }

                if (strategy != null)
                {
                    paymentContext.SetPaymentStrategy(strategy);
                    
                    Console.Write("Введите сумму платежа: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                    {
                        paymentContext.ExecutePayment(amount);
                    }
                    else
                    {
                        Console.WriteLine("❌ Некорректная сумма платежа");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine("Спасибо за использование системы оплаты!");
    }

    static CreditCardPaymentStrategy CreateCreditCardStrategy()
    {
        Console.WriteLine("\n--- Ввод данных банковской карты ---");
        Console.Write("Номер карты (16 цифр): ");
        string cardNumber = Console.ReadLine();
        
        Console.Write("Имя владельца карты: ");
        string cardHolder = Console.ReadLine();
        
        Console.Write("CVV код (3 цифры): ");
        string cvv = Console.ReadLine();
        
        Console.Write("Срок действия (MM/YY): ");
        string expiryDate = Console.ReadLine();

        return new CreditCardPaymentStrategy(cardNumber, cardHolder, cvv, expiryDate);
    }

    static PayPalPaymentStrategy CreatePayPalStrategy()
    {
        Console.WriteLine("\n--- Ввод данных PayPal ---");
        Console.Write("Email: ");
        string email = Console.ReadLine();
        
        Console.Write("Пароль: ");
        string password = Console.ReadLine();

        return new PayPalPaymentStrategy(email, password);
    }

    static CryptoPaymentStrategy CreateCryptoStrategy()
    {
        Console.WriteLine("\n--- Ввод данных криптокошелька ---");
        Console.Write("Адрес кошелька: ");
        string walletAddress = Console.ReadLine();
        
        Console.Write("Тип криптовалюты (BTC/ETH/USDT): ");
        string cryptoType = Console.ReadLine();

        return new CryptoPaymentStrategy(walletAddress, cryptoType);
    }

    static BankTransferPaymentStrategy CreateBankTransferStrategy()
    {
        Console.WriteLine("\n--- Ввод данных банковского перевода ---");
        Console.Write("Номер счета: ");
        string accountNumber = Console.ReadLine();
        
        Console.Write("Название банка: ");
        string bankName = Console.ReadLine();
        
        Console.Write("Имя получателя: ");
        string recipientName = Console.ReadLine();

        return new BankTransferPaymentStrategy(accountNumber, bankName, recipientName);
    }
}
