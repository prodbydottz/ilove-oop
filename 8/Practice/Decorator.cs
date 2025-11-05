using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IReport
{
    string Generate();
    string GetDescription();
}

public class SalesReport : IReport
{
    private List<Sale> _sales;

    public SalesReport()
    {
        _sales = GenerateSampleSalesData();
    }

    public string Generate()
    {
        var report = new StringBuilder();
        report.AppendLine("=== ОТЧЕТ ПО ПРОДАЖАМ ===");
        report.AppendLine($"Всего продаж: {_sales.Count}");
        report.AppendLine($"Общая сумма: {_sales.Sum(s => s.Amount):C}");
        report.AppendLine();
        
        foreach (var sale in _sales)
        {
            report.AppendLine($"{sale.Date:dd.MM.yyyy} | {sale.Product} | {sale.Amount:C} | {sale.Customer}");
        }
        
        return report.ToString();
    }

    public string GetDescription()
    {
        return "Базовый отчет по продажам";
    }

    private List<Sale> GenerateSampleSalesData()
    {
        return new List<Sale>
        {
            new Sale { Date = new DateTime(2024, 1, 15), Product = "Ноутбук", Amount = 1500, Customer = "Эмир Молдахулов" },
            new Sale { Date = new DateTime(2024, 1, 16), Product = "Мышь", Amount = 25, Customer = "Али Кожабек" },
            new Sale { Date = new DateTime(2024, 1, 17), Product = "Клавиатура", Amount = 75, Customer = "Казимир Казимирович" },
            new Sale { Date = new DateTime(2024, 1, 18), Product = "Монитор", Amount = 300, Customer = "Алексей Хан" },
            new Sale { Date = new DateTime(2024, 1, 19), Product = "Наушники", Amount = 150, Customer = "Елена Мерседес" },
            new Sale { Date = new DateTime(2024, 2, 1), Product = "Ноутбук", Amount = 1200, Customer = "Дмитрий БМВ" },
            new Sale { Date = new DateTime(2024, 2, 2), Product = "Планшет", Amount = 800, Customer = "Ольга Ольгина" },
            new Sale { Date = new DateTime(2024, 2, 3), Product = "Телефон", Amount = 600, Customer = "Сергей Мороз" }
        };
    }
}

public class UserReport : IReport
{
    private List<User> _users;

    public UserReport()
    {
        _users = GenerateSampleUserData();
    }

    public string Generate()
    {
        var report = new StringBuilder();
        report.AppendLine("=== ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ ===");
        report.AppendLine($"Всего пользователей: {_users.Count}");
        report.AppendLine($"Средний возраст: {_users.Average(u => u.Age):F1}");
        report.AppendLine();
        
        foreach (var user in _users)
        {
            report.AppendLine($"{user.Name} | {user.Email} | {user.Age} лет | {user.RegistrationDate:dd.MM.yyyy} | {user.Status}");
        }
        
        return report.ToString();
    }

    public string GetDescription()
    {
        return "Базовый отчет по пользователям";
    }

    private List<User> GenerateSampleUserData()
    {
        return new List<User>
        {
            new User { Name = "Эмир Молдахулов", Email = "emir.m@mail.com", Age = 20, RegistrationDate = new DateTime(2023, 5, 10), Status = "Активный" },
            new User { Name = "Али Кожабек", Email = "alishka@mail.com", Age = 20, RegistrationDate = new DateTime(2023, 6, 15), Status = "Активный" },
            new User { Name = "Казимир Казимирович", Email = "kazimirzalupenko@mail.com", Age = 44, RegistrationDate = new DateTime(2024, 1, 5), Status = "Новый" },
            new User { Name = "Алексей Хан", Email = "alex@mail.com", Age = 20, RegistrationDate = new DateTime(2022, 12, 20), Status = "Активный" },
            new User { Name = "Елена Мерседес", Email = "elena@mail.com", Age = 31, RegistrationDate = new DateTime(2023, 8, 30), Status = "Неактивный" },
            new User { Name = "Дмитрий БМВ", Email = "dmitry@mail.com", Age = 26, RegistrationDate = new DateTime(2024, 1, 25), Status = "Новый" }
        };
    }
}

public abstract class ReportDecorator : IReport
{
    protected IReport _report;

    protected ReportDecorator(IReport report)
    {
        _report = report ?? throw new ArgumentNullException(nameof(report));
    }

