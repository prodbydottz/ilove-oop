using System;
using System.Collections.Generic;
using System.IO;

public abstract class ReportGenerator
{
    protected List<string> _log;
    protected string _reportData;

    public ReportGenerator()
    {
        _log = new List<string>();
        _reportData = "Пример данных отчета: продажи за квартал, статистика пользователей, финансовые показатели";
    }

    public void GenerateReport()
    {
        LogStep("Начало генерации отчета");
        
        PrepareData();
        ValidateData();
        FormatHeader();
        FormatBody();
        FormatFooter();
        
        if (CustomerWantsSave())
        {
            SaveReport();
        }
        
        if (CustomerWantsEmail())
        {
            SendEmail();
        }
        
        LogStep("Завершение генерации отчета");
        DisplayLog();
    }

    private void PrepareData()
    {
        LogStep("Подготовка данных отчета");
        Console.WriteLine("Подготавливаем данные для отчета...");
    }

    private void ValidateData()
    {
        LogStep("Валидация данных");
        Console.WriteLine("Проверяем корректность данных...");
        
        if (string.IsNullOrEmpty(_reportData))
        {
            throw new InvalidOperationException("Данные отчета отсутствуют");
        }
    }

    protected abstract void FormatHeader();
    protected abstract void FormatBody();
    protected abstract void FormatFooter();
    protected abstract void SaveReport();

    protected virtual bool CustomerWantsSave()
    {
        Console.Write("Сохранить отчет? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "y";
        return input == "y" || input == "yes" || input == "д";
    }

    protected virtual bool CustomerWantsEmail()
    {
        return false;
    }

    protected virtual void SendEmail()
    {
    }

    protected void LogStep(string step)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] {step}";
        _log.Add(logEntry);
        Console.WriteLine(logEntry);
    }

    protected void DisplayLog()
    {
        Console.WriteLine("\n=== Журнал генерации отчета ===");
        foreach (var entry in _log)
        {
            Console.WriteLine(entry);
        }
    }

    public abstract string GetReportType();
}

public class PdfReport : ReportGenerator
{
    protected override void FormatHeader()
    {
        LogStep("Форматирование заголовка PDF");
        Console.WriteLine("Создаем заголовок отчета в формате PDF...");
        Console.WriteLine("=== ОТЧЕТ В ФОРМАТЕ PDF ===");
    }

    protected override void FormatBody()
    {
        LogStep("Форматирование тела PDF");
        Console.WriteLine("Форматируем данные для PDF...");
        Console.WriteLine($"Данные: {_reportData}");
        Console.WriteLine("Применяем стили PDF...");
    }

    protected override void FormatFooter()
    {
        LogStep("Форматирование подвала PDF");
        Console.WriteLine("Добавляем нумерацию страниц...");
        Console.WriteLine("=== КОНЕЦ ОТЧЕТА PDF ===");
    }

    protected override void SaveReport()
    {
        LogStep("Сохранение PDF отчета");
        string filename = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        Console.WriteLine($"Сохраняем отчет как: {filename}");
        File.WriteAllText(filename, "PDF отчет сгенерирован успешно");
    }

    public override string GetReportType()
    {
        return "PDF отчет";
    }
}

public class ExcelReport : ReportGenerator
{
    protected override void FormatHeader()
    {
        LogStep("Форматирование заголовка Excel");
        Console.WriteLine("Создаем заголовок отчета в формате Excel...");
        Console.WriteLine("=== ОТЧЕТ В ФОРМАТЕ EXCEL ===");
    }

    protected override void FormatBody()
    {
        LogStep("Форматирование тела Excel");
        Console.WriteLine("Создаем таблицы и формулы Excel...");
        Console.WriteLine($"Данные: {_reportData}");
        Console.WriteLine("Добавляем вычисляемые поля...");
    }

    protected override void FormatFooter()
    {
        LogStep("Форматирование подвала Excel");
        Console.WriteLine("Добавляем итоговые строки...");
        Console.WriteLine("=== КОНЕЦ ОТЧЕТА EXCEL ===");
    }

    protected override void SaveReport()
    {
        LogStep("Сохранение Excel отчета");
        string filename = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
        Console.WriteLine($"Сохраняем отчет как: {filename}");
        File.WriteAllText(filename, "Excel отчет сгенерирован успешно");
    }

    protected override bool CustomerWantsSave()
    {
        Console.Write("Сохранить Excel отчет? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "y";
        
        if (input != "y" && input != "yes" && input != "д" && input != "n" && input != "no" && input != "н")
        {
            Console.WriteLine("Неверный ввод. Используется значение по умолчанию: да");
            return true;
        }
        
        return input == "y" || input == "yes" || input == "д";
    }

    public override string GetReportType()
    {
        return "Excel отчет";
    }
}

public class HtmlReport : ReportGenerator
{
    protected override void FormatHeader()
    {
        LogStep("Форматирование заголовка HTML");
        Console.WriteLine("Создаем HTML заголовок...");
        Console.WriteLine("<h1>ОТЧЕТ В ФОРМАТЕ HTML</h1>");
    }

    protected override void FormatBody()
    {
        LogStep("Форматирование тела HTML");
        Console.WriteLine("Создаем HTML таблицы...");
        Console.WriteLine($"<p>Данные: {_reportData}</p>");
        Console.WriteLine("Применяем CSS стили...");
    }

