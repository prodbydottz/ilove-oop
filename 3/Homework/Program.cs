class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SRP Demo ===");
        var order = new Order { ProductName = "Laptop", Quantity = 2, Price = 1000 };
        var priceCalculator = new PriceCalculator();
        var paymentProcessor = new PaymentProcessor();
        var notificationService = new NotificationService();
        
        Console.WriteLine($"Total: {priceCalculator.CalculateTotalPrice(order)}");
        paymentProcessor.ProcessPayment("Credit Card");
        notificationService.SendConfirmationEmail("test@mail.com");

        Console.WriteLine("\n=== OCP Demo ===");
        var salaryCalculator = new EmployeeSalaryCalculator();
        var employee = new Employee { Name = "John", BaseSalary = 50000 };
        
        // Добавляем новый тип БЕЗ изменения существующего кода
        salaryCalculator.AddCalculator("Freelancer", new FreelancerCalculator());
        
        Console.WriteLine($"Permanent: {salaryCalculator.CalculateSalary(employee, "Permanent")}");
        Console.WriteLine($"Freelancer: {salaryCalculator.CalculateSalary(employee, "Freelancer")}");

        Console.WriteLine("\n=== ISP Demo ===");
        IPrinter basicPrinter = new BasicPrinter();
        basicPrinter.Print("Document"); // Работает без лишних методов
        
        IPrinter allInOne = new AllInOnePrinter();
        allInOne.Print("Document");

        Console.WriteLine("\n=== DIP Demo ===");
        var notificationService2 = new NotificationService(
            new EmailSender(),
            new SmsSender(),
            new PushNotificationSender() // НОВЫЙ тип без изменения кода
        );
        notificationService2.SendNotification("Hello SOLID!");
    }
}