    public virtual string Generate()
    {
        return _report.Generate();
    }

    public virtual string GetDescription()
    {
        return _report.GetDescription();
    }
}

public class DateFilterDecorator : ReportDecorator
{
    private DateTime _startDate;
    private DateTime _endDate;

    public DateFilterDecorator(IReport report, DateTime startDate, DateTime endDate) : base(report)
    {
        _startDate = startDate;
        _endDate = endDate;
    }

    public override string Generate()
    {
        var originalReport = _report.Generate();
        
        if (_report is SalesReport)
        {
            return ApplySalesDateFilter(originalReport);
        }
        else if (_report is UserReport)
        {
            return ApplyUserDateFilter(originalReport);
        }
        
        return originalReport;
    }

    public override string GetDescription()
    {
        return $"{_report.GetDescription()} + Фильтр по дате ({_startDate:dd.MM.yyyy} - {_endDate:dd.MM.yyyy})";
    }

    private string ApplySalesDateFilter(string report)
    {
        var salesReport = _report as SalesReport;
        var filteredData = salesReport.GetType().GetField("_sales", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(salesReport) as List<Sale>;
        
        if (filteredData != null)
        {
            filteredData = filteredData.Where(s => s.Date >= _startDate && s.Date <= _endDate).ToList();
        }

        var filteredReport = new StringBuilder();
        filteredReport.AppendLine($"=== ОТЧЕТ ПО ПРОДАЖАМ (фильтр: {_startDate:dd.MM.yyyy} - {_endDate:dd.MM.yyyy}) ===");
        filteredReport.AppendLine($"Всего продаж: {filteredData?.Count ?? 0}");
        filteredReport.AppendLine($"Общая сумма: {filteredData?.Sum(s => s.Amount):C}");
        filteredReport.AppendLine();
        
        foreach (var sale in filteredData ?? new List<Sale>())
        {
            filteredReport.AppendLine($"{sale.Date:dd.MM.yyyy} | {sale.Product} | {sale.Amount:C} | {sale.Customer}");
        }
        
        return filteredReport.ToString();
    }

    private string ApplyUserDateFilter(string report)
    {
        var userReport = _report as UserReport;
        var filteredData = userReport.GetType().GetField("_users", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(userReport) as List<User>;
        
        if (filteredData != null)
        {
            filteredData = filteredData.Where(u => u.RegistrationDate >= _startDate && u.RegistrationDate <= _endDate).ToList();
        }

        var filteredReport = new StringBuilder();
        filteredReport.AppendLine($"=== ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ (фильтр: {_startDate:dd.MM.yyyy} - {_endDate:dd.MM.yyyy}) ===");
        filteredReport.AppendLine($"Всего пользователей: {filteredData?.Count ?? 0}");
        filteredReport.AppendLine();
        
        foreach (var user in filteredData ?? new List<User>())
        {
            filteredReport.AppendLine($"{user.Name} | {user.Email} | {user.Age} лет | {user.RegistrationDate:dd.MM.yyyy} | {user.Status}");
        }
        
        return filteredReport.ToString();
    }
}

public class SortingDecorator : ReportDecorator
{
    private string _sortBy;
    private bool _ascending;

    public SortingDecorator(IReport report, string sortBy, bool ascending = true) : base(report)
    {
        _sortBy = sortBy;
        _ascending = ascending;
    }

    public override string Generate()
    {
        var originalReport = _report.Generate();
        
        if (_report is SalesReport)
        {
            return ApplySalesSorting(originalReport);
        }
        else if (_report is UserReport)
        {
            return ApplyUserSorting(originalReport);
        }
        
        return originalReport;
    }

    public override string GetDescription()
    {
        string order = _ascending ? "по возрастанию" : "по убыванию";
        return $"{_report.GetDescription()} + Сортировка по {_sortBy} ({order})";
    }

    private string ApplySalesSorting(string report)
    {
        var salesReport = _report as SalesReport;
        var sortedData = salesReport.GetType().GetField("_sales", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(salesReport) as List<Sale>;
        
        if (sortedData != null)
        {
            switch (_sortBy.ToLower())
            {
                case "date":
                    sortedData = _ascending ? sortedData.OrderBy(s => s.Date).ToList() : sortedData.OrderByDescending(s => s.Date).ToList();
                    break;
                case "amount":
                    sortedData = _ascending ? sortedData.OrderBy(s => s.Amount).ToList() : sortedData.OrderByDescending(s => s.Amount).ToList();
                    break;
                case "product":
                    sortedData = _ascending ? sortedData.OrderBy(s => s.Product).ToList() : sortedData.OrderByDescending(s => s.Product).ToList();
                    break;
            }
        }

        var sortedReport = new StringBuilder();
        sortedReport.AppendLine($"=== ОТЧЕТ ПО ПРОДАЖАМ (сортировка: {_sortBy}) ===");
        sortedReport.AppendLine($"Всего продаж: {sortedData?.Count ?? 0}");
        sortedReport.AppendLine();
        
        foreach (var sale in sortedData ?? new List<Sale>())
        {
            sortedReport.AppendLine($"{sale.Date:dd.MM.yyyy} | {sale.Product} | {sale.Amount:C} | {sale.Customer}");
        }
        
        return sortedReport.ToString();
    }

    private string ApplyUserSorting(string report)
    {
        var userReport = _report as UserReport;
        var sortedData = userReport.GetType().GetField("_users", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(userReport) as List<User>;
        
        if (sortedData != null)
        {
            switch (_sortBy.ToLower())
            {
                case "name":
                    sortedData = _ascending ? sortedData.OrderBy(u => u.Name).ToList() : sortedData.OrderByDescending(u => u.Name).ToList();
                    break;
                case "age":
                    sortedData = _ascending ? sortedData.OrderBy(u => u.Age).ToList() : sortedData.OrderByDescending(u => u.Age).ToList();
                    break;
                case "registrationdate":
                    sortedData = _ascending ? sortedData.OrderBy(u => u.RegistrationDate).ToList() : sortedData.OrderByDescending(u => u.RegistrationDate).ToList();
                    break;
            }
        }

        var sortedReport = new StringBuilder();
        sortedReport.AppendLine($"=== ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ (сортировка: {_sortBy}) ===");
        sortedReport.AppendLine($"Всего пользователей: {sortedData?.Count ?? 0}");
        sortedReport.AppendLine();
        
        foreach (var user in sortedData ?? new List<User>())
        {
            sortedReport.AppendLine($"{user.Name} | {user.Email} | {user.Age} лет | {user.RegistrationDate:dd.MM.yyyy} | {user.Status}");
        }
        
        return sortedReport.ToString();
    }
}

public class AmountFilterDecorator : ReportDecorator
{
    private decimal _minAmount;
    private decimal _maxAmount;

    public AmountFilterDecorator(IReport report, decimal minAmount, decimal maxAmount) : base(report)
    {
        _minAmount = minAmount;
        _maxAmount = maxAmount;
    }

    public override string Generate()
    {
        if (_report is SalesReport)
        {
            return ApplyAmountFilter();
        }
        
        return _report.Generate();
    }

    public override string GetDescription()
    {
        return $"{_report.GetDescription()} + Фильтр по сумме ({_minAmount:C} - {_maxAmount:C})";
    }

    private string ApplyAmountFilter()
    {
        var salesReport = _report as SalesReport;
        var filteredData = salesReport.GetType().GetField("_sales", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(salesReport) as List<Sale>;
        
        if (filteredData != null)
        {
            filteredData = filteredData.Where(s => s.Amount >= _minAmount && s.Amount <= _maxAmount).ToList();
        }

        var filteredReport = new StringBuilder();
        filteredReport.AppendLine($"=== ОТЧЕТ ПО ПРОДАЖАМ (фильтр по сумме: {_minAmount:C} - {_maxAmount:C}) ===");
        filteredReport.AppendLine($"Всего продаж: {filteredData?.Count ?? 0}");
        filteredReport.AppendLine($"Общая сумма: {filteredData?.Sum(s => s.Amount):C}");
        filteredReport.AppendLine();
        
        foreach (var sale in filteredData ?? new List<Sale>())
        {
            filteredReport.AppendLine($"{sale.Date:dd.MM.yyyy} | {sale.Product} | {sale.Amount:C} | {sale.Customer}");
        }
        
        return filteredReport.ToString();
    }
}

public class StatusFilterDecorator : ReportDecorator
{
    private string _status;

    public StatusFilterDecorator(IReport report, string status) : base(report)
    {
        _status = status;
    }

    public override string Generate()
    {
        if (_report is UserReport)
        {
            return ApplyStatusFilter();
        }
        
        return _report.Generate();
    }

    public override string GetDescription()
    {
        return $"{_report.GetDescription()} + Фильтр по статусу ({_status})";
    }

    private string ApplyStatusFilter()
    {
        var userReport = _report as UserReport;
        var filteredData = userReport.GetType().GetField("_users", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .GetValue(userReport) as List<User>;
        
        if (filteredData != null)
        {
            filteredData = filteredData.Where(u => u.Status.Equals(_status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var filteredReport = new StringBuilder();
        filteredReport.AppendLine($"=== ОТЧЕТ ПО ПОЛЬЗОВАТЕЛЯМ (фильтр по статусу: {_status}) ===");
        filteredReport.AppendLine($"Всего пользователей: {filteredData?.Count ?? 0}");
        filteredReport.AppendLine();
        
        foreach (var user in filteredData ?? new List<User>())
        {
            filteredReport.AppendLine($"{user.Name} | {user.Email} | {user.Age} лет | {user.RegistrationDate:dd.MM.yyyy} | {user.Status}");
        }
        
        return filteredReport.ToString();
    }
}

public class CsvExportDecorator : ReportDecorator
{
    public CsvExportDecorator(IReport report) : base(report)
    {
    }

    public override string Generate()
    {
        var originalReport = _report.Generate();
        return ConvertToCsv(originalReport);
    }

    public override string GetDescription()
    {
        return $"{_report.GetDescription()} + Экспорт в CSV";
    }

    private string ConvertToCsv(string report)
    {
        var lines = report.Split('\n');
        var csvBuilder = new StringBuilder();
        
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("===") && !line.StartsWith("Всего"))
            {
                var csvLine = line.Replace(" | ", ",").Replace(":", "").Replace("C", "").Trim();
                csvBuilder.AppendLine(csvLine);
            }
        }
        
        return csvBuilder.ToString();
    }
}

public class PdfExportDecorator : ReportDecorator
{
    public PdfExportDecorator(IReport report) : base(report)
    {
    }

    public override string Generate()
    {
        var originalReport = _report.Generate();
        return FormatAsPdf(originalReport);
    }

    public override string GetDescription()
    {
        return $"{_report.GetDescription()} + Экспорт в PDF";
    }

    private string FormatAsPdf(string report)
    {
        var pdfBuilder = new StringBuilder();
        pdfBuilder.AppendLine("%PDF-1.4");
        pdfBuilder.AppendLine("1 0 obj");
        pdfBuilder.AppendLine("<< /Type /Catalog /Pages 2 0 R >>");
        pdfBuilder.AppendLine("endobj");
        pdfBuilder.AppendLine();
        pdfBuilder.AppendLine("2 0 obj");
        pdfBuilder.AppendLine("<< /Type /Pages /Kids [3 0 R] /Count 1 >>");
        pdfBuilder.AppendLine("endobj");
        pdfBuilder.AppendLine();
        pdfBuilder.AppendLine("3 0 obj");
        pdfBuilder.AppendLine("<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R >>");
        pdfBuilder.AppendLine("endobj");
        pdfBuilder.AppendLine();
        pdfBuilder.AppendLine("4 0 obj");
        pdfBuilder.AppendLine("<< /Length 100 >>");
        pdfBuilder.AppendLine("stream");
        pdfBuilder.AppendLine("BT");
        pdfBuilder.AppendLine("/F1 12 Tf");
        pdfBuilder.AppendLine("50 750 Td");
        
        var lines = report.Split('\n');
        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                pdfBuilder.AppendLine($"({EscapePdfString(line)}) Tj");
                pdfBuilder.AppendLine("0 -15 Td");
            }
        }
        
        pdfBuilder.AppendLine("ET");
        pdfBuilder.AppendLine("endstream");
        pdfBuilder.AppendLine("endobj");
        pdfBuilder.AppendLine("xref");
        pdfBuilder.AppendLine("trailer");
        pdfBuilder.AppendLine("<< /Size 5 /Root 1 0 R >>");
        pdfBuilder.AppendLine("startxref");
        pdfBuilder.AppendLine("%%EOF");
        
        return pdfBuilder.ToString();
    }

    private string EscapePdfString(string text)
    {
        return text.Replace("(", "\\(").Replace(")", "\\)").Replace("\\", "\\\\");
    }
}

public class Sale
{
    public DateTime Date { get; set; }
    public string Product { get; set; }
    public decimal Amount { get; set; }
    public string Customer { get; set; }
}

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Status { get; set; }
}

public class ReportBuilder
{
    private IReport _report;

    public ReportBuilder(IReport baseReport)
    {
        _report = baseReport;
    }

    public ReportBuilder AddDateFilter(DateTime startDate, DateTime endDate)
    {
        _report = new DateFilterDecorator(_report, startDate, endDate);
        return this;
    }

    public ReportBuilder AddSorting(string sortBy, bool ascending = true)
    {
        _report = new SortingDecorator(_report, sortBy, ascending);
        return this;
    }

    public ReportBuilder AddAmountFilter(decimal minAmount, decimal maxAmount)
    {
        _report = new AmountFilterDecorator(_report, minAmount, maxAmount);
        return this;
    }

    public ReportBuilder AddStatusFilter(string status)
    {
        _report = new StatusFilterDecorator(_report, status);
        return this;
    }

    public ReportBuilder AddCsvExport()
    {
        _report = new CsvExportDecorator(_report);
        return this;
    }

    public ReportBuilder AddPdfExport()
    {
        _report = new PdfExportDecorator(_report);
        return this;
    }

    public IReport Build()
    {
        return _report;
    }
}

class ReportSystemProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ОТЧЕТНОСТЬЮ ===\n");

        while (true)
        {
            Console.WriteLine("Выберите тип отчета:");
            Console.WriteLine("1 - Отчет по продажам");
            Console.WriteLine("2 - Отчет по пользователям");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            if (choice == "0") break;

            IReport report = null;
            ReportBuilder builder = null;

            if (choice == "1")
            {
                report = new SalesReport();
                builder = new ReportBuilder(report);
                ConfigureSalesReport(builder);
            }
            else if (choice == "2")
            {
                report = new UserReport();
                builder = new ReportBuilder(report);
                ConfigureUserReport(builder);
            }
            else
            {
                Console.WriteLine("Неверный выбор");
                continue;
            }

            var finalReport = builder.Build();
            Console.WriteLine($"\n=== СФОРМИРОВАННЫЙ ОТЧЕТ: {finalReport.GetDescription()} ===");
            Console.WriteLine(finalReport.Generate());
            Console.WriteLine("\n" + new string('=', 50));
        }
    }

    static void ConfigureSalesReport(ReportBuilder builder)
    {
        Console.WriteLine("\nНастройка отчета по продажам:");
        
        Console.Write("Применить фильтр по дате? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Начальная дата (дд.мм.гггг): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Конечная дата (дд.мм.гггг): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());
            builder.AddDateFilter(startDate, endDate);
        }

        Console.Write("Применить сортировку? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Поле для сортировки (date/amount/product): ");
            string sortBy = Console.ReadLine();
            Console.Write("По возрастанию? (y/n): ");
            bool ascending = Console.ReadLine().ToLower() == "y";
            builder.AddSorting(sortBy, ascending);
        }

        Console.Write("Применить фильтр по сумме? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Минимальная сумма: ");
            decimal minAmount = decimal.Parse(Console.ReadLine());
            Console.Write("Максимальная сумма: ");
            decimal maxAmount = decimal.Parse(Console.ReadLine());
            builder.AddAmountFilter(minAmount, maxAmount);
        }

        Console.Write("Экспортировать в CSV? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            builder.AddCsvExport();
        }

        Console.Write("Экспортировать в PDF? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            builder.AddPdfExport();
        }
    }

    static void ConfigureUserReport(ReportBuilder builder)
    {
        Console.WriteLine("\nНастройка отчета по пользователям:");
        
        Console.Write("Применить фильтр по дате регистрации? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Начальная дата (дд.мм.гггг): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Конечная дата (дд.мм.гггг): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());
            builder.AddDateFilter(startDate, endDate);
        }

        Console.Write("Применить сортировку? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Поле для сортировки (name/age/registrationdate): ");
            string sortBy = Console.ReadLine();
            Console.Write("По возрастанию? (y/n): ");
            bool ascending = Console.ReadLine().ToLower() == "y";
            builder.AddSorting(sortBy, ascending);
        }

        Console.Write("Применить фильтр по статусу? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            Console.Write("Статус (Активный/Новый/Неактивный): ");
            string status = Console.ReadLine();
            builder.AddStatusFilter(status);
        }

        Console.Write("Экспортировать в CSV? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            builder.AddCsvExport();
        }
    }
}
