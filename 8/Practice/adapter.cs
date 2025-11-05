using System;
using System.Collections.Generic;

public interface IInternalDeliveryService
{
    void DeliverOrder(string orderId);
    string GetDeliveryStatus(string orderId);
    decimal CalculateCost(string orderId);
    void LogDelivery(string orderId, string serviceName);
}

public class InternalDeliveryService : IInternalDeliveryService
{
    private Dictionary<string, DeliveryInfo> _deliveries;

    public InternalDeliveryService()
    {
        _deliveries = new Dictionary<string, DeliveryInfo>();
    }

    public void DeliverOrder(string orderId)
    {
        try
        {
            Console.WriteLine($"Внутренняя служба: начало доставки заказа {orderId}");
            
            if (_deliveries.ContainsKey(orderId))
            {
                throw new InvalidOperationException($"Заказ {orderId} уже едет, брат");
            }

            var deliveryInfo = new DeliveryInfo
            {
                OrderId = orderId,
                Status = "В пути",
                EstimatedDelivery = DateTime.Now.AddDays(2),
                Cost = CalculateCost(orderId)
            };

            _deliveries[orderId] = deliveryInfo;
            LogDelivery(orderId, "InternalDeliveryService");
            
            Console.WriteLine($"Внутренняя служба: заказ {orderId} принят в доставку");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при доставке заказа {orderId}: {ex.Message}");
            throw;
        }
    }

    public string GetDeliveryStatus(string orderId)
    {
        if (_deliveries.ContainsKey(orderId))
        {
            return _deliveries[orderId].Status;
        }
        
        throw new KeyNotFoundException($"Заказ {orderId} не найден");
    }

    public decimal CalculateCost(string orderId)
    {
        return 100 + (orderId.GetHashCode() % 50);
    }

    public void LogDelivery(string orderId, string serviceName)
    {
        Console.WriteLine($"[ЛОГ] {DateTime.Now:dd.MM.yyyy HH:mm:ss} - Заказ {orderId} обработан службой {serviceName}");
    }
}

public class ExternalLogisticsServiceA
{
    private Dictionary<int, string> _shipments;

    public ExternalLogisticsServiceA()
    {
        _shipments = new Dictionary<int, string>();
    }

    public void ShipItem(int itemId)
    {
        Console.WriteLine($"Служба A: отправляем товар с ID {itemId}");
        _shipments[itemId] = "Отправлено";
    }

    public string TrackShipment(int shipmentId)
    {
        if (_shipments.ContainsKey(shipmentId))
        {
            return _shipments[shipmentId];
        }
        
        return "Отправление не найдено";
    }

    public double GetShippingPrice(int itemId)
    {
        return 75.50 + (itemId % 30);
    }
}

public class ExternalLogisticsServiceB
{
    private Dictionary<string, PackageInfo> _packages;

    public ExternalLogisticsServiceB()
    {
        _packages = new Dictionary<string, PackageInfo>();
    }

    public void SendPackage(string packageInfo)
    {
        Console.WriteLine($"Служба B: отправляем посылку {packageInfo}");
        
        var trackingCode = $"B{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        _packages[trackingCode] = new PackageInfo
        {
            TrackingCode = trackingCode,
            Status = "В обработке",
            Info = packageInfo
        };
    }

    public string CheckPackageStatus(string trackingCode)
    {
        if (_packages.ContainsKey(trackingCode))
        {
            return _packages[trackingCode].Status;
        }
        
        return "Посылка не найдена";
    }

    public decimal CalculateShippingCost(string packageInfo)
    {
        return 120 + (packageInfo.Length * 2);
    }
}

public class ExternalLogisticsServiceC
{
    private List<ExpressDelivery> _expressDeliveries;

    public ExternalLogisticsServiceC()
    {
        _expressDeliveries = new List<ExpressDelivery>();
    }

    public void StartExpressDelivery(string deliveryId, string address)
    {
        Console.WriteLine($"Служба C: начинаем экспресс-доставку {deliveryId} по адресу {address}");
        
        _expressDeliveries.Add(new ExpressDelivery
        {
            DeliveryId = deliveryId,
            Address = address,
            Status = "Экспресс доставка активирована",
            StartTime = DateTime.Now
        });
    }

    public string GetExpressDeliveryStatus(string deliveryId)
    {
        var delivery = _expressDeliveries.Find(d => d.DeliveryId == deliveryId);
        return delivery?.Status ?? "Экспресс доставка не найдена";
    }

    public float ComputeExpressCost(string deliveryId, bool isUrgent)
    {
        float baseCost = 200.0f;
        if (isUrgent)
        {
            baseCost *= 1.5f;
        }
        return baseCost;
    }
}

public class LogisticsAdapterA : IInternalDeliveryService
{
    private ExternalLogisticsServiceA _externalService;
    private Dictionary<string, int> _orderMappings;