    protected override void FormatFooter()
    {
        LogStep("Форматирование подвала HTML");
        Console.WriteLine("Добавляем информацию о генерации...");
        Console.WriteLine("<footer>КОНЕЦ ОТЧЕТА HTML</footer>");
    }

    protected override void SaveReport()
    {
        LogStep("Сохранение HTML отчета");
        string filename = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
        Console.WriteLine($"Сохраняем отчет как: {filename}");
        File.WriteAllText(filename, "<html>HTML отчет сгенерирован успешно</html>");
    }

    protected override bool CustomerWantsEmail()
    {
        Console.Write("Отправить HTML отчет по email? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "n";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override void SendEmail()
    {
        LogStep("Отправка отчета по email");
        Console.WriteLine("Отправляем HTML отчет на email...");
        Console.WriteLine("Отчет успешно отправлен!");
    }

    public override string GetReportType()
    {
        return "HTML отчет";
    }
}

public class CsvReport : ReportGenerator
{
    protected override void FormatHeader()
    {
        LogStep("Форматирование заголовка CSV");
        Console.WriteLine("Создаем CSV заголовок...");
        Console.WriteLine("Колонка1,Колонка2,Колонка3");
    }

    protected override void FormatBody()
    {
        LogStep("Форматирование тела CSV");
        Console.WriteLine("Форматируем данные в CSV...");
        Console.WriteLine($"Данные1,Данные2,Данные3");
        Console.WriteLine("Разделяем значения запятыми...");
    }

    protected override void FormatFooter()
    {
        LogStep("Форматирование подвала CSV");
        Console.WriteLine("Добавляем итоговую строку...");
        Console.WriteLine("Итого,100,200");
    }

    protected override void SaveReport()
    {
        LogStep("Сохранение CSV отчета");
        string filename = $"report_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        Console.WriteLine($"Сохраняем отчет как: {filename}");
        File.WriteAllText(filename, "CSV отчет сгенерирован успешно");
    }

    protected override bool CustomerWantsEmail()
    {
        Console.Write("Отправить CSV отчет по email? (y/n): ");
        string input = Console.ReadLine()?.ToLower() ?? "n";
        return input == "y" || input == "yes" || input == "д";
    }

    protected override void SendEmail()
    {
        LogStep("Отправка CSV отчета по email");
        Console.WriteLine("Отправляем CSV отчет на email...");
        Console.WriteLine("CSV отчет успешно отправлен!");
    }

    public override string GetReportType()
    {
        return "CSV отчет";
    }
}

public class EmailReport : ReportGenerator
{
    private string _emailAddress;

    public EmailReport(string emailAddress)
    {
        _emailAddress = emailAddress ?? "default@company.com";
    }

    protected override void FormatHeader()
    {
        LogStep("Форматирование заголовка для email");
        Console.WriteLine("Подготавливаем заголовок для email отправки...");
        Console.WriteLine($"Тема: Отчет от {DateTime.Now:dd.MM.yyyy}");
    }

    protected override void FormatBody()
    {
        LogStep("Форматирование тела для email");
        Console.WriteLine("Форматируем данные для email...");
        Console.WriteLine($"Уважаемый получатель,\n\nПрилагаем отчет:\n{_reportData}");
    }

    protected override void FormatFooter()
    {
        LogStep("Форматирование подвала для email");
        Console.WriteLine("Добавляем подпись...");
        Console.WriteLine("\nС уважением,\nСистема отчетности");
    }

    protected override void SaveReport()
    {
        LogStep("Сохранение отчета для email");
        Console.WriteLine("Подготавливаем вложение для email...");
    }

    protected override bool CustomerWantsSave()
    {
        return false;
    }

    protected override bool CustomerWantsEmail()
    {
        return true;
    }

    protected override void SendEmail()
    {
        LogStep("Отправка отчета по email");
        Console.WriteLine($"Отправляем отчет на адрес: {_emailAddress}");
        Console.WriteLine("Отчет успешно отправлен по email!");
    }

    public override string GetReportType()
    {
        return "Email отчет";
    }
}

class ReportSystemProgram
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Система генерации отчетов ===\n");

        Dictionary<string, ReportGenerator> reports = new Dictionary<string, ReportGenerator>
        {
            ["1"] = new PdfReport(),
            ["2"] = new ExcelReport(),
            ["3"] = new HtmlReport(),
            ["4"] = new CsvReport(),
            ["5"] = new EmailReport("manager@company.com")
        };

        bool continueGenerating = true;

        while (continueGenerating)
        {
            Console.WriteLine("Выберите тип отчета:");
            Console.WriteLine("1 - PDF отчет");
            Console.WriteLine("2 - Excel отчет");
            Console.WriteLine("3 - HTML отчет");
            Console.WriteLine("4 - CSV отчет");
            Console.WriteLine("5 - Email отчет");
            Console.WriteLine("0 - Выход");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            if (choice == "0")
            {
                continueGenerating = false;
                continue;
            }

            if (reports.ContainsKey(choice))
            {
                try
                {
                    Console.WriteLine($"\n=== Генерация {reports[choice].GetReportType()} ===");
                    reports[choice].GenerateReport();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при генерации отчета: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор типа отчета");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }

        Console.WriteLine("Работа системы отчетов завершена");
    }
}