    public LogisticsAdapterA(ExternalLogisticsServiceA externalService)
    {
        _externalService = externalService ?? throw new ArgumentNullException(nameof(externalService));
        _orderMappings = new Dictionary<string, int>();
    }

    public void DeliverOrder(string orderId)
    {
        try
        {
            Console.WriteLine($"Адаптер A: преобразуем заказ {orderId} для внешней службы");
            
            int itemId = ConvertOrderIdToItemId(orderId);
            _externalService.ShipItem(itemId);
            _orderMappings[orderId] = itemId;
            
            LogDelivery(orderId, "LogisticsAdapterA");
            Console.WriteLine($"Адаптер A: заказ {orderId} успешно передан во внешнюю службу");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка адаптера A при доставке заказа {orderId}: {ex.Message}");
            throw;
        }
    }

    public string GetDeliveryStatus(string orderId)
    {
        if (_orderMappings.ContainsKey(orderId))
        {
            int itemId = _orderMappings[orderId];
            return _externalService.TrackShipment(itemId);
        }
        
        throw new KeyNotFoundException($"Заказ {orderId} не найден в адаптере A");
    }

    public decimal CalculateCost(string orderId)
    {
        int itemId = ConvertOrderIdToItemId(orderId);
        return (decimal)_externalService.GetShippingPrice(itemId);
    }

    public void LogDelivery(string orderId, string serviceName)
    {
        Console.WriteLine($"[ЛОГ] {DateTime.Now:dd.MM.yyyy HH:mm:ss} - Заказ {orderId} адаптирован через {serviceName}");
    }

    private int ConvertOrderIdToItemId(string orderId)
    {
        return Math.Abs(orderId.GetHashCode()) % 10000;
    }
}

public class LogisticsAdapterB : IInternalDeliveryService
{
    private ExternalLogisticsServiceB _externalService;
    private Dictionary<string, string> _orderMappings;

    public LogisticsAdapterB(ExternalLogisticsServiceB externalService)
    {
        _externalService = externalService ?? throw new ArgumentNullException(nameof(externalService));
        _orderMappings = new Dictionary<string, string>();
    }

    public void DeliverOrder(string orderId)
    {
        try
        {
            Console.WriteLine($"Адаптер B: преобразуем заказ {orderId} для внешней службы");
            
            string packageInfo = $"Заказ_{orderId}_Пользователь_данные";
            _externalService.SendPackage(packageInfo);
            
            string trackingCode = GetTrackingCodeForOrder(orderId);
            _orderMappings[orderId] = trackingCode;
            
            LogDelivery(orderId, "LogisticsAdapterB");
            Console.WriteLine($"Адаптер B: заказ {orderId} успешно передан во внешнюю службу");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка адаптера B при доставке заказа {orderId}: {ex.Message}");
            throw;
        }
    }

    public string GetDeliveryStatus(string orderId)
    {
        if (_orderMappings.ContainsKey(orderId))
        {
            string trackingCode = _orderMappings[orderId];
            return _externalService.CheckPackageStatus(trackingCode);
        }
        
        throw new KeyNotFoundException($"Заказ {orderId} не найден в адаптере B");
    }

    public decimal CalculateCost(string orderId)
    {
        string packageInfo = $"Заказ_{orderId}_Пользователь_данные";
        return _externalService.CalculateShippingCost(packageInfo);
    }

    public void LogDelivery(string orderId, string serviceName)
    {
        Console.WriteLine($"[ЛОГ] {DateTime.Now:dd.MM.yyyy HH:mm:ss} - Заказ {orderId} адаптирован через {serviceName}");
    }

    private string GetTrackingCodeForOrder(string orderId)
    {
        return $"B{orderId.GetHashCode():X8}";
    }
}

public class LogisticsAdapterC : IInternalDeliveryService
{
    private ExternalLogisticsServiceC _externalService;
    private Dictionary<string, string> _deliveryMappings;

    public LogisticsAdapterC(ExternalLogisticsServiceC externalService)
    {
        _externalService = externalService ?? throw new ArgumentNullException(nameof(externalService));
        _deliveryMappings = new Dictionary<string, string>();
    }

    public void DeliverOrder(string orderId)
    {
        try
        {
            Console.WriteLine($"Адаптер C: преобразуем заказ {orderId} для экспресс-службы");
            
            string deliveryId = $"EXP_{orderId}";
            string address = "Адрес доставки по умолчанию";
            
            _externalService.StartExpressDelivery(deliveryId, address);
            _deliveryMappings[orderId] = deliveryId;
            
            LogDelivery(orderId, "LogisticsAdapterC");
            Console.WriteLine($"Адаптер C: заказ {orderId} передан в экспресс-службу");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка адаптера C при доставке заказа {orderId}: {ex.Message}");
            throw;
        }
    }

    public string GetDeliveryStatus(string orderId)
    {
        if (_deliveryMappings.ContainsKey(orderId))
        {
